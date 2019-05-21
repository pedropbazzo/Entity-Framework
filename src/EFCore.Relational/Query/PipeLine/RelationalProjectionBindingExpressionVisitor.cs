﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Query.Pipeline;
using Microsoft.EntityFrameworkCore.Relational.Query.Pipeline.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Relational.Query.Pipeline
{
    public class RelationalProjectionBindingExpressionVisitor : ExpressionVisitor
    {
        private readonly RelationalSqlTranslatingExpressionVisitor _sqlTranslator;

        private SelectExpression _selectExpression;
        private readonly IDictionary<ProjectionMember, Expression> _projectionMapping
            = new Dictionary<ProjectionMember, Expression>();

        private readonly Stack<ProjectionMember> _projectionMembers = new Stack<ProjectionMember>();

        public RelationalProjectionBindingExpressionVisitor(
            RelationalSqlTranslatingExpressionVisitor sqlTranslatingExpressionVisitor)
        {
            _sqlTranslator = sqlTranslatingExpressionVisitor;
        }

        public Expression Translate(SelectExpression selectExpression, Expression expression)
        {
            _selectExpression = selectExpression;

            _projectionMembers.Push(new ProjectionMember());

            var result = Visit(expression);

            _selectExpression.ApplyProjection(_projectionMapping);

            _selectExpression = null;
            _projectionMembers.Clear();
            _projectionMapping.Clear();

            return result;
        }

        public override Expression Visit(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (!(expression is NewExpression
                  || expression is MemberInitExpression
                  || expression is EntityShaperExpression))
            {
                // This skips the group parameter from GroupJoin
                if (expression is ParameterExpression parameter
                    && parameter.Type.IsGenericType
                    && parameter.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return parameter;
                }

                // This converts object[] from GetDatabaseValues to appropriate projection.
                if (expression is NewArrayExpression newArrayExpression
                    && newArrayExpression.NodeType == ExpressionType.NewArrayInit
                    && newArrayExpression.Expressions.Count > 0
                    && newArrayExpression.Expressions[0] is UnaryExpression unaryExpression
                    && unaryExpression.NodeType == ExpressionType.Convert
                    && unaryExpression.Type == typeof(object)
                    && unaryExpression.Operand is MethodCallExpression methodCall
                    && methodCall.Method.IsEFPropertyMethod()
                    && methodCall.Arguments[0] is EntityShaperExpression entityShaperExpression
                    && entityShaperExpression.EntityType.GetProperties().Count() == newArrayExpression.Expressions.Count)
                {
                    VerifySelectExpression(entityShaperExpression.ValueBufferExpression);

                    _projectionMapping[_projectionMembers.Peek()]
                       = _selectExpression.GetProjectionExpression(
                           entityShaperExpression.ValueBufferExpression.ProjectionMember);

                    return new EntityValuesExpression(entityShaperExpression.EntityType, entityShaperExpression.ValueBufferExpression);
                }

                var translation = _sqlTranslator.Translate(expression);

                _projectionMapping[_projectionMembers.Peek()] = translation ?? throw new InvalidOperationException();

                return new ProjectionBindingExpression(_selectExpression, _projectionMembers.Peek(), expression.Type);
            }

            return base.Visit(expression);
        }

        protected override Expression VisitExtension(Expression extensionExpression)
        {
            if (extensionExpression is EntityShaperExpression entityShaperExpression)
            {
                VerifySelectExpression(entityShaperExpression.ValueBufferExpression);

                _projectionMapping[_projectionMembers.Peek()]
                    = _selectExpression.GetProjectionExpression(
                        entityShaperExpression.ValueBufferExpression.ProjectionMember);

                return entityShaperExpression.Update(
                    new ProjectionBindingExpression(_selectExpression, _projectionMembers.Peek(), typeof(ValueBuffer)));
            }

            throw new InvalidOperationException();
        }

        protected override Expression VisitNew(NewExpression newExpression)
        {
            var newArguments = new Expression[newExpression.Arguments.Count];
            for (var i = 0; i < newArguments.Length; i++)
            {
                // TODO: Members can be null????
                var projectionMember = _projectionMembers.Peek().AddMember(newExpression.Members[i]);
                _projectionMembers.Push(projectionMember);

                newArguments[i] = Visit(newExpression.Arguments[i]);
                _projectionMembers.Pop();
            }

            return newExpression.Update(newArguments);
        }

        protected override Expression VisitMemberInit(MemberInitExpression memberInitExpression)
        {
            var newExpression = (NewExpression)Visit(memberInitExpression.NewExpression);
            var newBindings = new MemberAssignment[memberInitExpression.Bindings.Count];
            for (var i = 0; i < newBindings.Length; i++)
            {
                // TODO: Members can be null????
                var memberAssignment = (MemberAssignment)memberInitExpression.Bindings[i];

                var projectionMember = _projectionMembers.Peek().AddMember(memberAssignment.Member);
                _projectionMembers.Push(projectionMember);

                newBindings[i] = memberAssignment.Update(Visit(memberAssignment.Expression));
                _projectionMembers.Pop();
            }

            return memberInitExpression.Update(newExpression, newBindings);
        }

        // TODO: Debugging
        private void VerifySelectExpression(ProjectionBindingExpression projectionBindingExpression)
        {
            if (projectionBindingExpression.QueryExpression != _selectExpression)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
