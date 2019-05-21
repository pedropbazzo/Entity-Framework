// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public class CheckConstraint : IConventionCheckConstraint
    {
        private ConfigurationSource _configurationSource;

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public CheckConstraint(
            [NotNull] IMutableEntityType entityType,
            [NotNull] string name,
            [NotNull] string sql,
            ConfigurationSource configurationSource)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotEmpty(name, nameof(name));
            Check.NotEmpty(sql, nameof(sql));

            EntityType = entityType;
            Name = name;
            Sql = sql;
            _configurationSource = configurationSource;

            var dataDictionary = GetAnnotationsDictionary(EntityType);
            if (dataDictionary == null)
            {
                dataDictionary = new Dictionary<string, CheckConstraint>();
                ((IMutableEntityType)EntityType).SetOrRemoveAnnotation(RelationalAnnotationNames.CheckConstraints, dataDictionary);
            }

            if (dataDictionary.ContainsKey(Name))
            {
                throw new InvalidOperationException(RelationalStrings.DuplicateCheckConstraint(Name, EntityType.DisplayName()));
            }
            else
            {
                dataDictionary.Add(name, this);
            }
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static IEnumerable<CheckConstraint> GetCheckConstraints([NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            return GetAnnotationsDictionary(entityType)?.Values ?? Enumerable.Empty<CheckConstraint>();
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static ICheckConstraint FindCheckConstraint(
            [NotNull] IEntityType entityType, [NotNull] string name)
        {
            var dataDictionary = GetAnnotationsDictionary(entityType);

            if (dataDictionary == null)
            {
                return null;
            }

            return dataDictionary.TryGetValue(name, out var checkConstraint) ? checkConstraint : null;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static bool RemoveCheckConstraint(
            [NotNull] IMutableEntityType entityType, [NotNull] string name)
        {
            var dataDictionary = GetAnnotationsDictionary(entityType);

            return dataDictionary?.Remove(name) ?? false;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual IEntityType EntityType { get; }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual string Sql { get; }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ConfigurationSource GetConfigurationSource() => _configurationSource;

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
        {
            _configurationSource = configurationSource.Max(_configurationSource);
        }

        private static Dictionary<string, CheckConstraint> GetAnnotationsDictionary(IEntityType entityType)
        {
            return (Dictionary<string, CheckConstraint>)entityType[RelationalAnnotationNames.CheckConstraints];
        }

        /// <inheritdoc />
        IConventionEntityType IConventionCheckConstraint.EntityType => (IConventionEntityType)EntityType;
    }
}
