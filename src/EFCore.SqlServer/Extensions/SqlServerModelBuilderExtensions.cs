// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="ModelBuilder" />.
    /// </summary>
    public static class SqlServerModelBuilderExtensions
    {
        /// <summary>
        ///     Configures the model to use a sequence-based hi-lo pattern to generate values for key properties
        ///     marked as <see cref="ValueGenerated.OnAdd" />, when targeting SQL Server.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="name"> The name of the sequence. </param>
        /// <param name="schema">The schema of the sequence. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static ModelBuilder ForSqlServerUseSequenceHiLo(
            [NotNull] this ModelBuilder modelBuilder,
            [CanBeNull] string name = null,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NullButNotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            var model = modelBuilder.Model;

            name ??= SqlServerModelExtensions.DefaultHiLoSequenceName;

            if (model.FindSequence(name, schema) == null)
            {
                modelBuilder.HasSequence(name, schema).IncrementsBy(10);
            }

            model.SetSqlServerValueGenerationStrategy(SqlServerValueGenerationStrategy.SequenceHiLo);
            model.SetSqlServerHiLoSequenceName(name);
            model.SetSqlServerHiLoSequenceSchema(schema);
            model.SetSqlServerIdentitySeed(null);
            model.SetSqlServerIdentityIncrement(null);

            return modelBuilder;
        }

        /// <summary>
        ///     Configures the database sequence used for the hi-lo pattern to generate values for key properties
        ///     marked as <see cref="ValueGenerated.OnAdd" />, when targeting SQL Server.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="name"> The name of the sequence. </param>
        /// <param name="schema">The schema of the sequence. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> A builder to further configure the sequence. </returns>
        public static IConventionSequenceBuilder ForSqlServerHasHiLoSequence(
            [NotNull] this IConventionModelBuilder modelBuilder,
            [CanBeNull] string name,
            [CanBeNull] string schema,
            bool fromDataAnnotation = false)
        {
            if (!modelBuilder.ForSqlServerCanSetHiLoSequence(name, schema))
            {
                return null;
            }

            modelBuilder.Metadata.SetSqlServerHiLoSequenceName(name, fromDataAnnotation);
            modelBuilder.Metadata.SetSqlServerHiLoSequenceSchema(schema, fromDataAnnotation);

            return name == null ? null : modelBuilder.HasSequence(name, schema, fromDataAnnotation);
        }

        /// <summary>
        ///     Returns a value indicating whether the given name and schema can be set for the hi-lo sequence.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="name"> The name of the sequence. </param>
        /// <param name="schema">The schema of the sequence. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> <c>true</c> if the given name and schema can be set for the hi-lo sequence. </returns>
        public static bool ForSqlServerCanSetHiLoSequence(
            [NotNull] this IConventionModelBuilder modelBuilder,
            [CanBeNull] string name,
            [CanBeNull] string schema,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NullButNotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            return modelBuilder.CanSetAnnotation(SqlServerAnnotationNames.HiLoSequenceName, name, fromDataAnnotation)
                   && modelBuilder.CanSetAnnotation(SqlServerAnnotationNames.HiLoSequenceSchema, schema, fromDataAnnotation);
        }

        /// <summary>
        ///     Configures the model to use the SQL Server IDENTITY feature to generate values for key properties
        ///     marked as <see cref="ValueGenerated.OnAdd" />, when targeting SQL Server. This is the default
        ///     behavior when targeting SQL Server.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="seed"> The value that is used for the very first row loaded into the table. </param>
        /// <param name="increment"> The incremental value that is added to the identity value of the previous row that was loaded. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static ModelBuilder ForSqlServerUseIdentityColumns(
            [NotNull] this ModelBuilder modelBuilder,
            int seed = 1,
            int increment = 1)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            var model = modelBuilder.Model;

            model.SetSqlServerValueGenerationStrategy(SqlServerValueGenerationStrategy.IdentityColumn);
            model.SetSqlServerIdentitySeed(seed);
            model.SetSqlServerIdentityIncrement(increment);
            model.SetSqlServerHiLoSequenceName(null);
            model.SetSqlServerHiLoSequenceSchema(null);

            return modelBuilder;
        }

        /// <summary>
        ///     Configures the default seed for SQL Server IDENTITY.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="seed"> The value that is used for the very first row loaded into the table. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns>
        ///     The same builder instance if the configuration was applied,
        ///     <c>null</c> otherwise.
        /// </returns>
        public static IConventionModelBuilder ForSqlServerHasIdentitySeed(
            [NotNull] this IConventionModelBuilder modelBuilder, int? seed, bool fromDataAnnotation = false)
        {
            if (modelBuilder.ForSqlServerCanSetIdentitySeed(seed, fromDataAnnotation))
            {
                modelBuilder.Metadata.SetSqlServerIdentitySeed(seed, fromDataAnnotation);
                return modelBuilder;
            }

            return null;
        }

        /// <summary>
        ///     Returns a value indicating whether the given value can be set as the default seed for SQL Server IDENTITY.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="seed"> The value that is used for the very first row loaded into the table. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> <c>true</c> if the given value can be set as the seed for SQL Server IDENTITY. </returns>
        public static bool ForSqlServerCanSetIdentitySeed(
            [NotNull] this IConventionModelBuilder modelBuilder, int? seed, bool fromDataAnnotation = false)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.CanSetAnnotation(SqlServerAnnotationNames.IdentitySeed, seed, fromDataAnnotation);
        }

        /// <summary>
        ///     Configures the default increment for SQL Server IDENTITY.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="increment"> The incremental value that is added to the identity value of the previous row that was loaded. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns>
        ///     The same builder instance if the configuration was applied,
        ///     <c>null</c> otherwise.
        /// </returns>
        public static IConventionModelBuilder ForSqlServerHasIdentityIncrement(
            [NotNull] this IConventionModelBuilder modelBuilder, int? increment, bool fromDataAnnotation = false)
        {
            if (modelBuilder.ForSqlServerCanSetIdentityIncrement(increment, fromDataAnnotation))
            {
                modelBuilder.Metadata.SetSqlServerIdentityIncrement(increment, fromDataAnnotation);
                return modelBuilder;
            }

            return null;
        }

        /// <summary>
        ///     Returns a value indicating whether the given value can be set as the default increment for SQL Server IDENTITY.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="increment"> The incremental value that is added to the identity value of the previous row that was loaded. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> <c>true</c> if the given value can be set as the default increment for SQL Server IDENTITY. </returns>
        public static bool ForSqlServerCanSetIdentityIncrement(
            [NotNull] this IConventionModelBuilder modelBuilder, int? increment, bool fromDataAnnotation = false)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.CanSetAnnotation(SqlServerAnnotationNames.IdentityIncrement, increment, fromDataAnnotation);
        }

        /// <summary>
        ///     Configures the default value generation strategy for key properties marked as <see cref="ValueGenerated.OnAdd" />,
        ///     when targeting SQL Server.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="valueGenerationStrategy"> The value generation strategy. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns>
        ///     The same builder instance if the configuration was applied,
        ///     <c>null</c> otherwise.
        /// </returns>
        public static IConventionModelBuilder ForSqlServerHasValueGenerationStrategy(
            [NotNull] this IConventionModelBuilder modelBuilder,
            SqlServerValueGenerationStrategy? valueGenerationStrategy,
            bool fromDataAnnotation = false)
        {
            if (modelBuilder.ForSqlServerCanSetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation))
            {
                modelBuilder.Metadata.SetSqlServerValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
                if (valueGenerationStrategy != SqlServerValueGenerationStrategy.IdentityColumn)
                {
                    modelBuilder.ForSqlServerHasIdentitySeed(null, fromDataAnnotation);
                    modelBuilder.ForSqlServerHasIdentityIncrement(null, fromDataAnnotation);
                }

                if (valueGenerationStrategy != SqlServerValueGenerationStrategy.SequenceHiLo)
                {
                    modelBuilder.ForSqlServerHasHiLoSequence(null, null, fromDataAnnotation);
                }

                return modelBuilder;
            }

            return null;
        }

        /// <summary>
        ///     Returns a value indicating whether the given value can be set as the default value generation strategy.
        /// </summary>
        /// <param name="modelBuilder"> The model builder. </param>
        /// <param name="valueGenerationStrategy"> The value generation strategy. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> <c>true</c> if the given value can be set as the default value generation strategy. </returns>
        public static bool ForSqlServerCanSetValueGenerationStrategy(
            [NotNull] this IConventionModelBuilder modelBuilder,
            SqlServerValueGenerationStrategy? valueGenerationStrategy,
            bool fromDataAnnotation = false)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.CanSetAnnotation(
                SqlServerAnnotationNames.ValueGenerationStrategy, valueGenerationStrategy, fromDataAnnotation);
        }
    }
}
