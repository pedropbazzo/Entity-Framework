﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class SimpleQuerySqliteTest : SimpleQueryTestBase<NorthwindQuerySqliteFixture<NoopModelCustomizer>>
    {
        // ReSharper disable once UnusedParameter.Local
        public SimpleQuerySqliteTest(NorthwindQuerySqliteFixture<NoopModelCustomizer> fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        // Skip for SQLite. Issue #14935. Cannot eval 'Average()'
        public override Task Average_with_coalesce(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Average()'
        public override Task Average_with_division_on_decimal(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Average()'
        public override Task Average_with_division_on_decimal_no_significant_digits(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Convert(ToByte(ToByte(([o].OrderID % 1))), Int32) >= 0)'
        public override Task Convert_ToByte(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (ToDecimal(ToByte(([o].OrderID % 1))) >= 0)'
        public override Task Convert_ToDecimal(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (ToDouble(ToByte(([o].OrderID % 1))) >= 0)'
        public override Task Convert_ToDouble(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Convert(ToInt16(ToByte(([o].OrderID % 1))), Int32) >= 0)'
        public override Task Convert_ToInt16(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (ToInt32(ToByte(([o].OrderID % 1))) >= 0)'
        public override Task Convert_ToInt32(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (ToInt64(ToByte(([o].OrderID % 1))) >= 0)'
        public override Task Convert_ToInt64(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (ToString(ToByte(([o].OrderID % 1))) != \"10\")'
        public override Task Convert_ToString(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'from Order o in {from Order o in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Order]) where ([o].CustomerID == [c].CustomerID) select [o] => DefaultIfEmpty()}'
        public override Task DefaultIfEmpty_in_subquery(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'from Order o1 in {from Order o in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Order]) where ([o].OrderID > 11000) select [o] => DefaultIfEmpty()}'
        public override Task DefaultIfEmpty_in_subquery_nested(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Max()'
        public override Task Max_with_coalesce(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Min()'
        public override Task Min_with_coalesce(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'orderby Truncate(Convert([o].OrderID, Double)) asc'
        public override Task Projecting_Math_Truncate_and_ordering_by_it_twice(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'orderby Truncate(Convert([o].OrderID, Double)) asc'
        public override Task Projecting_Math_Truncate_and_ordering_by_it_twice2(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'orderby Truncate(Convert([o].OrderID, Double)) desc, Truncate(Convert([o].OrderID, Double)) asc'
        public override Task Projecting_Math_Truncate_and_ordering_by_it_twice3(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where Convert([o].EmployeeID, UInt32).ToString().Contains(\"10\")'
        public override Task Query_expression_with_to_string_and_contains(bool isAsync) => null;

        // TODO: Client Eval.
        public override Task Select_math_truncate_int(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'from Order o in {from Order o in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Order]) where ([o].CustomerID == [c].CustomerID) select [o] => DefaultIfEmpty()}'
        public override Task SelectMany_Joined_DefaultIfEmpty(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'from Order o in {from Order o in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Order]) where ([o].CustomerID == [c].CustomerID) select [o] => DefaultIfEmpty()}'
        public override Task SelectMany_Joined_DefaultIfEmpty2(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'from Order o in {from Order o in value(Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[Microsoft.EntityFrameworkCore.TestModels.Northwind.Order]) where ([o].CustomerID == [c].CustomerID) select [o] => Take(1000)}'
        public override Task SelectMany_Joined_Take(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Sum()'
        public override Task Sum_with_division_on_decimal(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'Sum()'
        public override Task Sum_with_division_on_decimal_no_significant_digits(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Convert([o].OrderDate, Nullable`1) == Convert(DateTimeOffset.Now, Nullable`1))'
        public override Task Where_datetimeoffset_now_component(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Convert([o].OrderDate, Nullable`1) == Convert(DateTimeOffset.UtcNow, Nullable`1))'
        public override Task Where_datetimeoffset_utcnow_component(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Pow(Convert([c].CustomerID.Length, Double), 2) == 25)'
        public override Task Where_functions_nested(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (NewGuid() != 00000000-0000-0000-0000-000000000000)'
        public override Task Where_guid_newguid(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Abs([od].UnitPrice) > 10)'
        public override Task Where_math_abs3(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Acos(Convert([od].Discount, Double)) > 1)'
        public override Task Where_math_acos(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Asin(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_asin(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Atan(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_atan(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Atan2(Convert([od].Discount, Double), 1) > 0)'
        public override Task Where_math_atan2(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Ceiling(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_ceiling1(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Ceiling([od].UnitPrice) > 10)'
        public override Task Where_math_ceiling2(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Cos(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_cos(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Exp(Convert([od].Discount, Double)) > 1)'
        public override Task Where_math_exp(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Floor([od].UnitPrice) > 10)'
        public override Task Where_math_floor(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Log(Convert([od].Discount, Double)) < 0)'
        public override Task Where_math_log(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Log(Convert([od].Discount, Double), 7) < 0)'
        public override Task Where_math_log_new_base(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Log10(Convert([od].Discount, Double)) < 0)'
        public override Task Where_math_log10(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Pow(Convert([od].Discount, Double), 2) > 0.05000000074505806)'
        public override Task Where_math_power(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Round([od].UnitPrice) > 10'
        public override Task Where_math_round(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Round([od].UnitPrice, 2) > 100)'
        public override Task Where_math_round2(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Sign([od].Discount) > 0)'
        public override Task Where_math_sign(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Sin(Convert([od].Discount, Double)) > 0'
        public override Task Where_math_sin(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Sqrt(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_sqrt(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Tan(Convert([od].Discount, Double)) > 0)'
        public override Task Where_math_tan(bool isAsync) => null;

        // Skip for SQLite. Issue #14935. Cannot eval 'where (Truncate([od].UnitPrice) > 10)'
        public override Task Where_math_truncate(bool isAsync) => null;

        public override void Query_backed_by_database_view()
        {
            // Not present on SQLite
        }

        public override async Task Take_Skip(bool isAsync)
        {
            await base.Take_Skip(isAsync);

            AssertSql(
                @"@__p_0='10' (DbType = String)
@__p_1='5' (DbType = String)

SELECT ""t"".*
FROM (
    SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
    FROM ""Customers"" AS ""c""
    ORDER BY ""c"".""ContactName""
    LIMIT @__p_0
) AS ""t""
ORDER BY ""t"".""ContactName""
LIMIT -1 OFFSET @__p_1");
        }

        public override async Task Where_datetime_now(bool isAsync)
        {
            await base.Where_datetime_now(isAsync);

            AssertSql(
                @"@__myDatetime_0='2015-04-10T00:00:00' (DbType = String)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime'), '0'), '.') <> @__myDatetime_0");
        }

        public override async Task Where_datetime_utcnow(bool isAsync)
        {
            await base.Where_datetime_utcnow(isAsync);

            AssertSql(
                @"@__myDatetime_0='2015-04-10T00:00:00' (DbType = String)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now'), '0'), '.') <> @__myDatetime_0");
        }

        public override async Task Where_datetime_today(bool isAsync)
        {
            await base.Where_datetime_today(isAsync);

            AssertSql(
                @"SELECT ""e"".""EmployeeID"", ""e"".""City"", ""e"".""Country"", ""e"".""FirstName"", ""e"".""ReportsTo"", ""e"".""Title""
FROM ""Employees"" AS ""e""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime', 'start of day'), '0'), '.') = rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime', 'start of day'), '0'), '.')");
        }

        public override async Task Where_datetime_date_component(bool isAsync)
        {
            await base.Where_datetime_date_component(isAsync);

            AssertSql(
                @"@__myDatetime_0='1998-05-04T00:00:00' (DbType = String)

SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", 'start of day'), '0'), '.') = @__myDatetime_0");
        }

        public override async Task Where_datetime_year_component(bool isAsync)
        {
            await base.Where_datetime_year_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%Y', ""o"".""OrderDate"") AS INTEGER) = 1998");
        }

        public override async Task Where_datetime_month_component(bool isAsync)
        {
            await base.Where_datetime_month_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%m', ""o"".""OrderDate"") AS INTEGER) = 4");
        }

        public override async Task Where_datetime_dayOfYear_component(bool isAsync)
        {
            await base.Where_datetime_dayOfYear_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%j', ""o"".""OrderDate"") AS INTEGER) = 68");
        }

        public override async Task Where_datetime_day_component(bool isAsync)
        {
            await base.Where_datetime_day_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%d', ""o"".""OrderDate"") AS INTEGER) = 4");
        }

        public override async Task Where_datetime_hour_component(bool isAsync)
        {
            await base.Where_datetime_hour_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%H', ""o"".""OrderDate"") AS INTEGER) = 14");
        }

        public override async Task Where_datetime_minute_component(bool isAsync)
        {
            await base.Where_datetime_minute_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%M', ""o"".""OrderDate"") AS INTEGER) = 23");
        }

        public override async Task Where_datetime_second_component(bool isAsync)
        {
            await base.Where_datetime_second_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%S', ""o"".""OrderDate"") AS INTEGER) = 44");
        }

        [ConditionalTheory(Skip = "Issue#15586")]
        public override async Task Where_datetime_millisecond_component(bool isAsync)
        {
            await base.Where_datetime_millisecond_component(isAsync);

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) = 88");
        }

        public override async Task String_StartsWith_Literal(bool isAsync)
        {
            await base.String_StartsWith_Literal(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""ContactName"" LIKE 'M%'");
        }

        public override async Task String_StartsWith_Identity(bool isAsync)
        {
            await base.String_StartsWith_Identity(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (""c"".""ContactName"" LIKE ""c"".""ContactName"" || '%' AND (substr(""c"".""ContactName"", 1, length(""c"".""ContactName"")) = ""c"".""ContactName"")) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_StartsWith_Column(bool isAsync)
        {
            await base.String_StartsWith_Column(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (""c"".""ContactName"" LIKE ""c"".""ContactName"" || '%' AND (substr(""c"".""ContactName"", 1, length(""c"".""ContactName"")) = ""c"".""ContactName"")) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_StartsWith_MethodCall(bool isAsync)
        {
            await base.String_StartsWith_MethodCall(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""ContactName"" LIKE 'M%'");
        }

        public override async Task String_EndsWith_Literal(bool isAsync)
        {
            await base.String_EndsWith_Literal(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""ContactName"" LIKE '%b'");
        }

        public override async Task String_EndsWith_Identity(bool isAsync)
        {
            await base.String_EndsWith_Identity(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (substr(""c"".""ContactName"", -length(""c"".""ContactName"")) = ""c"".""ContactName"") OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_EndsWith_Column(bool isAsync)
        {
            await base.String_EndsWith_Column(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (substr(""c"".""ContactName"", -length(""c"".""ContactName"")) = ""c"".""ContactName"") OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_EndsWith_MethodCall(bool isAsync)
        {
            await base.String_EndsWith_MethodCall(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""ContactName"" LIKE '%m'");
        }

        public override async Task String_Contains_Literal(bool isAsync)
        {
            await base.String_Contains_Literal(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE instr(""c"".""ContactName"", 'M') > 0");
        }

        public override async Task String_Contains_Identity(bool isAsync)
        {
            await base.String_Contains_Identity(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""ContactName"", ""c"".""ContactName"") > 0) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_Contains_Column(bool isAsync)
        {
            await base.String_Contains_Column(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""ContactName"", ""c"".""ContactName"") > 0) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_Contains_MethodCall(bool isAsync)
        {
            await base.String_Contains_MethodCall(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE instr(""c"".""ContactName"", 'M') > 0");
        }

        public override async Task IsNullOrWhiteSpace_in_predicate(bool isAsync)
        {
            await base.IsNullOrWhiteSpace_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""Region"" IS NULL OR (trim(""c"".""Region"") = '')");
        }

        public override async Task Where_string_length(bool isAsync)
        {
            await base.Where_string_length(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE length(""c"".""City"") = 6");
        }

        public override async Task Where_string_indexof(bool isAsync)
        {
            await base.Where_string_indexof(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ((instr(""c"".""City"", 'Sea') - 1) <> -1) OR ""c"".""City"" IS NULL");
        }

        public override async Task Indexof_with_emptystring(bool isAsync)
        {
            await base.Indexof_with_emptystring(isAsync);

            AssertSql(
                @"SELECT instr(""c"".""ContactName"", '') - 1
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_string_replace(bool isAsync)
        {
            await base.Where_string_replace(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE replace(""c"".""City"", 'Sea', 'Rea') = 'Reattle'");
        }

        public override async Task Replace_with_emptystring(bool isAsync)
        {
            await base.Replace_with_emptystring(isAsync);

            AssertSql(
                @"SELECT replace(""c"".""ContactName"", 'ari', '')
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_string_substring(bool isAsync)
        {
            await base.Where_string_substring(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE substr(""c"".""City"", 2, 2) = 'ea'");
        }

        public override async Task Substring_with_zero_startindex(bool isAsync)
        {
            await base.Substring_with_zero_startindex(isAsync);

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 1, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Substring_with_constant(bool isAsync)
        {
            await base.Substring_with_constant(isAsync);

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 2, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Substring_with_closure(bool isAsync)
        {
            await base.Substring_with_closure(isAsync);

            AssertSql(
                @"@__start_0='2' (DbType = String)

SELECT substr(""c"".""ContactName"", @__start_0 + 1, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Substring_with_Index_of(bool isAsync)
        {
            await base.Substring_with_Index_of(isAsync);

            AssertSql(
                @"SELECT ""c"".""ContactName""
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Substring_with_zero_length(bool isAsync)
        {
            await base.Substring_with_zero_length(isAsync);

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 3, 0)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_math_abs1(bool isAsync)
        {
            await base.Where_math_abs1(isAsync);

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE abs(""od"".""ProductID"") > 10");
        }

        public override async Task Where_math_abs2(bool isAsync)
        {
            await base.Where_math_abs2(isAsync);

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE abs(""od"".""Quantity"") > 10");
        }

        public override async Task Where_math_abs_uncorrelated(bool isAsync)
        {
            await base.Where_math_abs_uncorrelated(isAsync);

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE 10 < ""od"".""ProductID""");
        }

        public override async Task Select_math_round_int(bool isAsync)
        {
            await base.Select_math_round_int(isAsync);

            AssertSql(
                @"SELECT round(""o"".""OrderID"") AS ""A""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderID"" < 10250");
        }

        public override async Task Where_math_min(bool isAsync)
        {
            await base.Where_math_min(isAsync);

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE (""od"".""OrderID"" = 11077) AND (min(""od"".""OrderID"", ""od"".""ProductID"") = ""od"".""ProductID"")");
        }

        public override async Task Where_math_max(bool isAsync)
        {
            await base.Where_math_max(isAsync);

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE (""od"".""OrderID"" = 11077) AND (max(""od"".""OrderID"", ""od"".""ProductID"") = ""od"".""OrderID"")");
        }

        public override async Task Where_string_to_lower(bool isAsync)
        {
            await base.Where_string_to_lower(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE lower(""c"".""CustomerID"") = 'alfki'");
        }

        public override async Task Where_string_to_upper(bool isAsync)
        {
            await base.Where_string_to_upper(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE upper(""c"".""CustomerID"") = 'ALFKI'");
        }

        public override async Task TrimStart_without_arguments_in_predicate(bool isAsync)
        {
            await base.TrimStart_without_arguments_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task TrimStart_with_char_argument_in_predicate(bool isAsync)
        {
            await base.TrimStart_with_char_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"", 'O') = 'wner'");
        }

        public override async Task TrimStart_with_char_array_argument_in_predicate(bool isAsync)
        {
            await base.TrimStart_with_char_array_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"", 'Ow') = 'ner'");
        }

        public override async Task TrimEnd_without_arguments_in_predicate(bool isAsync)
        {
            await base.TrimEnd_without_arguments_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task TrimEnd_with_char_argument_in_predicate(bool isAsync)
        {
            await base.TrimEnd_with_char_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"", 'r') = 'Owne'");
        }

        public override async Task TrimEnd_with_char_array_argument_in_predicate(bool isAsync)
        {
            await base.TrimEnd_with_char_array_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"", 'er') = 'Own'");
        }

        public override async Task Trim_without_argument_in_predicate(bool isAsync)
        {
            await base.Trim_without_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task Trim_with_char_argument_in_predicate(bool isAsync)
        {
            await base.Trim_with_char_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"", 'O') = 'wner'");
        }

        public override async Task Trim_with_char_array_argument_in_predicate(bool isAsync)
        {
            await base.Trim_with_char_array_argument_in_predicate(isAsync);

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"", 'Or') = 'wne'");
        }

        // Skip for SQLite. Issue #14935. Cannot eval 'Sum()'
        public override Task Sum_with_coalesce(bool isAsync) => null;

        public override async Task Select_datetime_year_component(bool isAsync)
        {
            await base.Select_datetime_year_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%Y', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_datetime_year_component_composed(bool isAsync)
        {
            await AssertQueryScalar<Order>(
                isAsync,
                os => os.Select(o => o.OrderDate.Value.AddYears(1).Year));

            AssertSql(
                @"SELECT CAST(strftime('%Y', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' years') AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_month_component(bool isAsync)
        {
            await base.Select_datetime_month_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%m', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_day_of_year_component(bool isAsync)
        {
            await base.Select_datetime_day_of_year_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%j', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_day_component(bool isAsync)
        {
            await base.Select_datetime_day_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%d', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_hour_component(bool isAsync)
        {
            await base.Select_datetime_hour_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%H', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_minute_component(bool isAsync)
        {
            await base.Select_datetime_minute_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%M', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_second_component(bool isAsync)
        {
            await base.Select_datetime_second_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%S', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_millisecond_component(bool isAsync)
        {
            await base.Select_datetime_millisecond_component(isAsync);

            AssertSql(
                @"SELECT (CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000
FROM ""Orders"" AS ""o""");
        }

        [ConditionalTheory(Skip = "QueryIssue")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_datetime_millisecond_component_composed(bool isAsync)
        {
            await AssertQueryScalar<Order>(
                isAsync,
                os => os.Select(o => o.OrderDate.Value.AddYears(1).Millisecond));

            AssertSql(
                @"SELECT (CAST(strftime('%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' years') AS REAL) * 1000) % 1000
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_DayOfWeek_component(bool isAsync)
        {
            await base.Select_datetime_DayOfWeek_component(isAsync);

            AssertSql(
                @"SELECT CAST(strftime('%w', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_Ticks_component(bool isAsync)
        {
            await base.Select_datetime_Ticks_component(isAsync);

            AssertSql(
                @"SELECT CAST((julianday(""o"".""OrderDate"") - 1721425.5) * 864000000000 AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_TimeOfDay_component(bool isAsync)
        {
            await base.Select_datetime_TimeOfDay_component(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%H:%M:%f', ""o"".""OrderDate""), '0'), '.')
FROM ""Orders"" AS ""o""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_datetime_TimeOfDay_component_composed(bool isAsync)
        {
            await AssertQueryScalar<Order>(
                isAsync,
                os => os.Select(o => o.OrderDate.Value.AddYears(1).TimeOfDay));

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%H:%M:%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' years'), '0'), '.')
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_expression_date_add_year(bool isAsync)
        {
            await base.Select_expression_date_add_year(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' years'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_month(bool isAsync)
        {
            await base.Select_expression_datetime_add_month(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' months'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_hour(bool isAsync)
        {
            await base.Select_expression_datetime_add_hour(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' hours'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_minute(bool isAsync)
        {
            await base.Select_expression_datetime_add_minute(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' minutes'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_second(bool isAsync)
        {
            await base.Select_expression_datetime_add_second(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_ticks(bool isAsync)
        {
            await base.Select_expression_datetime_add_ticks(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(10000 / 10000000.0 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_above_the_range(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_above_the_range(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1000000000000.0 / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_below_the_range(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_below_the_range(isAsync);

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(-1000000000000.0 / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_large_number_divided(bool isAsync)
        {
            await base.Select_expression_date_add_milliseconds_large_number_divided(isAsync);

            AssertSql(
                @"@__millisecondsPerDay_0='86400000' (DbType = String)

SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) / @__millisecondsPerDay_0 AS TEXT) || ' days', CAST((((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) % @__millisecondsPerDay_0) / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
