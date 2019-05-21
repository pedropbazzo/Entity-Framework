// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Query
{
    /// <summary>
    ///     <para>
    ///         A factory for <see cref="RelationalQueryContext" /> instances.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Scoped"/>. This means that each
    ///         <see cref="DbContext"/> instance will use its own instance of this service.
    ///         The implementation may depend on other services registered with any lifetime.
    ///         The implementation does not need to be thread-safe.
    ///     </para>
    /// </summary>
    public class RelationalQueryContextFactory : QueryContextFactory
    {
        private readonly IRelationalConnection _connection;

        /// <summary>
        ///     Creates a new <see cref="RelationalQueryContextFactory"/> instance using the given dependencies.
        /// </summary>
        /// <param name="dependencies"> The dependencies to use. </param>
        /// <param name="connection"> The connection to use. </param>
        /// <param name="executionStrategyFactory"> A factory for the execution strategy to use. </param>
        public RelationalQueryContextFactory(
            [NotNull] QueryContextDependencies dependencies,
            [NotNull] IRelationalConnection connection,
            [NotNull] IExecutionStrategyFactory executionStrategyFactory)
            : base(dependencies)
        {
            _connection = connection;
            ExecutionStrategyFactory = executionStrategyFactory;
        }

        /// <summary>
        ///     The execution strategy factory.
        /// </summary>
        /// <value>
        ///     The execution strategy factory.
        /// </value>
        protected virtual IExecutionStrategyFactory ExecutionStrategyFactory { get; }

        /// <summary>
        ///     Creates a new <see cref="RelationalQueryContext"/>.
        /// </summary>
        /// <returns>
        ///     A QueryContext.
        /// </returns>
        public override QueryContext Create()
            => new RelationalQueryContext(Dependencies, CreateQueryBuffer, _connection, ExecutionStrategyFactory);
    }
}
