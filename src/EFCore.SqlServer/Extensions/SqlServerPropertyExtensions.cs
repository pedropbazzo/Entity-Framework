// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Extension methods for <see cref="IProperty" /> for SQL Server-specific metadata.
    /// </summary>
    public static class SqlServerPropertyExtensions
    {
        /// <summary>
        ///     Returns the name to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The name to use for the hi-lo sequence. </returns>
        public static string GetSqlServerHiLoSequenceName([NotNull] this IProperty property)
            => (string)property[SqlServerAnnotationNames.HiLoSequenceName];

        /// <summary>
        ///     Sets the name to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="name"> The sequence name to use. </param>
        public static void SetSqlServerHiLoSequenceName([NotNull] this IMutableProperty property, string name)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.HiLoSequenceName,
                Check.NullButNotEmpty(name, nameof(name)));

        /// <summary>
        ///     Sets the name to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="name"> The sequence name to use. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerHiLoSequenceName(
            [NotNull] this IConventionProperty property, string name, bool fromDataAnnotation = false)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.HiLoSequenceName,
                Check.NullButNotEmpty(name, nameof(name)),
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the hi-lo sequence name.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the hi-lo sequence name. </returns>
        public static ConfigurationSource? GetSqlServerHiLoSequenceNameConfigurationSource([NotNull] this IConventionProperty property)
            => property.FindAnnotation(SqlServerAnnotationNames.HiLoSequenceName)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the schema to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The schema to use for the hi-lo sequence. </returns>
        public static string GetSqlServerHiLoSequenceSchema([NotNull] this IProperty property)
            => (string)property[SqlServerAnnotationNames.HiLoSequenceSchema];

        /// <summary>
        ///     Sets the schema to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="schema"> The schema to use. </param>
        public static void SetSqlServerHiLoSequenceSchema([NotNull] this IMutableProperty property, string schema)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.HiLoSequenceSchema,
                Check.NullButNotEmpty(schema, nameof(schema)));

        /// <summary>
        ///     Sets the schema to use for the hi-lo sequence.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="schema"> The schema to use. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerHiLoSequenceSchema(
            [NotNull] this IConventionProperty property, string schema, bool fromDataAnnotation = false)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.HiLoSequenceSchema,
                Check.NullButNotEmpty(schema, nameof(schema)),
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the hi-lo sequence schema.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the hi-lo sequence schema. </returns>
        public static ConfigurationSource? GetSqlServerHiLoSequenceSchemaConfigurationSource([NotNull] this IConventionProperty property)
            => property.FindAnnotation(SqlServerAnnotationNames.HiLoSequenceSchema)?.GetConfigurationSource();

        /// <summary>
        ///     Finds the <see cref="ISequence" /> in the model to use for the hi-lo pattern.
        /// </summary>
        /// <returns> The sequence to use, or <c>null</c> if no sequence exists in the model. </returns>
        public static ISequence FindSqlServerHiLoSequence([NotNull] this IProperty property)
        {
            var model = property.DeclaringEntityType.Model;

            if (property.GetSqlServerValueGenerationStrategy() != SqlServerValueGenerationStrategy.SequenceHiLo)
            {
                return null;
            }

            var sequenceName = property.GetSqlServerHiLoSequenceName()
                               ?? model.GetSqlServerHiLoSequenceName();

            var sequenceSchema = property.GetSqlServerHiLoSequenceSchema()
                                 ?? model.GetSqlServerHiLoSequenceSchema();

            return model.FindSequence(sequenceName, sequenceSchema);
        }

        /// <summary>
        ///     Returns the identity seed.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The identity seed. </returns>
        public static int? GetSqlServerIdentitySeed([NotNull] this IProperty property)
            => (int?)property[SqlServerAnnotationNames.IdentitySeed];

        /// <summary>
        ///     Sets the identity seed.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="seed"> The value to set. </param>
        public static void SetSqlServerIdentitySeed([NotNull] this IMutableProperty property, int? seed)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentitySeed,
                seed);

        /// <summary>
        ///     Sets the identity seed.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="seed"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerIdentitySeed(
            [NotNull] this IConventionProperty property, int? seed, bool fromDataAnnotation = false)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentitySeed,
                seed,
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the identity seed.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the identity seed. </returns>
        public static ConfigurationSource? GetSqlServerIdentitySeedConfigurationSource([NotNull] this IConventionProperty property)
            => property.FindAnnotation(SqlServerAnnotationNames.IdentitySeed)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the identity increment.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The identity increment. </returns>
        public static int? GetSqlServerIdentityIncrement([NotNull] this IProperty property)
            => (int?)property[SqlServerAnnotationNames.IdentityIncrement];

        /// <summary>
        ///     Sets the identity increment.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="increment"> The value to set. </param>
        public static void SetSqlServerIdentityIncrement([NotNull] this IMutableProperty property, int? increment)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentityIncrement,
                increment);

        /// <summary>
        ///     Sets the identity increment.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="increment"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerIdentityIncrement(
            [NotNull] this IConventionProperty property, int? increment, bool fromDataAnnotation = false)
            => property.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentityIncrement,
                increment,
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the identity increment.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the identity increment. </returns>
        public static ConfigurationSource? GetSqlServerIdentityIncrementConfigurationSource([NotNull] this IConventionProperty property)
            => property.FindAnnotation(SqlServerAnnotationNames.IdentityIncrement)?.GetConfigurationSource();

        /// <summary>
        ///     <para>
        ///         Returns the <see cref="SqlServerValueGenerationStrategy" /> to use for the property.
        ///     </para>
        ///     <para>
        ///         If no strategy is set for the property, then the strategy to use will be taken from the <see cref="IModel" />
        ///     </para>
        /// </summary>
        /// <returns> The strategy, or <c>null</c> if none was set. </returns>
        public static SqlServerValueGenerationStrategy? GetSqlServerValueGenerationStrategy([NotNull] this IProperty property)
        {
            var annotation = property.FindAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy);
            if (annotation != null)
            {
                return (SqlServerValueGenerationStrategy?)annotation.Value;
            }

            var sharedTablePrincipalPrimaryKeyProperty = property.FindSharedTableRootPrimaryKeyProperty();
            if (sharedTablePrincipalPrimaryKeyProperty != null)
            {
                return sharedTablePrincipalPrimaryKeyProperty.GetSqlServerValueGenerationStrategy()
                       == SqlServerValueGenerationStrategy.IdentityColumn
                    ? (SqlServerValueGenerationStrategy?)SqlServerValueGenerationStrategy.IdentityColumn
                    : null;
            }

            if (property.ValueGenerated != ValueGenerated.OnAdd
                || property.GetDefaultValue() != null
                || property.GetDefaultValueSql() != null
                || property.GetComputedColumnSql() != null)
            {
                return null;
            }

            var modelStrategy = property.DeclaringEntityType.Model.GetSqlServerValueGenerationStrategy();

            if (modelStrategy == SqlServerValueGenerationStrategy.SequenceHiLo
                && IsCompatibleWithValueGeneration(property))
            {
                return SqlServerValueGenerationStrategy.SequenceHiLo;
            }

            return modelStrategy == SqlServerValueGenerationStrategy.IdentityColumn
                   && IsCompatibleWithValueGeneration(property)
                ? (SqlServerValueGenerationStrategy?)SqlServerValueGenerationStrategy.IdentityColumn
                : null;
        }

        /// <summary>
        ///     Sets the <see cref="SqlServerValueGenerationStrategy" /> to use for the property.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="value"> The strategy to use. </param>
        public static void SetSqlServerValueGenerationStrategy(
            [NotNull] this IMutableProperty property, SqlServerValueGenerationStrategy? value)
        {
            CheckSqlServerValueGenerationStrategy(property, value);

            property.SetAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, value);
        }

        /// <summary>
        ///     Sets the <see cref="SqlServerValueGenerationStrategy" /> to use for the property.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <param name="value"> The strategy to use. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerValueGenerationStrategy(
            [NotNull] this IConventionProperty property, SqlServerValueGenerationStrategy? value, bool fromDataAnnotation = false)
        {
            CheckSqlServerValueGenerationStrategy(property, value);

            property.SetAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, value, fromDataAnnotation);
        }

        private static void CheckSqlServerValueGenerationStrategy(IProperty property, SqlServerValueGenerationStrategy? value)
        {
            if (value != null)
            {
                var propertyType = property.ClrType;

                if (value == SqlServerValueGenerationStrategy.IdentityColumn
                    && !IsCompatibleWithValueGeneration(property))
                {
                    throw new ArgumentException(
                        SqlServerStrings.IdentityBadType(
                            property.Name, property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
                }

                if (value == SqlServerValueGenerationStrategy.SequenceHiLo
                    && !IsCompatibleWithValueGeneration(property))
                {
                    throw new ArgumentException(
                        SqlServerStrings.SequenceBadType(
                            property.Name, property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
                }
            }
        }

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the <see cref="SqlServerValueGenerationStrategy" />.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the <see cref="SqlServerValueGenerationStrategy" />. </returns>
        public static ConfigurationSource? GetSqlServerValueGenerationStrategyConfigurationSource(
            [NotNull] this IConventionProperty property)
            => property.FindAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy)?.GetConfigurationSource();

        /// <summary>
        ///     Returns a value indicating whether the property is compatible with any <see cref="SqlServerValueGenerationStrategy"/>.
        /// </summary>
        /// <param name="property"> The property. </param>
        /// <returns> <c>true</c> if compatible. </returns>
        public static bool IsCompatibleWithValueGeneration(IProperty property)
        {
            var type = property.ClrType;

            return (type.IsInteger()
                    || type == typeof(decimal))
                   && (property.FindMapping()?.Converter
                       ?? property.GetValueConverter()) == null;
        }
    }
}
