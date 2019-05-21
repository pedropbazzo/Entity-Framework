// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class EntityFrameworkServicesBuilderTest
    {
        [Fact]
        public void Can_register_scoped_service_with_concrete_implementation()
        {
            TestScoped(b => b.TryAdd<IConcurrencyDetector, FakeConcurrencyDetector>());
        }

        [Fact]
        public void Can_register_scoped_service_with_concrete_implementation_non_generic()
        {
            TestScoped(b => b.TryAdd(typeof(IConcurrencyDetector), typeof(FakeConcurrencyDetector)));
        }

        [Fact]
        public void Can_register_scoped_service_with_full_factory()
        {
            TestScoped(b => b.TryAdd<IConcurrencyDetector, FakeConcurrencyDetector>(p => new FakeConcurrencyDetector()));
        }

        [Fact]
        public void Can_register_scoped_service_with_half_factory()
        {
            TestScoped(b => b.TryAdd<IConcurrencyDetector>(p => new FakeConcurrencyDetector()));
        }

        [Fact]
        public void Can_register_scoped_service_with_full_factory_non_generic()
        {
            TestScoped(b => b.TryAdd(typeof(IConcurrencyDetector), typeof(FakeConcurrencyDetector), p => new FakeConcurrencyDetector()));
        }

        [Fact]
        public void Can_register_scoped_service_with_half_factory_non_generic()
        {
            TestScoped(b => b.TryAdd(typeof(IConcurrencyDetector), typeof(IConcurrencyDetector), p => new FakeConcurrencyDetector()));
        }

        [Fact]
        public void Can_register_scoped_service_with_object_factory()
        {
            TestScoped(b => b.TryAdd(typeof(IConcurrencyDetector), typeof(object), p => new FakeConcurrencyDetector()));
        }

        [Fact]
        public void Cannot_register_scoped_with_instance()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.SingletonRequired("Scoped", nameof(IConcurrencyDetector)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd<IConcurrencyDetector>(new FakeConcurrencyDetector()))
                    .Message);
        }

        [Fact]
        public void Cannot_register_scoped_with_instance_non_generic()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.SingletonRequired("Scoped", nameof(IConcurrencyDetector)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd(typeof(IConcurrencyDetector), new FakeConcurrencyDetector()))
                    .Message);
        }

        private static void TestScoped(Action<EntityFrameworkServicesBuilder> tryAdd)
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EntityFrameworkServicesBuilder(serviceCollection);

            tryAdd(builder);

            serviceCollection.AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            FakeConcurrencyDetector service;

            using (var context = CreateContext(serviceProvider))
            {
                service = (FakeConcurrencyDetector)context.GetService<IConcurrencyDetector>();
                Assert.Same(service, context.GetService<IConcurrencyDetector>());
            }

            using (var context = CreateContext(serviceProvider))
            {
                Assert.NotSame(service, context.GetService<IConcurrencyDetector>());
            }
        }

        [Fact]
        public void Can_register_singleton_service_with_concrete_implementation()
        {
            TestSingleton(b => b.TryAdd<IDbSetInitializer, FakeDbSetInitializer>());
        }

        [Fact]
        public void Can_register_singleton_service_with_concrete_implementation_non_generic()
        {
            TestSingleton(b => b.TryAdd(typeof(IDbSetInitializer), typeof(FakeDbSetInitializer)));
        }

        [Fact]
        public void Can_register_singleton_service_with_full_factory()
        {
            TestSingleton(b => b.TryAdd<IDbSetInitializer, FakeDbSetInitializer>(p => new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_service_with_half_factory()
        {
            TestSingleton(b => b.TryAdd<IDbSetInitializer>(p => new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_service_with_full_factory_non_generic()
        {
            TestSingleton(b => b.TryAdd(typeof(IDbSetInitializer), typeof(FakeDbSetInitializer), p => new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_service_with_half_factory_non_generic()
        {
            TestSingleton(b => b.TryAdd(typeof(IDbSetInitializer), typeof(IDbSetInitializer), p => new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_service_with_object_factory()
        {
            TestSingleton(b => b.TryAdd(typeof(IDbSetInitializer), typeof(object), p => new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_with_instance()
        {
            TestSingleton(b => b.TryAdd<IDbSetInitializer>(new FakeDbSetInitializer()));
        }

        [Fact]
        public void Can_register_singleton_with_instance_non_generic()
        {
            TestSingleton(b => b.TryAdd(typeof(IDbSetInitializer), new FakeDbSetInitializer()));
        }

        private static void TestSingleton(Action<EntityFrameworkServicesBuilder> tryAdd)
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EntityFrameworkServicesBuilder(serviceCollection);

            tryAdd(builder);

            serviceCollection.AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            FakeDbSetInitializer service;

            using (var context = CreateContext(serviceProvider))
            {
                service = (FakeDbSetInitializer)context.GetService<IDbSetInitializer>();
                Assert.Same(service, context.GetService<IDbSetInitializer>());
            }

            using (var context = CreateContext(serviceProvider))
            {
                Assert.Same(service, context.GetService<IDbSetInitializer>());
            }
        }

        [Fact]
        public void Can_register_multiple_scoped_service_with_concrete_implementation()
        {
            TestMultipleScoped(b => b.TryAdd<IResettableService, FakeResetableService>());
        }

        [Fact]
        public void Can_register_multiple_scoped_service_with_concrete_implementation_non_generic()
        {
            TestMultipleScoped(b => b.TryAdd(typeof(IResettableService), typeof(FakeResetableService)));
        }

        [Fact]
        public void Can_register_multiple_scoped_service_with_full_factory()
        {
            TestMultipleScoped(b => b.TryAdd<IResettableService, FakeResetableService>(p => new FakeResetableService()));
        }

        [Fact]
        public void Cannot_register_multiple_scoped_service_with_half_factory()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.ImplementationTypeRequired(nameof(IResettableService)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd<IResettableService>(p => new FakeResetableService()))
                    .Message);
        }

        [Fact]
        public void Can_register_multiple_scoped_service_with_full_factory_non_generic()
        {
            TestMultipleScoped(
                b => b.TryAdd(typeof(IResettableService), typeof(FakeResetableService), p => new FakeResetableService()));
        }

        [Fact]
        public void Cannot_register_multiple_scoped_service_with_half_factory_non_generic()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.ImplementationTypeRequired(nameof(IResettableService)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd(
                            typeof(IResettableService), typeof(IResettableService), p => new FakeResetableService()))
                    .Message);
        }

        [Fact]
        public void Cannot_register_multiple_scoped_service_with_object_factory()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.ImplementationTypeRequired(nameof(IResettableService)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd(typeof(IResettableService), typeof(object), p => new FakeResetableService()))
                    .Message);
        }

        [Fact]
        public void Cannot_register_multiple_scoped_with_instance()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.SingletonRequired("Scoped", nameof(IResettableService)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd<IResettableService>(new FakeResetableService()))
                    .Message);
        }

        [Fact]
        public void Cannot_register_multiple_scoped_with_instance_non_generic()
        {
            var builder = new EntityFrameworkServicesBuilder(new ServiceCollection());

            Assert.Equal(
                CoreStrings.SingletonRequired("Scoped", nameof(IResettableService)),
                Assert.Throws<InvalidOperationException>(
                        () => builder.TryAdd(typeof(IResettableService), new FakeResetableService()))
                    .Message);
        }

        private static void TestMultipleScoped(Action<EntityFrameworkServicesBuilder> tryAdd)
        {
            var serviceCollection = new ServiceCollection();
            var builder = new EntityFrameworkServicesBuilder(serviceCollection);

            tryAdd(builder);

            serviceCollection.AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var services = new List<IResettableService>();

            using (var context = CreateContext(serviceProvider))
            {
                services = context.GetService<IEnumerable<IResettableService>>().ToList();

                Assert.Equal(3, services.Count);
                Assert.Contains(typeof(FakeResetableService), services.Select(s => s.GetType()));
                Assert.Contains(typeof(StateManager), services.Select(s => s.GetType()));
                Assert.Contains(typeof(InMemoryTransactionManager), services.Select(s => s.GetType()));

                foreach (var service in context.GetService<IEnumerable<IResettableService>>())
                {
                    Assert.Contains(service, services);
                }
            }

            using (var context = CreateContext(serviceProvider))
            {
                var newServices = context.GetService<IEnumerable<IResettableService>>().ToList();

                Assert.Equal(3, newServices.Count);

                foreach (var service in newServices)
                {
                    Assert.DoesNotContain(service, services);
                }
            }
        }

        private class FakeConcurrencyDetector : IConcurrencyDetector
        {
            public IDisposable EnterCriticalSection()
            {
                throw new NotImplementedException();
            }

            public Task<IDisposable> EnterCriticalSectionAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeDbSetInitializer : IDbSetInitializer
        {
            public void InitializeSets(DbContext context)
            {
            }

            public DbSet<TEntity> CreateSet<TEntity>(DbContext context)
                where TEntity : class => throw new NotImplementedException();

            public object CreateSet(DbContext context, Type type) => throw new NotImplementedException();
        }

        private class FakeResetableService : IResettableService
        {
            public void ResetState()
            {
            }
        }

        private static DbContext CreateContext(IServiceProvider serviceProvider)
            => new DbContext(
                new DbContextOptionsBuilder()
                    .UseInternalServiceProvider(serviceProvider)
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
    }
}