// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// ReSharper disable InconsistentNaming
namespace Microsoft.EntityFrameworkCore
{
    public class SqliteDatabaseCreatorTest
    {
        [ConditionalTheory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task Exists_returns_false_when_database_doesnt_exist(bool async, bool useCanConnect)
        {
            var context = CreateContext("Data Source=doesnt-exist.db");

            if (useCanConnect)
            {
                Assert.False(async ? await context.Database.CanConnectAsync() : context.Database.CanConnect());
            }
            else
            {
                var creator = context.GetService<IRelationalDatabaseCreator>();
                Assert.False(async ? await creator.ExistsAsync() : creator.Exists());
            }
        }

        [ConditionalTheory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task Exists_returns_true_when_database_exists(bool async, bool useCanConnect)
        {
            using (var testStore = SqliteTestStore.GetOrCreateInitialized("Empty"))
            {
                var context = CreateContext(testStore.ConnectionString);

                if (useCanConnect)
                {
                    Assert.True(async ? await context.Database.CanConnectAsync() : context.Database.CanConnect());
                }
                else
                {
                    var creator = context.GetService<IRelationalDatabaseCreator>();
                    Assert.True(async ? await creator.ExistsAsync() : creator.Exists());
                }
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Create_sets_journal_mode_to_wal(bool async)
        {
            using (var testStore = SqliteTestStore.GetOrCreate("Create"))
            using (var context = CreateContext(testStore.ConnectionString))
            {
                var creator = context.GetService<IRelationalDatabaseCreator>();

                if (async)
                {
                    await creator.CreateAsync();
                }
                else
                {
                    creator.Create();
                }

                testStore.OpenConnection();
                var journalMode = testStore.ExecuteScalar<string>("PRAGMA journal_mode;");
                Assert.Equal("wal", journalMode);
            }
        }

        private DbContext CreateContext(string connectionString)
            => new DbContext(
                new DbContextOptionsBuilder()
                    .UseSqlite(connectionString)
                    .UseInternalServiceProvider(
                        SqliteTestStoreFactory.Instance.AddProviderServices(new ServiceCollection())
                            .BuildServiceProvider(validateScopes: true))
                    .Options);
    }
}
