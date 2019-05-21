﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

// ReSharper disable UnusedVariable
// ReSharper disable InconsistentNaming
namespace Microsoft.EntityFrameworkCore
{
    public abstract class ConcurrencyDetectorTestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : NorthwindQueryFixtureBase<NoopModelCustomizer>, new()
    {
        protected ConcurrencyDetectorTestBase(TFixture fixture) => Fixture = fixture;

        protected TFixture Fixture { get; }

        [Fact]
        public virtual Task SaveChanges_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    c.SaveChanges();
                    return Task.FromResult(false);
                });
        }

        [Fact]
        public virtual Task SaveChanges_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.SaveChangesAsync());
        }

        [Fact]
        public virtual Task Find_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    c.Products.Find(1);
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task Find_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.FindAsync(1).AsTask());
        }

        [Fact]
        public virtual Task Count_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.Count();
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task Count_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.CountAsync());
        }

        [Fact]
        public virtual Task First_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.First();
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task First_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.FirstAsync());
        }

        [Fact(Skip = "Issue #14935. Cannot eval 'Last()'")]
        public virtual Task Last_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.Last();
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task Last_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.LastAsync());
        }

        [Fact]
        public virtual Task Single_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.Single(p => p.ProductID == 1);
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task Single_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.SingleAsync(p => p.ProductID == 1));
        }

        [Fact]
        public virtual Task Any_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.Any(p => p.ProductID < 10);
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task Any_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.AnyAsync(p => p.ProductID < 10));
        }

        [Fact]
        public virtual Task ToList_logs_concurrent_access_nonasync()
        {
            return ConcurrencyDetectorTest(
                c =>
                {
                    var result = c.Products.ToList();
                    return Task.FromResult(false);
                });
        }

        [Fact(Skip = "#12138")]
        public virtual Task ToList_logs_concurrent_access_async()
        {
            return ConcurrencyDetectorTest(c => c.Products.ToListAsync());
        }

        protected virtual async Task ConcurrencyDetectorTest(Func<NorthwindContext, Task> test)
        {
            using (var context = CreateContext())
            {
                context.Products.Add(new Product());

                using (context.GetService<IConcurrencyDetector>().EnterCriticalSection())
                {
                    Exception ex = await Assert.ThrowsAsync<InvalidOperationException>(() => test(context));

                    Assert.Equal(CoreStrings.ConcurrentMethodInvocation, ex.Message);
                }
            }
        }

        protected NorthwindContext CreateContext() => Fixture.CreateContext();
    }
}
