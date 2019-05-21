// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Extension methods for <see cref="IModel" /> for SQL Server-specific metadata.
    /// </summary>
    public static class SqlServerModelExtensions
    {
        /// <summary>
        ///     The default name for the hi-lo sequence.
        /// </summary>
        public const string DefaultHiLoSequenceName = "EntityFrameworkHiLoSequence";

        /// <summary>
        ///     Returns the name to use for the default hi-lo sequence.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The name to use for the default hi-lo sequence. </returns>
        public static string GetSqlServerHiLoSequenceName([NotNull] this IModel model)
            => (string)model[SqlServerAnnotationNames.HiLoSequenceName]
               ?? DefaultHiLoSequenceName;

        /// <summary>
        ///     Sets the name to use for the default hi-lo sequence.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="name"> The value to set. </param>
        public static void SetSqlServerHiLoSequenceName([NotNull] this IMutableModel model, string name)
        {
            Check.NullButNotEmpty(name, nameof(name));

            model.SetOrRemoveAnnotation(SqlServerAnnotationNames.HiLoSequenceName, name);
        }

        /// <summary>
        ///     Sets the name to use for the default hi-lo sequence.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="name"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerHiLoSequenceName(
            [NotNull] this IConventionModel model, string name, bool fromDataAnnotation = false)
        {
            Check.NullButNotEmpty(name, nameof(name));

            model.SetOrRemoveAnnotation(SqlServerAnnotationNames.HiLoSequenceName, name, fromDataAnnotation);
        }

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the default hi-lo sequence name.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the default hi-lo sequence name. </returns>
        public static ConfigurationSource? GetSqlServerHiLoSequenceNameConfigurationSource([NotNull] this IConventionModel model)
            => model.FindAnnotation(SqlServerAnnotationNames.HiLoSequenceName)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the schema to use for the default hi-lo sequence.
        ///     <see cref="SqlServerPropertyBuilderExtensions.ForSqlServerUseSequenceHiLo" />
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The schema to use for the default hi-lo sequence. </returns>
        public static string GetSqlServerHiLoSequenceSchema([NotNull] this IModel model)
            => (string)model[SqlServerAnnotationNames.HiLoSequenceSchema];

        /// <summary>
        ///     Sets the schema to use for the default hi-lo sequence.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="value"> The value to set. </param>
        public static void SetSqlServerHiLoSequenceSchema([NotNull] this IMutableModel model, string value)
        {
            Check.NullButNotEmpty(value, nameof(value));

            model.SetOrRemoveAnnotation(SqlServerAnnotationNames.HiLoSequenceSchema, value);
        }

        /// <summary>
        ///     Sets the schema to use for the default hi-lo sequence.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="value"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerHiLoSequenceSchema(
            [NotNull] this IConventionModel model, string value, bool fromDataAnnotation = false)
        {
            Check.NullButNotEmpty(value, nameof(value));

            model.SetOrRemoveAnnotation(SqlServerAnnotationNames.HiLoSequenceSchema, value, fromDataAnnotation);
        }

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the default hi-lo sequence schema.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the default hi-lo sequence schema. </returns>
        public static ConfigurationSource? GetSqlServerHiLoSequenceSchemaConfigurationSource([NotNull] this IConventionModel model)
            => model.FindAnnotation(SqlServerAnnotationNames.HiLoSequenceSchema)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the default identity seed.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The default identity seed. </returns>
        public static int GetSqlServerIdentitySeed([NotNull] this IModel model)
            => (int?)model[SqlServerAnnotationNames.IdentitySeed] ?? 1;

        /// <summary>
        ///     Sets the default identity seed.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="seed"> The value to set. </param>
        public static void SetSqlServerIdentitySeed([NotNull] this IMutableModel model, int? seed)
            => model.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentitySeed,
                seed);

        /// <summary>
        ///     Sets the default identity seed.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="seed"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerIdentitySeed([NotNull] this IConventionModel model, int? seed, bool fromDataAnnotation = false)
            => model.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentitySeed,
                seed,
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the default schema.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the default schema. </returns>
        public static ConfigurationSource? GetSqlServerIdentitySeedConfigurationSource([NotNull] this IConventionModel model)
            => model.FindAnnotation(SqlServerAnnotationNames.IdentitySeed)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the default identity increment.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The default identity increment. </returns>
        public static int GetSqlServerIdentityIncrement([NotNull] this IModel model)
            => (int?)model[SqlServerAnnotationNames.IdentityIncrement] ?? 1;

        /// <summary>
        ///     Sets the default identity increment.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="increment"> The value to set. </param>
        public static void SetSqlServerIdentityIncrement([NotNull] this IMutableModel model, int? increment)
            => model.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentityIncrement,
                increment);

        /// <summary>
        ///     Sets the default identity increment.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="increment"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerIdentityIncrement(
            [NotNull] this IConventionModel model, int? increment, bool fromDataAnnotation = false)
            => model.SetOrRemoveAnnotation(
                SqlServerAnnotationNames.IdentityIncrement,
                increment,
                fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the default identity increment.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the default identity increment. </returns>
        public static ConfigurationSource? GetSqlServerIdentityIncrementConfigurationSource([NotNull] this IConventionModel model)
            => model.FindAnnotation(SqlServerAnnotationNames.IdentityIncrement)?.GetConfigurationSource();

        /// <summary>
        ///     Returns the <see cref="SqlServerValueGenerationStrategy" /> to use for properties
        ///     of keys in the model, unless the property has a strategy explicitly set.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The default <see cref="SqlServerValueGenerationStrategy" />. </returns>
        public static SqlServerValueGenerationStrategy? GetSqlServerValueGenerationStrategy([NotNull] this IModel model)
            => (SqlServerValueGenerationStrategy?)model[SqlServerAnnotationNames.ValueGenerationStrategy];

        /// <summary>
        ///     Attempts to set the <see cref="SqlServerValueGenerationStrategy" /> to use for properties
        ///     of keys in the model that don't have a strategy explicitly set.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="value"> The value to set. </param>
        public static void SetSqlServerValueGenerationStrategy([NotNull] this IMutableModel model, SqlServerValueGenerationStrategy? value)
            => model.SetOrRemoveAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, value);

        /// <summary>
        ///     Attempts to set the <see cref="SqlServerValueGenerationStrategy" /> to use for properties
        ///     of keys in the model that don't have a strategy explicitly set.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <param name="value"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetSqlServerValueGenerationStrategy(
            [NotNull] this IConventionModel model, SqlServerValueGenerationStrategy? value, bool fromDataAnnotation = false)
            => model.SetOrRemoveAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy, value, fromDataAnnotation);

        /// <summary>
        ///     Returns the <see cref="ConfigurationSource" /> for the default <see cref="SqlServerValueGenerationStrategy" />.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ConfigurationSource" /> for the default <see cref="SqlServerValueGenerationStrategy" />. </returns>
        public static ConfigurationSource? GetSqlServerValueGenerationStrategyConfigurationSource([NotNull] this IConventionModel model)
            => model.FindAnnotation(SqlServerAnnotationNames.ValueGenerationStrategy)?.GetConfigurationSource();
    }
}
