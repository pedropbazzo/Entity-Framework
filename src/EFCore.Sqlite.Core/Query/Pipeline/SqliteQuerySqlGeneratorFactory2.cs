﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Relational.Query.Pipeline;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Sqlite.Query.Pipeline
{
    public class SqliteQuerySqlGeneratorFactory2 : QuerySqlGeneratorFactory2
    {
        private readonly IRelationalCommandBuilderFactory _commandBuilderFactory;
        private readonly ISqlGenerationHelper _sqlGenerationHelper;

        public SqliteQuerySqlGeneratorFactory2(
            IRelationalCommandBuilderFactory commandBuilderFactory,
            ISqlGenerationHelper sqlGenerationHelper)
            : base(commandBuilderFactory, sqlGenerationHelper)
        {
            _commandBuilderFactory = commandBuilderFactory;
            _sqlGenerationHelper = sqlGenerationHelper;
        }

        public override QuerySqlGenerator Create()
        {
            return new SqliteQuerySqlGenerator(_commandBuilderFactory, _sqlGenerationHelper);
        }
    }
}
