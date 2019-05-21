﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Relational.Query.Pipeline;
using Microsoft.EntityFrameworkCore.Relational.Query.Pipeline.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.Pipeline
{
    public class SqliteDateTimeAddTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _addMilliseconds
            = typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMilliseconds), new[] { typeof(double) });

        private static readonly MethodInfo _addTicks
            = typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddTicks), new[] { typeof(long) });

        private readonly Dictionary<MethodInfo, string> _methodInfoToUnitSuffix = new Dictionary<MethodInfo, string>
        {
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddYears), new[] { typeof(int) }), " years" },
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMonths), new[] { typeof(int) }), " months" },
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddDays), new[] { typeof(double) }), " days" },
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddHours), new[] { typeof(double) }), " hours" },
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddMinutes), new[] { typeof(double) }), " minutes" },
            { typeof(DateTime).GetRuntimeMethod(nameof(DateTime.AddSeconds), new[] { typeof(double) }), " seconds" }
        };
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public SqliteDateTimeAddTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public SqlExpression Translate(SqlExpression instance, MethodInfo method, IList<SqlExpression> arguments)
        {
            SqlExpression modifier = null;
            if (_addMilliseconds.Equals(method))
            {
                modifier = _sqlExpressionFactory.Add(
                    _sqlExpressionFactory.Convert(
                        _sqlExpressionFactory.Divide(
                            arguments[0],
                            _sqlExpressionFactory.Constant(1000.0)),
                        typeof(string)),
                    _sqlExpressionFactory.Constant(" seconds"));
            }
            else if (_addTicks.Equals(method))
            {
                modifier = _sqlExpressionFactory.Add(
                    _sqlExpressionFactory.Convert(
                        _sqlExpressionFactory.Divide(
                            arguments[0],
                            _sqlExpressionFactory.Constant((double)TimeSpan.TicksPerDay)),
                        typeof(string)),
                    _sqlExpressionFactory.Constant(" seconds"));
            }
            else if (_methodInfoToUnitSuffix.TryGetValue(method, out var unitSuffix))
            {
                modifier = _sqlExpressionFactory.Add(
                    _sqlExpressionFactory.Convert(arguments[0], typeof(string)),
                    _sqlExpressionFactory.Constant(unitSuffix));
            }

            if (modifier != null)
            {
                return _sqlExpressionFactory.Function(
                    "rtrim",
                    new SqlExpression[]
                    {
                        _sqlExpressionFactory.Function(
                            "rtrim",
                            new SqlExpression[]
                            {
                                SqliteExpression.Strftime(
                                    _sqlExpressionFactory,
                                    method.ReturnType,
                                    "%Y-%m-%d %H:%M:%f",
                                    instance,
                                    new [] { modifier }),
                                _sqlExpressionFactory.Constant("0")
                            },
                            method.ReturnType),
                        _sqlExpressionFactory.Constant(".")
                    },
                    method.ReturnType);
            }

            return null;
        }
    }
}
