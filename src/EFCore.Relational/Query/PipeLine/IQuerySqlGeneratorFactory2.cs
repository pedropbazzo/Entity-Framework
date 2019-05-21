﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore.Relational.Query.Pipeline
{
    public interface IQuerySqlGeneratorFactory2
    {
        QuerySqlGenerator Create();
    }
}
