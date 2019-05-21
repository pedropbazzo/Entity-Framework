// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;

namespace Microsoft.EntityFrameworkCore.Query
{
    internal class QueryNavigationsSqliteTest : QueryNavigationsTestBase<NorthwindQuerySqliteFixture<NoopModelCustomizer>>
    {
        public QueryNavigationsSqliteTest(NorthwindQuerySqliteFixture<NoopModelCustomizer> fixture)
            : base(fixture)
        {
        }

        // Skip for SQLite. Issue #14935. Cannot eval 'where ([od].UnitPrice > 10)'
        public override Task Select_multiple_complex_projections(bool isAsync) => null;
    }
}
