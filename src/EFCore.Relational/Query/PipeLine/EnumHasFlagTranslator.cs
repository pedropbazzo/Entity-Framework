﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Relational.Query.Pipeline.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Relational.Query.Pipeline
{
    public class EnumHasFlagTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(Enum).GetRuntimeMethod(nameof(Enum.HasFlag), new[] { typeof(Enum) });

        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public EnumHasFlagTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public SqlExpression Translate(SqlExpression instance, MethodInfo method, IList<SqlExpression> arguments)
        {
            if (Equals(method, _methodInfo))
            {
                var argument = arguments[0];
                if (instance.Type.UnwrapNullableType() != argument.Type.UnwrapNullableType())
                {
                    return null;
                }

                return _sqlExpressionFactory.Equal(
                    _sqlExpressionFactory.And(instance, argument),
                    argument);
            }

            return null;
        }
    }
}
