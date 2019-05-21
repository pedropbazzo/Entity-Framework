// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using Xunit;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    public abstract class TestStore : IDisposable
    {
        private readonly bool _shared;
        private static readonly TestStoreIndex _globalTestStoreIndex = new TestStoreIndex();
        public IServiceProvider ServiceProvider { get; protected set; }

        protected TestStore(string name, bool shared)
        {
            Name = name;
            _shared = shared;
        }

        public string Name { get; protected set; }

        public virtual TestStore Initialize(
            IServiceProvider serviceProvider, Func<DbContext> createContext, Action<DbContext> seed = null, Action<DbContext> clean = null)
        {
            ServiceProvider = serviceProvider;
            if (createContext == null)
            {
                createContext = CreateDefaultContext;
            }

            if (_shared)
            {
                GetTestStoreIndex(serviceProvider).CreateShared(GetType().Name + Name, () => Initialize(createContext, seed, clean));
            }
            else
            {
                Initialize(createContext, seed, clean);
            }

            return this;
        }

        public TestStore Initialize(
            IServiceProvider serviceProvider, Func<TestStore, DbContext> createContext, Action<DbContext> seed = null, Action<DbContext> clean = null)
            => Initialize(serviceProvider, () => createContext(this), seed, clean);

        protected virtual void Initialize(Func<DbContext> createContext, Action<DbContext> seed, Action<DbContext> clean)
        {
            using (var context = createContext())
            {
                clean?.Invoke(context);

                Clean(context);

                seed?.Invoke(context);
            }
        }

        public abstract DbContextOptionsBuilder AddProviderOptions(DbContextOptionsBuilder builder);
        public abstract void Clean(DbContext context);

        protected virtual DbContext CreateDefaultContext()
            => new DbContext(AddProviderOptions(new DbContextOptionsBuilder().EnableServiceProviderCaching(false)).Options);

        protected virtual TestStoreIndex GetTestStoreIndex(IServiceProvider serviceProvider) => _globalTestStoreIndex;

        public virtual void Dispose()
        {
        }

        public static IDisposable CreateTransactionScope(bool useTransaction = true)
        {
            if (useTransaction)
            {
                var listener = new DistributedTransactionListener();
                var transactionDisposed = new ManualResetEventSlim();
                var transaction = new CommittableTransaction(TimeSpan.FromMinutes(10));
                transaction.TransactionCompleted += (_, __) => transactionDisposed.Set();

                return new CompositeDisposable(
                    listener,
                    new BlockingDisposable(transactionDisposed),
                    transaction,
                    new TransactionScope(transaction, TimeSpan.FromMinutes(10), TransactionScopeAsyncFlowOption.Enabled));
            }

            return new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
        }

        private class DistributedTransactionListener : IDisposable
        {
            public DistributedTransactionListener()
            {
                TransactionManager.DistributedTransactionStarted += DistributedTransactionStarted;
            }

            private void DistributedTransactionStarted(object sender, TransactionEventArgs e)
            {
                Assert.False(true, "Distributed transaction started");
            }

            public void Dispose()
            {
                TransactionManager.DistributedTransactionStarted -= DistributedTransactionStarted;
            }
        }

        private class CompositeDisposable : IDisposable
        {
            private readonly IDisposable[] _disposables;

            public CompositeDisposable(params IDisposable[] disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                var exceptions = new List<Exception>();
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = _disposables.Length - 1; i >= 0; i--)
                {
                    try
                    {
                        _disposables[i].Dispose();
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }

        private class BlockingDisposable : IDisposable
        {
            private readonly ManualResetEventSlim _signal;

            public BlockingDisposable(ManualResetEventSlim signal)
            {
                _signal = signal;
            }

            public void Dispose()
            {
                _signal.Wait(TimeSpan.FromMinutes(10));
                _signal.Dispose();
            }
        }
    }
}
