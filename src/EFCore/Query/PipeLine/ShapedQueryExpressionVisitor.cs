﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.Pipeline
{
    public abstract class ShapedQueryCompilingExpressionVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo _singleMethodInfo
            = typeof(Enumerable).GetTypeInfo().GetDeclaredMethods(nameof(Enumerable.Single))
                .Single(mi => mi.GetParameters().Length == 1);

        private static readonly MethodInfo _singleOrDefaultMethodInfo
            = typeof(Enumerable).GetTypeInfo().GetDeclaredMethods(nameof(Enumerable.SingleOrDefault))
                .Single(mi => mi.GetParameters().Length == 1);

        private static readonly PropertyInfo _cancellationTokenMemberInfo
            = typeof(QueryContext).GetProperty(nameof(QueryContext.CancellationToken));

        private readonly IEntityMaterializerSource _entityMaterializerSource;
        private readonly bool _trackQueryResults;
        private readonly Expression cancellationTokenParameter;
        protected readonly bool Async;

        public ShapedQueryCompilingExpressionVisitor(IEntityMaterializerSource entityMaterializerSource, bool trackQueryResults, bool async)
        {
            _entityMaterializerSource = entityMaterializerSource;
            _trackQueryResults = trackQueryResults;
            Async = async;
            if (async)
            {
                cancellationTokenParameter = Expression.MakeMemberAccess(
                    QueryCompilationContext2.QueryContextParameter,
                    _cancellationTokenMemberInfo);
            }
        }

        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case ShapedQueryExpression shapedQueryExpression:
                    var serverEnumerable = VisitShapedQueryExpression(shapedQueryExpression);
                    switch (shapedQueryExpression.ResultType)
                    {
                        case ResultType.Enumerable:
                            return serverEnumerable;

                        case ResultType.Single:
                            return Async
                                ? Expression.Call(
                                    _singleAsyncMethodInfo.MakeGenericMethod(serverEnumerable.Type.TryGetSequenceType()),
                                    serverEnumerable,
                                    cancellationTokenParameter)
                                : Expression.Call(
                                    _singleMethodInfo.MakeGenericMethod(serverEnumerable.Type.TryGetSequenceType()),
                                    serverEnumerable);

                        case ResultType.SingleWithDefault:
                            return Async
                                ? Expression.Call(
                                    _singleOrDefaultAsyncMethodInfo.MakeGenericMethod(serverEnumerable.Type.TryGetSequenceType()),
                                    serverEnumerable,
                                    cancellationTokenParameter)
                                : Expression.Call(
                                    _singleOrDefaultMethodInfo.MakeGenericMethod(serverEnumerable.Type.TryGetSequenceType()),
                                    serverEnumerable);
                    }

                    break;
            }

            return base.VisitExtension(extensionExpression);
        }

        private static readonly MethodInfo _singleAsyncMethodInfo
            = typeof(ShapedQueryCompilingExpressionVisitor).GetTypeInfo()
                .GetDeclaredMethods(nameof(ShapedQueryCompilingExpressionVisitor.SingleAsync))
                .Single(mi => mi.GetParameters().Length == 2);

        private static readonly MethodInfo _singleOrDefaultAsyncMethodInfo
            = typeof(ShapedQueryCompilingExpressionVisitor).GetTypeInfo()
                .GetDeclaredMethods(nameof(ShapedQueryCompilingExpressionVisitor.SingleOrDefaultAsync))
                .Single(mi => mi.GetParameters().Length == 2);

        private async static Task<TSource> SingleAsync<TSource>(
            IAsyncEnumerable<TSource> asyncEnumerable,
            CancellationToken cancellationToken = default)
        {
            using (var enumerator = asyncEnumerable.GetEnumerator())
            {
                if (!(await enumerator.MoveNext(cancellationToken)))
                {
                    throw new InvalidOperationException();
                }

                var result = enumerator.Current;

                if (await enumerator.MoveNext(cancellationToken))
                {
                    throw new InvalidOperationException();
                }
                return result;
            }
        }

        private async static Task<TSource> SingleOrDefaultAsync<TSource>(
            IAsyncEnumerable<TSource> asyncEnumerable,
            CancellationToken cancellationToken = default)
        {
            using (var enumerator = asyncEnumerable.GetEnumerator())
            {
                if (!(await enumerator.MoveNext()))
                {
                    return default;
                }

                var result = enumerator.Current;

                if (await enumerator.MoveNext())
                {
                    throw new InvalidOperationException();
                }
                return result;
            }
        }

        protected abstract Expression VisitShapedQueryExpression(ShapedQueryExpression shapedQueryExpression);

        protected virtual Expression InjectEntityMaterializer(Expression expression)
        {
            return new EntityMaterializerInjectingExpressionVisitor(
                _entityMaterializerSource, _trackQueryResults, Async).Inject(expression);
        }

        private class EntityMaterializerInjectingExpressionVisitor : ExpressionVisitor
        {
            private static readonly ConstructorInfo _materializationContextConstructor
                = typeof(MaterializationContext).GetConstructors().Single(ci => ci.GetParameters().Length == 2);

            private static readonly PropertyInfo _dbContextMemberInfo
                = typeof(QueryContext).GetProperty(nameof(QueryContext.Context));
            private static readonly PropertyInfo _stateManagerMemberInfo
                = typeof(QueryContext).GetProperty(nameof(QueryContext.StateManager));
            private static readonly PropertyInfo _entityMemberInfo
                = typeof(InternalEntityEntry).GetProperty(nameof(InternalEntityEntry.Entity));
            private static readonly MethodInfo _taskFromResultMethodInfo
                = typeof(Task).GetTypeInfo().GetDeclaredMethods(nameof(Task.FromResult))
                    .Single();

            private static readonly MethodInfo _tryGetEntryMethodInfo
                = typeof(IStateManager).GetTypeInfo().GetDeclaredMethods(nameof(IStateManager.TryGetEntry))
                    .Single(mi => mi.GetParameters().Length == 4);
            private static readonly MethodInfo _startTrackingMethodInfo
                = typeof(QueryContext).GetMethod(nameof(QueryContext.StartTracking), new[] { typeof(IEntityType), typeof(object), typeof(ValueBuffer) });

            private readonly IEntityMaterializerSource _entityMaterializerSource;
            private readonly bool _trackQueryResults;
            private readonly bool _async;

            private readonly List<ParameterExpression> _variables = new List<ParameterExpression>();
            private readonly List<Expression> _expressions = new List<Expression>();
            private int _currentEntityIndex;

            public EntityMaterializerInjectingExpressionVisitor(
                IEntityMaterializerSource entityMaterializerSource, bool trackQueryResults, bool async)
            {
                _entityMaterializerSource = entityMaterializerSource;
                _trackQueryResults = trackQueryResults;
                _async = async;
            }

            public Expression Inject(Expression expression)
            {
                var modifiedBody = Visit(expression);
                _expressions.Add(
                    _async
                    ? Expression.Call(_taskFromResultMethodInfo.MakeGenericMethod(expression.Type), modifiedBody)
                    : modifiedBody);

                return Expression.Block(_variables, _expressions);
            }

            protected override Expression VisitExtension(Expression extensionExpression)
            {
                if (extensionExpression is EntityShaperExpression entityShaperExpression)
                {
                    return ProcessEntityShaper(entityShaperExpression);
                }

                if (extensionExpression is EntityValuesExpression entityValuesExpression)
                {
                    return Expression.NewArrayInit(
                        typeof(object),
                        entityValuesExpression.EntityType.GetProperties()
                            .Select(p => _entityMaterializerSource.CreateReadValueExpression(
                                entityValuesExpression.ValueBufferExpression,
                                typeof(object),
                                p.GetIndex(),
                                p)));
                }

                if (extensionExpression is CollectionShaperExpression collectionShaper)
                {
                    var keyType = collectionShaper.OuterKey.Type;
                    var comparerType = typeof(EqualityComparer<>).MakeGenericType(keyType);
                    var comparer = Expression.Variable(comparerType, "comparer" + _currentEntityIndex);

                    _variables.Add(comparer);
                    Expression.Assign(
                        comparer,
                        Expression.MakeMemberAccess(null, comparerType.GetProperty(nameof(EqualityComparer<int>.Default))));
                    var parent = Visit(collectionShaper.Parent);
                }

                if (extensionExpression is ProjectionBindingExpression)
                {
                    return extensionExpression;
                }

                return base.VisitExtension(extensionExpression);
            }

            private Expression ProcessEntityShaper(EntityShaperExpression entityShaperExpression)
            {
                _currentEntityIndex++;
                var expressions = new List<Expression>();
                var variables = new List<ParameterExpression>();

                var entityType = entityShaperExpression.EntityType;
                var valueBuffer = entityShaperExpression.ValueBufferExpression;

                var primaryKey = entityType.FindPrimaryKey();

                if (_trackQueryResults && primaryKey == null)
                {
                    throw new InvalidOperationException();
                }

                if (_trackQueryResults)
                {
                    var entry = Expression.Variable(typeof(InternalEntityEntry), "entry" + _currentEntityIndex);
                    var hasNullKey = Expression.Variable(typeof(bool), "hasNullKey" + _currentEntityIndex);
                    variables.Add(entry);
                    variables.Add(hasNullKey);

                    expressions.Add(
                        Expression.Assign(
                            entry,
                            Expression.Call(
                                Expression.MakeMemberAccess(
                                    QueryCompilationContext2.QueryContextParameter,
                                    _stateManagerMemberInfo),
                                _tryGetEntryMethodInfo,
                                Expression.Constant(primaryKey),
                                Expression.NewArrayInit(
                                    typeof(object),
                                    primaryKey.Properties
                                        .Select(p => _entityMaterializerSource.CreateReadValueExpression(
                                            entityShaperExpression.ValueBufferExpression,
                                            typeof(object),
                                            p.GetIndex(),
                                            p))),
                                Expression.Constant(!entityShaperExpression.Nullable),
                                hasNullKey)));

                    expressions.Add(Expression.Condition(
                        hasNullKey,
                        Expression.Constant(null, entityType.ClrType),
                        Expression.Condition(
                            Expression.NotEqual(
                                entry,
                                Expression.Constant(default(InternalEntityEntry), typeof(InternalEntityEntry))),
                            Expression.Convert(
                                Expression.MakeMemberAccess(entry, _entityMemberInfo),
                                entityType.ClrType),
                            MaterializeEntity(entityType, valueBuffer))));
                }
                else
                {
                    expressions.Add(Expression.Condition(
                        primaryKey.Properties
                                .Select(p =>
                                    Expression.Equal(
                                        _entityMaterializerSource.CreateReadValueExpression(
                                            entityShaperExpression.ValueBufferExpression,
                                            typeof(object),
                                            p.GetIndex(),
                                            p),
                                        Expression.Constant(null)))
                                    .Aggregate((a, b) => Expression.OrElse(a, b)),
                        Expression.Constant(null, entityType.ClrType),
                        MaterializeEntity(entityType, valueBuffer)));
                }

                return Expression.Block(variables, expressions);
            }

            private Expression MaterializeEntity(IEntityType entityType, Expression valueBuffer)
            {
                var expressions = new List<Expression>();

                var materializationContext = Expression.Variable(typeof(MaterializationContext), "materializationContext" + _currentEntityIndex);

                expressions.Add(
                    Expression.Assign(
                        materializationContext,
                        Expression.New(
                            _materializationContextConstructor,
                            valueBuffer,
                            Expression.MakeMemberAccess(
                                QueryCompilationContext2.QueryContextParameter,
                                _dbContextMemberInfo))));

                var materializationExpression
                    = _entityMaterializerSource.CreateMaterializeExpression(
                        entityType,
                        "instance" + _currentEntityIndex,
                        materializationContext);

                if (materializationExpression is BlockExpression blockExpression)
                {
                    expressions.AddRange(blockExpression.Expressions.Take(blockExpression.Expressions.Count - 1));

                    if (_trackQueryResults)
                    {
                        expressions.Add(
                            Expression.Call(
                                QueryCompilationContext2.QueryContextParameter,
                                _startTrackingMethodInfo,
                                Expression.Constant(entityType),
                                blockExpression.Expressions.Last(),
                                Expression.New(
                                    typeof(ValueBuffer).GetTypeInfo().DeclaredConstructors.Single(ci => ci.GetParameters().Length == 1),
                                    Expression.NewArrayInit(
                                        typeof(object),
                                        entityType.GetProperties().Where(p => p.IsShadowProperty())
                                            .Select(p => _entityMaterializerSource.CreateReadValueExpression(
                                                valueBuffer,
                                                typeof(object),
                                                p.GetIndex(),
                                                p))))));
                    }

                    expressions.Add(blockExpression.Expressions.Last());

                    return Expression.Block(
                        entityType.ClrType,
                        new[] { materializationContext }.Concat(blockExpression.Variables),
                        expressions);
                }
                else
                {
                    var instanceVariable = Expression.Variable(materializationExpression.Type, "instance" + _currentEntityIndex);
                    expressions.Add(Expression.Assign(instanceVariable, materializationExpression));

                    if (_trackQueryResults)
                    {
                        expressions.Add(
                            Expression.Call(
                                QueryCompilationContext2.QueryContextParameter,
                                _startTrackingMethodInfo,
                                Expression.Constant(entityType),
                                instanceVariable,
                                Expression.New(
                                    typeof(ValueBuffer).GetTypeInfo().DeclaredConstructors.Single(ci => ci.GetParameters().Length == 1),
                                    Expression.NewArrayInit(
                                        typeof(object),
                                        entityType.GetProperties().Where(p => p.IsShadowProperty())
                                            .Select(p => _entityMaterializerSource.CreateReadValueExpression(
                                                valueBuffer,
                                                typeof(object),
                                                p.GetIndex(),
                                                p))))));
                    }

                    expressions.Add(instanceVariable);

                    return Expression.Block(
                        entityType.ClrType,
                        new[] { materializationContext, instanceVariable },
                        expressions);
                }
            }
        }
    }
}
