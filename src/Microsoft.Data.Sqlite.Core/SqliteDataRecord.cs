﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Sqlite.Properties;
using SQLitePCL;

using static SQLitePCL.raw;

namespace Microsoft.Data.Sqlite
{
    internal class SqliteDataRecord : SqliteValueReader, IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly byte[][] _blobCache;
        private bool _stepped;

        public SqliteDataRecord(sqlite3_stmt stmt, bool hasRows, SqliteConnection connection)
        {
            Handle = stmt;
            HasRows = hasRows;
            _connection = connection;
            _blobCache = new byte[FieldCount][];
        }

        public virtual object this[string name]
            => GetValue(GetOrdinal(name));

        public virtual object this[int ordinal]
            => GetValue(ordinal);

        public override int FieldCount
            => sqlite3_column_count(Handle);

        public sqlite3_stmt Handle { get; }

        public bool HasRows { get; }

        public override bool IsDBNull(int ordinal)
            => !_stepped || sqlite3_data_count(Handle) == 0
                ? throw new InvalidOperationException(Resources.NoData)
                : base.IsDBNull(ordinal);

        public override object GetValue(int ordinal)
            => !_stepped || sqlite3_data_count(Handle) == 0
                ? throw new InvalidOperationException(Resources.NoData)
                : base.GetValue(ordinal);

        protected override double GetDoubleCore(int ordinal)
            => sqlite3_column_double(Handle, ordinal);

        protected override long GetInt64Core(int ordinal)
            => sqlite3_column_int64(Handle, ordinal);

        protected override string GetStringCore(int ordinal)
            => sqlite3_column_text(Handle, ordinal);

        protected override byte[] GetBlobCore(int ordinal)
            => sqlite3_column_blob(Handle, ordinal);

        protected override int GetSqliteType(int ordinal)
        {
            var type = sqlite3_column_type(Handle, ordinal);
            if (type == SQLITE_NULL
                && (ordinal < 0 || ordinal >= FieldCount))
            {
                // NB: Message is provided by the framework
                throw new ArgumentOutOfRangeException(nameof(ordinal), ordinal, message: null);
            }

            return type;
        }

        protected override T GetNull<T>(int ordinal)
            => typeof(T) == typeof(DBNull) || typeof(T) == typeof(object)
                ? (T)(object)DBNull.Value
                : throw new InvalidOperationException(GetOnNullErrorMsg(ordinal));

        public virtual string GetName(int ordinal)
        {
            var name = sqlite3_column_name(Handle, ordinal);
            if (name == null
                && (ordinal < 0 || ordinal >= FieldCount))
            {
                // NB: Message is provided by the framework
                throw new ArgumentOutOfRangeException(nameof(ordinal), ordinal, message: null);
            }

            return name;
        }

        public virtual int GetOrdinal(string name)
        {
            for (var i = 0; i < FieldCount; i++)
            {
                if (GetName(i) == name)
                {
                    return i;
                }
            }

            // NB: Message is provided by framework
            throw new ArgumentOutOfRangeException(nameof(name), name, message: null);
        }

        public virtual string GetDataTypeName(int ordinal)
        {
            var typeName = sqlite3_column_decltype(Handle, ordinal);
            if (typeName != null)
            {
                var i = typeName.IndexOf('(');

                return i == -1
                    ? typeName
                    : typeName.Substring(0, i);
            }

            var sqliteType = GetSqliteType(ordinal);
            switch (sqliteType)
            {
                case SQLITE_INTEGER:
                    return "INTEGER";

                case SQLITE_FLOAT:
                    return "REAL";

                case SQLITE_TEXT:
                    return "TEXT";

                default:
                    Debug.Assert(
                        sqliteType == SQLITE_BLOB || sqliteType == SQLITE_NULL,
                        "Unexpected column type: " + sqliteType);
                    return "BLOB";
            }
        }

        public virtual Type GetFieldType(int ordinal)
        {
            var sqliteType = GetSqliteType(ordinal);
            switch (sqliteType)
            {
                case SQLITE_INTEGER:
                    return typeof(long);

                case SQLITE_FLOAT:
                    return typeof(double);

                case SQLITE_TEXT:
                    return typeof(string);

                default:
                    Debug.Assert(
                        sqliteType == SQLITE_BLOB || sqliteType == SQLITE_NULL,
                        "Unexpected column type: " + sqliteType);
                    return typeof(byte[]);
            }
        }

        public static Type GetFieldType(string type)
        {
            switch (type)
            {
                case "integer":
                    return typeof(long);

                case "real":
                    return typeof(double);

                case "text":
                    return typeof(string);

                default:
                    Debug.Assert(type == "blob" || type == null, "Unexpected column type: " + type);
                    return typeof(byte[]);
            }
        }

        public virtual long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            var blob = GetCachedBlob(ordinal);

            long bytesToRead = blob.Length - dataOffset;
            if (buffer != null)
            {
                bytesToRead = Math.Min(bytesToRead, length);
                Array.Copy(blob, dataOffset, buffer, bufferOffset, bytesToRead);
            }

            return bytesToRead;
        }

        public virtual long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            var text = GetString(ordinal);

            int charsToRead = text.Length - (int)dataOffset;
            charsToRead = Math.Min(charsToRead, length);
            text.CopyTo((int)dataOffset, buffer, bufferOffset, charsToRead);
            return charsToRead;
        }

        public virtual Stream GetStream(int ordinal)
        {
            if (ordinal < 0
                || ordinal >= FieldCount)
            {
                throw new ArgumentOutOfRangeException(nameof(ordinal), ordinal, message: null);
            }

            var blobDatabaseName = sqlite3_column_database_name(Handle, ordinal);
            var blobTableName = sqlite3_column_table_name(Handle, ordinal);

            var rowidOrdinal = -1;
            for (var i = 0; i < FieldCount; i++)
            {
                if (i == ordinal)
                {
                    continue;
                }

                var databaseName = sqlite3_column_database_name(Handle, i);
                if (databaseName != blobDatabaseName)
                {
                    continue;
                }

                var tableName = sqlite3_column_table_name(Handle, i);
                if (tableName != blobTableName)
                {
                    continue;
                }

                var columnName = sqlite3_column_origin_name(Handle, i);
                if (columnName == "rowid")
                {
                    rowidOrdinal = i;
                    break;
                }

                var rc = sqlite3_table_column_metadata(
                    _connection.Handle,
                    databaseName,
                    tableName,
                    columnName,
                    out var dataType,
                    out var collSeq,
                    out var notNull,
                    out var primaryKey,
                    out var autoInc);
                SqliteException.ThrowExceptionForRC(rc, _connection.Handle);
                if (string.Equals(dataType, "INTEGER", StringComparison.OrdinalIgnoreCase)
                    && primaryKey != 0)
                {
                    rowidOrdinal = i;
                    break;
                }
            }

            if (rowidOrdinal < 0)
            {
                return new MemoryStream(GetCachedBlob(ordinal), false);
            }

            var blobColumnName = sqlite3_column_origin_name(Handle, ordinal);
            var rowid = GetInt32(rowidOrdinal);

            return new SqliteBlob(_connection, blobTableName, blobColumnName, rowid, readOnly: true);
        }

        public bool Read()
        {
            if (!_stepped)
            {
                _stepped = true;

                return HasRows;
            }

            var rc = sqlite3_step(Handle);
            SqliteException.ThrowExceptionForRC(rc, _connection.Handle);

            Array.Clear(_blobCache, 0, _blobCache.Length);

            return rc != SQLITE_DONE;
        }

        public void Dispose()
            => sqlite3_reset(Handle);

        private byte[] GetCachedBlob(int ordinal)
        {
            if (ordinal < 0
                || ordinal >= FieldCount)
            {
                // NB: Message is provided by the framework
                throw new ArgumentOutOfRangeException(nameof(ordinal), ordinal, message: null);
            }

            var blob = _blobCache[ordinal];
            if (blob == null)
            {
                blob = GetBlob(ordinal);
                _blobCache[ordinal] = blob;
            }

            return blob;
        }
    }
}
