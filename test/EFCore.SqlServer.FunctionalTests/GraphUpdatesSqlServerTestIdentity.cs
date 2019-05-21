// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore
{
    // issue #15318
    internal class GraphUpdatesSqlServerTestIdentity : GraphUpdatesSqlServerTestBase<GraphUpdatesSqlServerTestIdentity.GraphUpdatesWithIdentitySqlServerFixture>
    {
        public GraphUpdatesSqlServerTestIdentity(GraphUpdatesWithIdentitySqlServerFixture fixture)
            : base(fixture)
        {
        }

        public class GraphUpdatesWithIdentitySqlServerFixture : GraphUpdatesSqlServerFixtureBase
        {
            protected override string StoreName { get; } = "GraphIdentityUpdatesTest";

            protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
            {
                modelBuilder.ForSqlServerUseIdentityColumns();

                base.OnModelCreating(modelBuilder, context);
            }
        }
    }
}
