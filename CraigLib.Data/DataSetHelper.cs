using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CraigLib.Data
{
    public static class DataSetHelper
    {

        public static void AttachDbError(DataSet ds, Exception ex)
        {
            var exception = ex as DatabaseAdapterException;
            if (exception != null)
            {
                var adapterException = exception;
                AttachDbError(ds, adapterException.ErrorMessage, adapterException.CommandText);
            }
            else
            {
                if (ex == null)
                    return;
                AttachDbError(ds, ex.Message, string.Empty);
            }
        }

        public static void AttachDbError(DataSet ds, string errorMessage, string commandText)
        {
            if (ds == null)
                return;
            var dataTable = ds.Tables["DbError"];
            if (dataTable == null)
            {
                dataTable = ds.Tables.Add("DbError");
                dataTable.Columns.Add("ErrorMessage", typeof(string));
                dataTable.Columns.Add("CommandText", typeof(string));
            }
            if (errorMessage.Length <= 0 && commandText.Length <= 0)
                return;
            dataTable.Rows.Add((object)errorMessage, (object)commandText);
        }

        public static void AttachSqlLog(DataSet ds, string sql, string log)
        {
            if (ds == null || !IsSqlTracing(ds))
                return;
            var dataTable = ds.Tables["SqlLog"];
            if (dataTable == null)
            {
                dataTable = ds.Tables.Add("SqlLog");
                dataTable.Columns.Add("Sql", typeof(string));
                dataTable.Columns.Add("Log", typeof(string));
            }
            var dataRow = (DataRow)null;
            if (dataTable.Rows.Count > 0)
                dataRow = dataTable.Rows[dataTable.Rows.Count - 1];
            if (dataRow != null && log.Length > 0 && sql == (string)dataRow["Sql"])
            {
                dataRow["Log"] = log;
            }
            else
            {
                if (log.Length <= 0 && sql.Length <= 0)
                    return;
                dataTable.Rows.Add((object)sql, (object)log);
            }
        }
        public static int ClearDeleteSqls(this DataTable dt)
        {
            var deleteSqlCount = GetDeleteSqlCount(dt);
            if (deleteSqlCount <= 0)
                return 0;
            for (var index = 0; index < deleteSqlCount; ++index)
            {
                if (dt.ExtendedProperties.Contains("DeleteSql" + index))
                    dt.ExtendedProperties.Remove("DeleteSql" + index);
            }
            if (dt.ExtendedProperties.Contains("DeleteSqlCount"))
                dt.ExtendedProperties.Remove("DeleteSqlCount");
            return deleteSqlCount;
        }

        public static string[] GetDeleteSqls(this DataTable dt)
        {
            var list = new List<string>();
            var deleteSqlCount = GetDeleteSqlCount(dt);
            for (var index = 0; index < deleteSqlCount; ++index)
            {
                if (dt.ExtendedProperties.Contains("DeleteSql" + index))
                    list.Add(dt.ExtendedProperties["DeleteSql" + index] as string);
            }
            return list.ToArray();
        }

        public static string GetJoinSql(this DataRelation relation)
        {
            var str1 = string.Empty;
            var list = relation.ChildTable.ParentRelations.Cast<DataRelation>().Where(dataRelation => dataRelation.ParentTable == relation.ParentTable).ToList();
            foreach (var dataRelation in list)
            {
                var str2 = string.Empty;
                for (var index = 0; index < dataRelation.ParentColumns.Length; ++index)
                {
                    var dc1 = dataRelation.ParentColumns[index];
                    var str3 = GetDbTable(dc1);
                    if (str3.Length == 0)
                        str3 = GetDbTable(dc1.Table);
                    if (str3.Length == 0)
                        str3 = dc1.Table.TableName;
                    var str4 = GetDbColumn(dc1);
                    if (str4.Length == 0)
                        str4 = dc1.ColumnName;
                    var dc2 = dataRelation.ChildColumns[index];
                    var str5 = GetDbTable(dc2);
                    if (str5.Length == 0)
                        str5 = GetDbTable(dc2.Table);
                    if (str5.Length == 0)
                        str5 = dc2.Table.TableName;
                    var str6 = GetDbColumn(dc2);
                    if (str6.Length == 0)
                        str6 = dc2.ColumnName;
                    if (str2.Length > 0)
                        str2 = str2 + " AND ";
                    str2 = str2 + str3 + "." + str4 + "=" + str5 + "." + str6;
                }
                if (str2.Length > 0)
                {
                    if (str2.IndexOf(" AND ", StringComparison.Ordinal) > 0)
                        str2 = "(" + str2 + ")";
                    if (str1.Length > 0)
                        str1 = str1 + " OR ";
                    str1 = str1 + str2;
                }
            }
            if (str1.IndexOf(" OR ", StringComparison.Ordinal) > 0)
                str1 = "(" + str1 + ")";
            return str1;
        }

        public static bool IsDynamicColumn(this DataColumn dc)
        {
            var flag = dc.ColumnName.LastIndexOf("-", StringComparison.Ordinal) > 0;
            return flag;
        }

        public static bool IsConcurrentCheck(this DataColumn dc)
        {
            var flag = false;
            if (dc.ExtendedProperties.Contains("concurrentcheck"))
                flag = bool.Parse((string)dc.ExtendedProperties["concurrentcheck"]);
            return flag;
        }

        public static List<string> GetConcurrentCheckColumns(this DataTable dt)
        {
            List<string> list = null;
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsConcurrentCheck(dc))
                {
                    if (list == null)
                        list = new List<string>();
                    list.Add(GetDbColumn(dc));
                }
            }
            return list;
        }

        public static bool IsNumericColumn(this DataColumn dc)
        {
            return Expr.IsNumericDataType(dc.DataType);
        }

        public static string GetDbColumn(this DataColumn dc)
        {
            return !dc.ExtendedProperties.Contains("dbcolumn") ? dc.ColumnName.ToLower() : (string)dc.ExtendedProperties["dbcolumn"];
        }

        public static string GetDbColumnOrDefault(this DataColumn dc)
        {
            var str = GetDbColumn(dc);
            if (string.IsNullOrEmpty(str))
                str = dc.ColumnName.ToLower();
            return str;
        }

        public static string GetDbCrosstab(this DataColumn dc)
        {
            var str = "";
            if (dc.ExtendedProperties.Contains("dbcrosstab"))
                str = (string)dc.ExtendedProperties["dbcrosstab"];
            return str;
        }

        public static string GetDbExpr(this DataColumn dc)
        {
            var str = "";
            if (dc.ExtendedProperties.Contains("dbexpr"))
                str = (string)dc.ExtendedProperties["dbexpr"];
            return str;
        }

        public static string GetDbTable(this DataTable dt)
        {
            return !dt.ExtendedProperties.Contains("dbtable") ? dt.TableName.ToLower() : (string)dt.ExtendedProperties["dbtable"];
        }

        public static void SetDbTable(this DataTable dt, string dbtable)
        {
            dt.ExtendedProperties["dbtable"] = dbtable;
        }

        public static string GetDbTable(this DataColumn dc)
        {
            var str = "";
            if (dc.ExtendedProperties.Contains("dbtable"))
                str = (string)dc.ExtendedProperties["dbtable"];
            else if (dc.Table != null)
                str = GetDbTable(dc.Table);
            return str;
        }

        public static string GetDbTableOrDefault(this DataColumn dc)
        {
            var str = "";
            if (dc.ExtendedProperties.Contains("dbtable"))
                str = (string)dc.ExtendedProperties["dbtable"];
            if (string.IsNullOrEmpty(str) && dc.Table != null)
            {
                str = GetDbTable(dc.Table);
                if (string.IsNullOrEmpty(str))
                    str = dc.Table.TableName.ToLower();
            }
            return str;
        }

        public static int GetPageNumber(this DataTable dt)
        {
            var num = 0;
            if (dt.ExtendedProperties.Contains("PageNumber"))
                num = int.Parse((string)dt.ExtendedProperties["PageNumber"]);
            return num;
        }

        public static string GetSelectSql(this DataTable dt)
        {
            var str = "";
            if (dt.ExtendedProperties.Contains("SelectSql"))
                str = (string)dt.ExtendedProperties["SelectSql"];
            return str;
        }

        public static void SetSelectSql(this DataTable dt, string selectSql)
        {
            if (string.IsNullOrEmpty(selectSql))
                dt.ExtendedProperties.Remove("SelectSql");
            else
                dt.ExtendedProperties["SelectSql"] = selectSql;
        }

        public static int GetPageSize(this DataTable dt)
        {
            var num = 0;
            if (dt.ExtendedProperties.Contains("PageSize"))
                num = int.Parse((string)dt.ExtendedProperties["PageSize"]);
            return num;
        }

        public static string GetDbWhere(this DataTable dt)
        {
            var str = "";
            if (dt.ExtendedProperties.Contains("dbwhere"))
                str = (string)dt.ExtendedProperties["dbwhere"];
            return str;
        }

        public static void SetDbWhere(this DataTable dt, string dbwhere)
        {
            dt.ExtendedProperties["dbwhere"] = dbwhere;
        }

        public static int GetRowsLimit(this DataTable dt)
        {
            var num = 0;
            if (dt.ExtendedProperties.Contains("RowsLimit"))
                num = int.Parse((string)dt.ExtendedProperties["RowsLimit"]);
            return num;
        }

        public static void SetRowsLimit(this DataTable dt, int rowsLimit)
        {
            if (rowsLimit <= 0)
                dt.ExtendedProperties.Remove("RowsLimit");
            else
                dt.ExtendedProperties["RowsLimit"] = rowsLimit.ToString(CultureInfo.InvariantCulture);
        }

        public static object GetDefaultValue(string defaultString, Type dataType)
        {
            object obj = DBNull.Value;
            if (defaultString != null)
            {
                if (defaultString.Length != 0)
                {

                    defaultString = defaultString.Trim();
                    obj = defaultString != "<null>"
                        ? (!(dataType == typeof (bool))
                            ? (!(dataType == typeof (DateTime))
                                ? (!(dataType == typeof (int))
                                    ? (!(dataType == typeof (long))
                                        ? (!(dataType == typeof (Decimal))
                                            ? (!(dataType == typeof (double))
                                                ? (!(dataType == typeof (string))
                                                    ? Convert.ChangeType(defaultString, dataType)
                                                    : defaultString)
                                                : Convert.ToDouble(defaultString))
                                            : Convert.ToDecimal(defaultString))
                                        : Convert.ToInt64(defaultString))
                                    : Convert.ToInt32(defaultString))
                                : (defaultString.ToLower() != "now"
                                    ? (defaultString.ToLower() != "today"
                                        ? Convert.ToDateTime(defaultString)
                                        : (object) DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified))
                                    : DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified)))
                            : (defaultString != "0" && (defaultString == "1" || (bool.Parse(defaultString)))))
                        : DBNull.Value;

                    return obj;
                }
            }
            return obj;
        }

        public static void SetDbGroupBy(this DataColumn dc, bool groupBy)
        {
            if (groupBy)
                dc.ExtendedProperties["dbgroupby"] = bool.TrueString;
            else
                dc.ExtendedProperties.Remove("dbgroupby");
        }

        public static bool IsProxy(this DataColumn dc)
        {
            var str1 = dc.ExtendedProperties["proxypane"] as string;
            var str2 = dc.ExtendedProperties["proxycolumn"] as string;
            if (!string.IsNullOrEmpty(str1))
                return !string.IsNullOrEmpty(str2);
            return false;
        }

        public static void JoinByMatchColumnsOnly(this DataTable dt, bool byMatchColumnsOnly)
        {
            if (byMatchColumnsOnly)
                dt.ExtendedProperties["JoinByMatchColumnsOnly"] = bool.TrueString;
            else
                dt.ExtendedProperties.Remove("JoinByMatchColumnsOnly");
        }

        public static bool JoinByMatchColumnsOnly(this DataTable dt)
        {
            var flag = false;
            if (dt.ExtendedProperties.Contains("JoinByMatchColumnsOnly"))
                flag = bool.Parse((string)dt.ExtendedProperties["JoinByMatchColumnsOnly"]);
            return flag;
        }

        public static bool IsSelectDistinct(this DataTable dt)
        {
            var flag = false;
            if (dt.ExtendedProperties.Contains("selectdistinct"))
                flag = bool.Parse((string)dt.ExtendedProperties["selectdistinct"]);
            return flag;
        }

        public static bool IsDbGroupBy(this DataColumn dc)
        {
            var flag = false;
            if (dc.ExtendedProperties.Contains("dbgroupby"))
                flag = bool.Parse((string)dc.ExtendedProperties["dbgroupby"]);
            return flag;
        }

        public static void SetPageSize(this DataTable dt, int pageSize)
        {
            if (pageSize <= 0)
                dt.ExtendedProperties.Remove("PageSize");
            else
                dt.ExtendedProperties["PageSize"] = pageSize.ToString(CultureInfo.InvariantCulture);
        }

        public static void SetPageNumber(this DataTable dt, int pageNumber)
        {
            if (pageNumber <= 0)
                dt.ExtendedProperties.Remove("PageNumber");
            else
                dt.ExtendedProperties["PageNumber"] = pageNumber.ToString(CultureInfo.InvariantCulture);
        }

        public static void SetDbTable(this DataColumn dc, string dbtable)
        {
            dc.ExtendedProperties["dbtable"] = dbtable;
        }

        public static int GetDeleteSqlCount(this DataTable dt)
        {
            var num = 0;
            if (dt.ExtendedProperties.Contains("DeleteSqlCount"))
                num = int.Parse((string)dt.ExtendedProperties["DeleteSqlCount"]);
            return num;
        }
        public static bool IsSqlTracing(DataSet ds)
        {
            var flag = false;
            if (ds.ExtendedProperties.Contains("SqlTracing"))
                flag = bool.Parse((string)ds.ExtendedProperties["SqlTracing"]);
            return flag;
        }

        public static string SelectCriteriaToString(SelectCriteria criteria)
        {
            string str;
            if (criteria.DateColumn == "timerange")
                str = "begtime < '" + criteria.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and endtime > '" + criteria.BegTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            else
                str = criteria.DateColumn + " >= '" + criteria.BegTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and " + criteria.DateColumn + " < '" + criteria.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            return criteria.DatabaseCriteria.Aggregate(str, (current, dbCriteria) => current + " " + dbCriteria.LogOperator + " " + dbCriteria.OpenParen + dbCriteria.DbTable + (string.IsNullOrEmpty(dbCriteria.DbTable) ? "" : ".") + dbCriteria.DbColumn + dbCriteria.RelOperator + "'" + dbCriteria.DbValue + "'" + dbCriteria.CloseParen);
        }

        public static string GetColumnByPartialName(this DataTable dt, string colname)
        {
            if (dt.ExtendedProperties.ContainsKey("nopartialcolname"))
            {
                if (dt.Columns.Contains(colname))
                    return colname;
                return "";
            }
            var str = "_" + colname;
            foreach (DataColumn dataColumn in dt.Columns)
            {
                if (dataColumn.ColumnName.Equals(colname, StringComparison.OrdinalIgnoreCase) || dataColumn.ColumnName.EndsWith(str, StringComparison.OrdinalIgnoreCase))
                    return dataColumn.ColumnName;
            }
            return "";
        }

        public static Type SetDataType(this DataColumn dc, DatabaseSchema.DbColumnRow dbColumnRow)
        {
            var type = typeof(string);
            if (dbColumnRow != null)
            {
                type = GetDataType(dbColumnRow);
                if (dc.DataType != type)
                {
                    var table = dc.Table;
                    if (table != null)
                        table.Columns.Remove(dc);
                    dc.DataType = type;
                    if (table != null)
                        table.Columns.Add(dc);
                }
            }
            return type;
        }

        public static Type GetDataType(DatabaseSchema.DbColumnRow dbColumnRow)
        {
            var type = typeof(string);
            if (dbColumnRow.DbType.IndexOf("date", StringComparison.Ordinal) >= 0)
                type = typeof(DateTime);
            else if (dbColumnRow.DbType.IndexOf("bool", StringComparison.Ordinal) >= 0)
                type = typeof(bool);
            else if (dbColumnRow.DbType.IndexOf("image", StringComparison.Ordinal) >= 0)
                type = typeof(byte[]);
            else if (dbColumnRow.DbType.IndexOf("number", StringComparison.Ordinal) >= 0)
                type = typeof(Decimal);
            else if (dbColumnRow.DbType.StartsWith("int"))
                type = typeof(int);
            return type;
        }

        //public static Type SetDataType(this DataColumn dc, MetaDataDS.viewcolumnRow viewcolumnRow)
        //{
        //    Type type = DsHelper.ExpectedColumnDataType(viewcolumnRow);
        //    if (type != (Type)null)
        //        dc.DataType = type;
        //    return type;
        //}

        public static string SelectCriteriaToString(SelectCriteria criteria, DataTable dt)
        {
            var stringBuilder = new StringBuilder();
            if (criteria.DateColumn == "timerange")
            {
                if (dt.Columns.Contains("begtime") && dt.Columns.Contains("endtime"))
                    stringBuilder.AppendFormat("((begtime < '{0}' or begtime is null) and (endtime > '{1}' or endtime is null))", criteria.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), criteria.BegTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (dt.Columns.Contains(criteria.DateColumn))
                stringBuilder.Append(criteria.DateColumn + " >= '" + criteria.BegTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and " + criteria.DateColumn + " < '" + criteria.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            foreach (var dbCriteria in criteria.DatabaseCriteria)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(" " + dbCriteria.LogOperator + " ");
                if (!dt.TableName.Equals(dbCriteria.DbTable, StringComparison.OrdinalIgnoreCase))
                {
                    stringBuilder.Append(dbCriteria.OpenParen + " 1=1 " + dbCriteria.CloseParen);
                    continue;
                }
                var columnByPartialName = GetColumnByPartialName(dt, dbCriteria.DbColumn);
                if (columnByPartialName.Length > 0)
                {
                    if (dbCriteria.RelOperator.Equals("in", StringComparison.OrdinalIgnoreCase))
                    {
                        if (dbCriteria.DbValue.IndexOf("select", StringComparison.OrdinalIgnoreCase) >= 0 && dbCriteria.DbValue.IndexOf("where", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            stringBuilder.Append(dbCriteria.OpenParen + " 1=1 " + dbCriteria.CloseParen);
                        }
                        else
                        {
                            stringBuilder.Append(dbCriteria.OpenParen + columnByPartialName + " in (");
                            var str1 = dbCriteria.DbValue;
                            var chArray = new[]
              {
                ','
              };
                            foreach (var str2 in str1.Split(chArray))
                                stringBuilder.Append("'" + str2 + "',");
                            stringBuilder[stringBuilder.Length - 1] = ')';
                            stringBuilder.Append(dbCriteria.CloseParen);
                        }
                    }
                    else
                        stringBuilder.Append(dbCriteria.OpenParen + columnByPartialName + " " + dbCriteria.RelOperator + " '" + dbCriteria.DbValue + "'" + dbCriteria.CloseParen);
                }
                else
                    stringBuilder.Append(dbCriteria.OpenParen + " 1=1 " + dbCriteria.CloseParen);
            }
            return stringBuilder.ToString();
        }

       public static DataTable[] SortDataTables(DataSet dataSet, bool descending)
        {
            var list = new List<DataTable>();
            var maxTableLevel = GetMaxTableLevel(dataSet);
            for (var level = 0; level <= maxTableLevel; ++level)
            {
                var tablesFromLevel = GetTablesFromLevel(dataSet, level);
                list.AddRange(tablesFromLevel);
            }
            if (descending)
                list.Reverse();
            return list.ToArray();
        }

        public static DataTable[] SortTables(this DataSet ds)
        {
            return SortTables(ds, false);
        }

        public static DataTable[] SortTables(this DataSet ds, bool descending)
        {
            return SortTables(ds, descending, null);
        }

        public static DataTable[] SortTables(this DataSet ds, List<string> tableNames)
        {
            return SortTables(ds, false, tableNames);
        }

        public static DataTable[] SortTables(this DataSet ds, bool descending, List<string> tableNames)
        {
            var list = new List<DataTable>();
            var maxTableLevel = GetMaxTableLevel(ds);
            for (var level = 0; level <= maxTableLevel; ++level)
            {
                var tablesFromLevel = GetTablesFromLevel(ds, level);
                if (tableNames != null)
                {
                    list.AddRange(tablesFromLevel.Where(dataTable => tableNames.Contains(dataTable.TableName)));
                }
                else
                    list.AddRange(tablesFromLevel);
            }
            if (descending)
                list.Reverse();
            return list.ToArray();
        }
        public static int GetMaxTableLevel(DataSet dataSet)
        {
            return (from DataTable dt in dataSet.Tables select GetTableLevel(dt)).Concat(new[] {0}).Max();
        }

        public static int GetTableLevel(DataTable dataTable)
        {
            return (from DataRelation relation in dataTable.ParentRelations where !IsRecursive(relation) select GetTableLevel(relation.ParentTable) into tableLevel select tableLevel + 1).Concat(new[] {0}).Max();
        }

        public static DataTable[] GetTablesFromLevel(this DataSet dataSet, int level)
        {
            return dataSet.Tables.Cast<DataTable>().Where(dt => GetTableLevel(dt) == level).ToArray();
        }

        public static bool IsRecursive(DataTable dataTable)
        {
            return dataTable.ChildRelations.Cast<DataRelation>().Any(rel => IsRecursive(rel));
        }

        public static bool IsRecursive(this DataRelation relation)
        {
            return relation.ParentTable == relation.ChildTable;
        }

        public static void SetDateTimeKind(this DataSet ds)
        {
            if (ds == null)
                return;
            foreach (DataTable dt in ds.Tables)
                SetDateTimeKind(dt);
        }

        public static void SetDateTimeKind(this DataTable dt)
        {
            if (dt == null)
                return;
            foreach (var dataColumn in dt.Columns.Cast<DataColumn>().Where(dataColumn => dataColumn.DataType == typeof(DateTime) && dataColumn.DateTimeMode != DataSetDateTime.Unspecified))
            {
                dataColumn.DateTimeMode = DataSetDateTime.Unspecified;
            }
        }
    }
}
