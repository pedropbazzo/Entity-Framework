// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static class RelationalPropertyExtensions
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static IForeignKey FindSharedTableLink([NotNull] this IProperty property)
        {
            var pk = property.FindContainingPrimaryKey();
            if (pk == null)
            {
                return null;
            }

            var entityType = property.DeclaringEntityType;

            foreach (var fk in entityType.FindForeignKeys(pk.Properties))
            {
                if (!fk.PrincipalKey.IsPrimaryKey()
                    || fk.PrincipalEntityType == fk.DeclaringEntityType)
                {
                    continue;
                }

                var principalEntityType = fk.PrincipalEntityType;
                var declaringEntityType = fk.DeclaringEntityType;
                if (declaringEntityType.GetTableName() == principalEntityType.GetTableName()
                    && declaringEntityType.GetSchema() == principalEntityType.GetSchema())
                {
                    return fk;
                }
            }

            return null;
        }
    }
}
