// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal
{
    /// <summary>
    ///     <para>
    ///         This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///         the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///         any release. You should only use it directly in your code with extreme caution and knowing that
    ///         doing so can result in application failures when updating to a new Entity Framework Core release.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Singleton"/>. This means a single instance
    ///         is used by many <see cref="DbContext"/> instances. The implementation must be thread-safe.
    ///         This service cannot depend on services registered as <see cref="ServiceLifetime.Scoped"/>.
    ///     </para>
    /// </summary>
    public class SqlServerMigrationsAnnotationProvider : MigrationsAnnotationProvider
    {
        /// <summary>
        ///     Initializes a new instance of this class.
        /// </summary>
        /// <param name="dependencies"> Parameter object containing dependencies for this service. </param>
        public SqlServerMigrationsAnnotationProvider([NotNull] MigrationsAnnotationProviderDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IModel model) => ForRemove(model);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IEntityType entityType) => ForRemove(entityType);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IKey key)
        {
            var isClustered = key.GetSqlServerIsClustered();
            if (isClustered.HasValue)
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.Clustered,
                    isClustered.Value);
            }
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IIndex index)
        {
            var isClustered = index.GetSqlServerIsClustered();
            if (isClustered.HasValue)
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.Clustered,
                    isClustered.Value);
            }

            var includeProperties = index.GetSqlServerIncludeProperties();
            if (includeProperties != null)
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.Include,
                    includeProperties);
            }

            var isOnline = index.GetSqlServerIsCreatedOnline();
            if (isOnline.HasValue)
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.CreatedOnline,
                    isOnline.Value);
            }
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            if (property.GetSqlServerValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn)
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.ValueGenerationStrategy,
                    SqlServerValueGenerationStrategy.IdentityColumn);

                var seed = property.GetSqlServerIdentitySeed();
                if (seed.HasValue)
                {
                    yield return new Annotation(
                        SqlServerAnnotationNames.IdentitySeed,
                        seed.Value);
                }

                var increment = property.GetSqlServerIdentityIncrement();
                if (increment.HasValue)
                {
                    yield return new Annotation(
                        SqlServerAnnotationNames.IdentityIncrement,
                        increment.Value);
                }
            }

        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> ForRemove(IModel model)
        {
            if (model.GetEntityTypes().Any(e => e.BaseType == null && e.GetSqlServerIsMemoryOptimized()))
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.MemoryOptimized,
                    true);
            }
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override IEnumerable<IAnnotation> ForRemove(IEntityType entityType)
        {
            if (IsMemoryOptimized(entityType))
            {
                yield return new Annotation(
                    SqlServerAnnotationNames.MemoryOptimized,
                    true);
            }
        }

        private static bool IsMemoryOptimized(IEntityType entityType)
            => entityType.GetAllBaseTypesInclusive().Any(t => t.GetSqlServerIsMemoryOptimized());
    }
}
