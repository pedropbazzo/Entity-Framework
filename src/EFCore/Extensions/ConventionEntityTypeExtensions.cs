// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Extension methods for <see cref="IConventionEntityType" />.
    /// </summary>
    public static class ConventionEntityTypeExtensions
    {
        /// <summary>
        ///     Returns all derived types of the given <see cref="IConventionEntityType" />, including the type itself.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Derived types. </returns>
        public static IEnumerable<IConventionEntityType> GetDerivedTypesInclusive([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDerivedTypesInclusive().Cast<IConventionEntityType>();

        /// <summary>
        ///     <para>
        ///         Gets all foreign keys declared on the given <see cref="IConventionEntityType" />.
        ///     </para>
        ///     <para>
        ///         This method does not return foreign keys declared on derived types.
        ///         It is useful when iterating over all entity types to avoid processing the same foreign key more than once.
        ///         Use <see cref="IConventionEntityType.GetForeignKeys" /> to also return foreign keys declared on derived types.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Declared foreign keys. </returns>
        public static IEnumerable<IConventionForeignKey> GetDeclaredForeignKeys([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDeclaredForeignKeys().Cast<IConventionForeignKey>();

        /// <summary>
        ///     <para>
        ///         Gets all non-navigation properties declared on the given <see cref="IConventionEntityType" />.
        ///     </para>
        ///     <para>
        ///         This method does not return properties declared on derived types.
        ///         It is useful when iterating over all entity types to avoid processing the same property more than once.
        ///         Use <see cref="IConventionEntityType.GetProperties" /> to also return properties declared on derived types.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Declared non-navigation properties. </returns>
        public static IEnumerable<IConventionProperty> GetDeclaredProperties([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDeclaredProperties().Cast<IConventionProperty>();

        /// <summary>
        ///     <para>
        ///         Gets all navigation properties declared on the given <see cref="IConventionEntityType" />.
        ///     </para>
        ///     <para>
        ///         This method does not return navigation properties declared on derived types.
        ///         It is useful when iterating over all entity types to avoid processing the same navigation property more than once.
        ///         Use <see cref="GetNavigations" /> to also return navigation properties declared on derived types.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Declared navigation properties. </returns>
        public static IEnumerable<IConventionNavigation> GetDeclaredNavigations([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDeclaredNavigations().Cast<IConventionNavigation>();

        /// <summary>
        ///     <para>
        ///         Gets all service properties declared on the given <see cref="IConventionEntityType" />.
        ///     </para>
        ///     <para>
        ///         This method does not return properties declared on derived types.
        ///         It is useful when iterating over all entity types to avoid processing the same property more than once.
        ///         Use <see cref="IConventionEntityType.GetServiceProperties" /> to also return properties declared on derived types.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Declared service properties. </returns>
        public static IEnumerable<IConventionServiceProperty> GetDeclaredServiceProperties([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDeclaredServiceProperties().Cast<IConventionServiceProperty>();

        /// <summary>
        ///     <para>
        ///         Gets all indexes declared on the given <see cref="IConventionEntityType" />.
        ///     </para>
        ///     <para>
        ///         This method does not return indexes declared on derived types.
        ///         It is useful when iterating over all entity types to avoid processing the same index more than once.
        ///         Use <see cref="IConventionEntityType.GetForeignKeys" /> to also return indexes declared on derived types.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> Declared indexes. </returns>
        public static IEnumerable<IConventionIndex> GetDeclaredIndexes([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDeclaredIndexes().Cast<IConventionIndex>();

        /// <summary>
        ///     Gets all types in the model that derive from a given entity type.
        /// </summary>
        /// <param name="entityType"> The base type to find types that derive from. </param>
        /// <returns> The derived types. </returns>
        public static IEnumerable<IConventionEntityType> GetDerivedTypes([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetDerivedTypes().Cast<IConventionEntityType>();

        /// <summary>
        ///     Gets the root base type for a given entity type.
        /// </summary>
        /// <param name="entityType"> The type to find the root of. </param>
        /// <returns>
        ///     The root base type. If the given entity type is not a derived type, then the same entity type is returned.
        /// </returns>
        public static IConventionEntityType RootType([NotNull] this IConventionEntityType entityType)
            => (IConventionEntityType)((IEntityType)entityType).RootType();

        /// <summary>
        ///     Sets the primary key for this entity.
        /// </summary>
        /// <param name="entityType"> The entity type to set the key on. </param>
        /// <param name="property"> The primary key property. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The newly created key. </returns>
        public static IConventionKey SetPrimaryKey(
            [NotNull] this IConventionEntityType entityType,
            [CanBeNull] IConventionProperty property,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.SetPrimaryKey(property == null ? null : new[] { property }, fromDataAnnotation);
        }

        /// <summary>
        ///     Gets the primary or alternate key that is defined on the given property. Returns <c>null</c> if no key is defined
        ///     for the given property.
        /// </summary>
        /// <param name="entityType"> The entity type to find the key on. </param>
        /// <param name="property"> The property that the key is defined on. </param>
        /// <returns> The key, or null if none is defined. </returns>
        public static IConventionKey FindKey([NotNull] this IConventionEntityType entityType, [NotNull] IProperty property)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.FindKey(new[] { property });
        }

        /// <summary>
        ///     Adds a new alternate key to this entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to add the alternate key to. </param>
        /// <param name="property"> The property to use as an alternate key. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The newly created key. </returns>
        public static IConventionKey AddKey(
            [NotNull] this IConventionEntityType entityType,
            [NotNull] IConventionProperty property,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.AddKey(new[] { property }, fromDataAnnotation);
        }

        /// <summary>
        ///     Gets the foreign keys defined on the given property. Only foreign keys that are defined on exactly the specified
        ///     property are returned. Composite foreign keys that include the specified property are not returned.
        /// </summary>
        /// <param name="entityType"> The entity type to find the foreign keys on. </param>
        /// <param name="property"> The property to find the foreign keys on. </param>
        /// <returns> The foreign keys. </returns>
        public static IEnumerable<IConventionForeignKey> FindForeignKeys(
            [NotNull] this IConventionEntityType entityType, [NotNull] IProperty property)
            => entityType.FindForeignKeys(new[] { property });

        /// <summary>
        ///     Gets the foreign keys defined on the given properties. Only foreign keys that are defined on exactly the specified
        ///     set of properties are returned.
        /// </summary>
        /// <param name="entityType"> The entity type to find the foreign keys on. </param>
        /// <param name="properties"> The properties to find the foreign keys on. </param>
        /// <returns> The foreign keys. </returns>
        public static IEnumerable<IConventionForeignKey> FindForeignKeys(
            [NotNull] this IConventionEntityType entityType, [NotNull] IReadOnlyList<IProperty> properties)
            => ((IEntityType)entityType).FindForeignKeys(properties).Cast<IConventionForeignKey>();

        /// <summary>
        ///     Gets the foreign key for the given properties that points to a given primary or alternate key. Returns <c>null</c>
        ///     if no foreign key is found.
        /// </summary>
        /// <param name="entityType"> The entity type to find the foreign keys on. </param>
        /// <param name="property"> The property that the foreign key is defined on. </param>
        /// <param name="principalKey"> The primary or alternate key that is referenced. </param>
        /// <param name="principalEntityType">
        ///     The entity type that the relationship targets. This may be different from the type that <paramref name="principalKey" />
        ///     is defined on when the relationship targets a derived type in an inheritance hierarchy (since the key is defined on the
        ///     base type of the hierarchy).
        /// </param>
        /// <returns> The foreign key, or <c>null</c> if none is defined. </returns>
        public static IConventionForeignKey FindForeignKey(
            [NotNull] this IConventionEntityType entityType,
            [NotNull] IProperty property,
            [NotNull] IKey principalKey,
            [NotNull] IEntityType principalEntityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.FindForeignKey(new[] { property }, principalKey, principalEntityType);
        }

        /// <summary>
        ///     Returns the relationship to the owner if this is an owned type or <c>null</c> otherwise.
        /// </summary>
        /// <param name="entityType"> The entity type to find the foreign keys on. </param>
        /// <returns> The relationship to the owner if this is an owned type or <c>null</c> otherwise. </returns>
        public static IConventionForeignKey FindOwnership([NotNull] this IConventionEntityType entityType)
            => ((EntityType)entityType).FindOwnership();

        /// <summary>
        ///     Gets all foreign keys that target a given entity type (i.e. foreign keys where the given entity type
        ///     is the principal).
        /// </summary>
        /// <param name="entityType"> The entity type to find the foreign keys for. </param>
        /// <returns> The foreign keys that reference the given entity type. </returns>
        public static IEnumerable<IConventionForeignKey> GetReferencingForeignKeys([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetReferencingForeignKeys().Cast<IConventionForeignKey>();

        /// <summary>
        ///     Adds a new relationship to this entity.
        /// </summary>
        /// <param name="entityType"> The entity type to add the foreign key to. </param>
        /// <param name="property"> The property that the foreign key is defined on. </param>
        /// <param name="principalKey"> The primary or alternate key that is referenced. </param>
        /// <param name="principalEntityType">
        ///     The entity type that the relationship targets. This may be different from the type that <paramref name="principalKey" />
        ///     is defined on when the relationship targets a derived type in an inheritance hierarchy (since the key is defined on the
        ///     base type of the hierarchy).
        /// </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The newly created foreign key. </returns>
        public static IConventionForeignKey AddForeignKey(
            [NotNull] this IConventionEntityType entityType,
            [NotNull] IConventionProperty property,
            [NotNull] IConventionKey principalKey,
            [NotNull] IConventionEntityType principalEntityType,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.AddForeignKey(new[] { property }, principalKey, principalEntityType, fromDataAnnotation);
        }

        /// <summary>
        ///     Gets a navigation property on the given entity type. Returns null if no navigation property is found.
        /// </summary>
        /// <param name="entityType"> The entity type to find the navigation property on. </param>
        /// <param name="propertyInfo"> The navigation property on the entity class. </param>
        /// <returns> The navigation property, or null if none is found. </returns>
        public static IConventionNavigation FindNavigation(
            [NotNull] this IConventionEntityType entityType, [NotNull] PropertyInfo propertyInfo)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(propertyInfo, nameof(propertyInfo));

            return entityType.FindNavigation(propertyInfo.GetSimpleMemberName());
        }

        /// <summary>
        ///     Gets a navigation property on the given entity type. Returns null if no navigation property is found.
        /// </summary>
        /// <param name="entityType"> The entity type to find the navigation property on. </param>
        /// <param name="name"> The name of the navigation property on the entity class. </param>
        /// <returns> The navigation property, or null if none is found. </returns>
        public static IConventionNavigation FindNavigation([NotNull] this IConventionEntityType entityType, [NotNull] string name)
            => (IConventionNavigation)((IEntityType)entityType).FindNavigation(name);

        /// <summary>
        ///     Gets all navigation properties on the given entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to get navigation properties for. </param>
        /// <returns> All navigation properties on the given entity type. </returns>
        public static IEnumerable<IConventionNavigation> GetNavigations([NotNull] this IConventionEntityType entityType)
            => ((IEntityType)entityType).GetNavigations().Cast<IConventionNavigation>();

        /// <summary>
        ///     <para>
        ///         Gets a property on the given entity type. Returns null if no property is found.
        ///     </para>
        ///     <para>
        ///         This API only finds scalar properties and does not find navigation properties. Use
        ///         <see cref="FindNavigation(IConventionEntityType, PropertyInfo)" /> to find a navigation property.
        ///     </para>
        /// </summary>
        /// <param name="entityType"> The entity type to find the property on. </param>
        /// <param name="propertyInfo"> The property on the entity class. </param>
        /// <returns> The property, or null if none is found. </returns>
        public static IConventionProperty FindProperty([NotNull] this IConventionEntityType entityType, [NotNull] PropertyInfo propertyInfo)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(propertyInfo, nameof(propertyInfo));

            return entityType.FindProperty(propertyInfo.GetSimpleMemberName());
        }

        /// <summary>
        ///     Adds a property to this entity.
        /// </summary>
        /// <param name="entityType"> The entity type to add the property to. </param>
        /// <param name="propertyInfo"> The corresponding property in the entity class. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The newly created property. </returns>
        public static IConventionProperty AddProperty(
            [NotNull] this IConventionEntityType entityType,
            [NotNull] PropertyInfo propertyInfo,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(propertyInfo, nameof(propertyInfo));

            return entityType.AsEntityType().AddProperty(
                propertyInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
        }

        /// <summary>
        ///     Gets the index defined on the given property. Returns null if no index is defined.
        /// </summary>
        /// <param name="entityType"> The entity type to find the index on. </param>
        /// <param name="property"> The property to find the index on. </param>
        /// <returns> The index, or null if none is found. </returns>
        public static IConventionIndex FindIndex([NotNull] this IConventionEntityType entityType, [NotNull] IProperty property)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.FindIndex(new[] { property });
        }

        /// <summary>
        ///     Adds an index to this entity.
        /// </summary>
        /// <param name="entityType"> The entity type to add the index to. </param>
        /// <param name="property"> The property to be indexed. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The newly created index. </returns>
        public static IConventionIndex AddIndex(
            [NotNull] this IConventionEntityType entityType,
            [NotNull] IConventionProperty property,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(entityType, nameof(entityType));

            return entityType.AddIndex(new[] { property }, fromDataAnnotation);
        }

        /// <summary>
        ///     Sets the change tracking strategy to use for this entity type. This strategy indicates how the
        ///     context detects changes to properties for an instance of the entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to set the change tracking strategy for. </param>
        /// <param name="changeTrackingStrategy"> The strategy to use. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetChangeTrackingStrategy(
            [NotNull] this IConventionEntityType entityType,
            ChangeTrackingStrategy? changeTrackingStrategy,
            bool fromDataAnnotation = false)
            => Check.NotNull(entityType, nameof(entityType)).AsEntityType()
                .SetChangeTrackingStrategy(
                    changeTrackingStrategy,
                    fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        /// <summary>
        ///     Returns the configuration source for <see cref="EntityTypeExtensions.GetChangeTrackingStrategy" />.
        /// </summary>
        /// <param name="entityType"> The entity type to find configuration source for. </param>
        /// <returns> The configuration source for <see cref="EntityTypeExtensions.GetChangeTrackingStrategy" />. </returns>
        public static ConfigurationSource? GetChangeTrackingStrategyConfigurationSource([NotNull] this IConventionEntityType entityType)
            => entityType.FindAnnotation(CoreAnnotationNames.ChangeTrackingStrategy)?.GetConfigurationSource();

        /// <summary>
        ///     Sets the LINQ expression filter automatically applied to queries for this entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to set the query filter for. </param>
        /// <param name="queryFilter"> The LINQ expression filter. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetQueryFilter(
            [NotNull] this IConventionEntityType entityType,
            [CanBeNull] LambdaExpression queryFilter,
            bool fromDataAnnotation = false)
            => Check.NotNull(entityType, nameof(entityType)).AsEntityType()
                .SetQueryFilter(
                    queryFilter,
                    fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        /// <summary>
        ///     Returns the configuration source for <see cref="EntityTypeExtensions.GetQueryFilter" />.
        /// </summary>
        /// <param name="entityType"> The entity type to find configuration source for. </param>
        /// <returns> The configuration source for <see cref="EntityTypeExtensions.GetQueryFilter" />. </returns>
        public static ConfigurationSource? GetQueryFilterConfigurationSource([NotNull] this IConventionEntityType entityType)
            => entityType.FindAnnotation(CoreAnnotationNames.QueryFilter)?.GetConfigurationSource();

        /// <summary>
        ///     Sets the LINQ query used as the default source for queries of this type.
        /// </summary>
        /// <param name="entityType"> The entity type to set the defining query for. </param>
        /// <param name="definingQuery"> The LINQ query used as the default source. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetDefiningQuery(
            [NotNull] this IConventionEntityType entityType,
            [CanBeNull] LambdaExpression definingQuery,
            bool fromDataAnnotation = false)
            => Check.NotNull(entityType, nameof(entityType)).AsEntityType()
                .SetDefiningQuery(
                    definingQuery,
                    fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        /// <summary>
        ///     Returns the configuration source for <see cref="EntityTypeExtensions.GetDefiningQuery" />.
        /// </summary>
        /// <param name="entityType"> The entity type to find configuration source for. </param>
        /// <returns> The configuration source for <see cref="EntityTypeExtensions.GetDefiningQuery" />. </returns>
        public static ConfigurationSource? GetDefiningQueryConfigurationSource([NotNull] this IConventionEntityType entityType)
            => entityType.FindAnnotation(CoreAnnotationNames.DefiningQuery)?.GetConfigurationSource();

        /// <summary>
        ///     Sets the <see cref="IProperty" /> that will be used for storing a discriminator value.
        /// </summary>
        /// <param name="entityType"> The entity type to set the discriminator property for. </param>
        /// <param name="property"> The property to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetDiscriminatorProperty(
            [NotNull] this IConventionEntityType entityType, [CanBeNull] IProperty property, bool fromDataAnnotation = false)
            => Check.NotNull(entityType, nameof(entityType)).AsEntityType()
                .SetDiscriminatorProperty(
                    property,
                    fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);

        /// <summary>
        ///     Gets the <see cref="ConfigurationSource" /> for the discriminator property.
        /// </summary>
        /// <param name="entityType"> The entity type to find configuration source for. </param>
        /// <returns> The <see cref="ConfigurationSource" /> or <c>null</c> if no discriminator property has been set. </returns>
        public static ConfigurationSource? GetDiscriminatorPropertyConfigurationSource([NotNull] this IConventionEntityType entityType)
            => entityType.FindAnnotation(CoreAnnotationNames.DiscriminatorProperty)
                ?.GetConfigurationSource();

        /// <summary>
        ///     Sets the discriminator value for this entity type.
        /// </summary>
        /// <param name="entityType"> The entity type to set the discriminator value for. </param>
        /// <param name="value"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetDiscriminatorValue(
            [NotNull] this IConventionEntityType entityType, [CanBeNull] object value, bool fromDataAnnotation = false)
        {
            entityType.AsEntityType().CheckDiscriminatorValue(entityType, value);

            entityType.SetAnnotation(CoreAnnotationNames.DiscriminatorValue, value, fromDataAnnotation);
        }

        /// <summary>
        ///     Removes the discriminator value for this entity type.
        /// </summary>
        public static void RemoveDiscriminatorValue([NotNull] this IConventionEntityType entityType)
            => entityType.RemoveAnnotation(CoreAnnotationNames.DiscriminatorValue);

        /// <summary>
        ///     Gets the <see cref="ConfigurationSource" /> for the discriminator value.
        /// </summary>
        /// <returns> The <see cref="ConfigurationSource" /> or <c>null</c> if no discriminator value has been set. </returns>
        public static ConfigurationSource? GetDiscriminatorValueConfigurationSource([NotNull] this IConventionEntityType entityType)
            => entityType.FindAnnotation(CoreAnnotationNames.DiscriminatorValue)
                ?.GetConfigurationSource();
    }
}
