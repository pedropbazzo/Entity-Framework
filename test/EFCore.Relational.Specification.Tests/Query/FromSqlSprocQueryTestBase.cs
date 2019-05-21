﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable AccessToDisposedClosure
namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class FromSqlSprocQueryTestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : NorthwindQueryRelationalFixture<NoopModelCustomizer>, new()
    {
        protected FromSqlSprocQueryTestBase(TFixture fixture) => Fixture = fixture;

        protected TFixture Fixture { get; }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                    .ToArray();

                Assert.Equal(10, actual.Length);

                Assert.True(
                    actual.Any(
                        mep =>
                            mep.TenMostExpensiveProducts == "Côte de Blaye"
                            && mep.UnitPrice == 263.50m));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_projection()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                    .Select(mep => mep.TenMostExpensiveProducts)
                    .ToArray();

                Assert.Equal(10, actual.Length);
                Assert.True(actual.Any(r => r == "Côte de Blaye"));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_reprojection()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                    .Select(
                        mep =>
                            new MostExpensiveProduct
                            {
                                TenMostExpensiveProducts = "Foo",
                                UnitPrice = mep.UnitPrice
                            })
                    .ToArray();

                Assert.Equal(10, actual.Length);
                Assert.True(actual.All(mep => mep.TenMostExpensiveProducts == "Foo"));
            }
        }

        [Fact]
        public virtual void From_sql_queryable_stored_procedure_with_parameter()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<CustomerOrderHistory>()
                    .FromSqlRaw(CustomerOrderHistorySproc, GetCustomerOrderHistorySprocParameters())
                    .ToArray();

                Assert.Equal(11, actual.Length);

                Assert.True(
                    actual.Any(
                        coh =>
                            coh.ProductName == "Aniseed Syrup"
                            && coh.Total == 6));
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'where [mep].TenMostExpensiveProducts.Contains(\"C\")'")]
        public virtual void From_sql_queryable_stored_procedure_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                    .Where(mep => mep.TenMostExpensiveProducts.Contains("C"))
                    .OrderBy(mep => mep.UnitPrice)
                    .ToArray();

                Assert.Equal(4, actual.Length);
                Assert.Equal(46.00m, actual.First().UnitPrice);
                Assert.Equal(263.50m, actual.Last().UnitPrice);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'where [coh].ProductName.Contains(\"C\")'")]
        public virtual void From_sql_queryable_stored_procedure_with_parameter_composed()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<CustomerOrderHistory>()
                    .FromSqlRaw(CustomerOrderHistorySproc, GetCustomerOrderHistorySprocParameters())
                    .Where(coh => coh.ProductName.Contains("C"))
                    .OrderBy(coh => coh.Total)
                    .ToArray();

                Assert.Equal(2, actual.Length);
                Assert.Equal(15, actual.First().Total);
                Assert.Equal(21, actual.Last().Total);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'orderby [mep].UnitPrice desc'")]
        public virtual void From_sql_queryable_stored_procedure_take()
        {
            using (var context = CreateContext())
            {
                var actual = context
                    .Set<MostExpensiveProduct>()
                    .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                    .OrderByDescending(mep => mep.UnitPrice)
                    .Take(2)
                    .ToArray();

                Assert.Equal(2, actual.Length);
                Assert.Equal(263.50m, actual.First().UnitPrice);
                Assert.Equal(123.79m, actual.Last().UnitPrice);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'Min()'")]
        public virtual void From_sql_queryable_stored_procedure_min()
        {
            using (var context = CreateContext())
            {
                Assert.Equal(
                    45.60m,
                    context.Set<MostExpensiveProduct>()
                        .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                        .Min(mep => mep.UnitPrice));
            }
        }

        [Fact(Skip = "issue #15312")]
        public virtual void From_sql_queryable_stored_procedure_with_include_throws()
        {
            using (var context = CreateContext())
            {
                Assert.Equal(
                    RelationalStrings.StoredProcedureIncludeNotSupported,
                    Assert.Throws<InvalidOperationException>(
                        () => context.Set<Product>()
                            .FromSqlRaw("SelectStoredProcedure", GetTenMostExpensiveProductsParameters())
                            .Include(p => p.OrderDetails)
                            .ToArray()
                    ).Message);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'from MostExpensiveProduct b in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.MostExpensiveProduct])'")]
        public virtual void From_sql_queryable_with_multiple_stored_procedures()
        {
            using (var context = CreateContext())
            {
                var actual
                    = (from a in context.Set<MostExpensiveProduct>()
                           .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                       from b in context.Set<MostExpensiveProduct>()
                           .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                       where a.TenMostExpensiveProducts == b.TenMostExpensiveProducts
                       select new
                       {
                           a,
                           b
                       })
                    .ToArray();

                Assert.Equal(10, actual.Length);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'from Product p in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Product])'")]
        public virtual void From_sql_queryable_stored_procedure_and_select()
        {
            using (var context = CreateContext())
            {
                var actual
                    = (from mep in context.Set<MostExpensiveProduct>()
                           .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                       from p in context.Set<Product>()
                           .FromSqlRaw(NormalizeDelimetersInRawString("SELECT * FROM [Products]"))
                       where mep.TenMostExpensiveProducts == p.ProductName
                       select new
                       {
                           mep,
                           p
                       })
                    .ToArray();

                Assert.Equal(10, actual.Length);
            }
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'from MostExpensiveProduct mep in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.MostExpensiveProduct])'")]
        public virtual void From_sql_queryable_select_and_stored_procedure()
        {
            using (var context = CreateContext())
            {
                var actual
                    = (from p in context.Set<Product>().FromSqlRaw(NormalizeDelimetersInRawString("SELECT * FROM [Products]"))
                       from mep in context.Set<MostExpensiveProduct>()
                           .FromSqlRaw(TenMostExpensiveProductsSproc, GetTenMostExpensiveProductsParameters())
                       where mep.TenMostExpensiveProducts == p.ProductName
                       select new
                       {
                           mep,
                           p
                       })
                    .ToArray();

                Assert.Equal(10, actual.Length);
            }
        }

        private string NormalizeDelimetersInRawString(string sql)
            => Fixture.TestStore.NormalizeDelimetersInRawString(sql);

        private FormattableString NormalizeDelimetersInInterpolatedString(FormattableString sql)
            => Fixture.TestStore.NormalizeDelimetersInInterpolatedString(sql);

        protected NorthwindContext CreateContext()
        {
            var context = Fixture.CreateContext();

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return context;
        }

        protected virtual object[] GetTenMostExpensiveProductsParameters()
            => Array.Empty<object>();

        protected virtual object[] GetCustomerOrderHistorySprocParameters()
            => new[] { "ALFKI" };

        protected abstract string TenMostExpensiveProductsSproc { get; }
        protected abstract string CustomerOrderHistorySproc { get; }
    }
}
