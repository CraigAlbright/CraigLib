using System;
using System.Collections.Generic;
using System.Text;

namespace CraigLib.Data
{
    public class QueryBuilder
    {
        public List<string> TableList = new List<string>();
        public Dictionary<string, string> JoinList = new Dictionary<string, string>();
        public List<string> WhereList = new List<string>();
        public List<string> GroupList = new List<string>();
        public List<string> HavingList = new List<string>();
        public List<string> OrderList = new List<string>();
        private string _nonQuery = string.Empty;
        private string _select = string.Empty;
        private List<string> _columnList = new List<string>();
        private bool _selectDistinct;
        private int _rowsLimit;

        public string Query
        {
            get
            {
                if (_nonQuery.Length > 0)
                    return _nonQuery;
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(Select).Append(From).Append(Where).Append(Group).Append(Having).Append(Order);
                return stringBuilder.ToString();
            }
            set
            {
                _nonQuery = string.Empty;
                _select = string.Empty;
                _columnList.Clear();
                TableList.Clear();
                JoinList.Clear();
                WhereList.Clear();
                GroupList.Clear();
                HavingList.Clear();
                OrderList.Clear();
                var selectSql = value;
                if (selectSql.ToUpper().IndexOf("SELECT ", System.StringComparison.Ordinal) >= 0)
                {
                    var select = "";
                    var from = "";
                    var where = "";
                    var groupBy = "";
                    var having = "";
                    var orderBy = "";
                    Expr.ParseSelectSql(selectSql, ref select, ref from, ref where, ref groupBy, ref having, ref orderBy);
                    if (orderBy.Length > 0)
                        Order = orderBy;
                    if (having.Length > 0)
                        Having = having;
                    if (groupBy.Length > 0)
                        Group = groupBy;
                    if (where.Length > 0)
                        Where = where;
                    if (from.Length > 0)
                        From = from;
                    if (select.Length <= 0)
                        return;
                    Select = select;
                }
                else
                    _nonQuery = value;
            }
        }

        public string Select
        {
            get
            {
                if (_columnList.Count <= 0)
                    return _select;
                var str1 = string.Empty;
                var str2 = "SELECT ";
                if (_selectDistinct)
                    str2 = str2 + "DISTINCT ";
                if (_rowsLimit > 0 && DatabaseHelper.DbSyntax == DatabaseType.MSSQL)
                    str2 = string.Concat(new object[4]
          {
            str2,
            "TOP ",
            _rowsLimit,
            " "
          });
                return str2 + string.Join(", ", _columnList.ToArray());
            }
            set
            {
                _select = value.Trim();
                _selectDistinct = _select.ToUpper().IndexOf(" DISTINCT ", System.StringComparison.Ordinal) > 0;
                _rowsLimit = 0;
                _columnList.Clear();
            }
        }

        public List<string> ColumnList
        {
            get
            {
                if (_select.Length > 0 && _columnList.Count == 0)
                {
                    var columnClause = _select;
                    if (columnClause.ToUpper().StartsWith("SELECT "))
                        columnClause = columnClause.Substring(7).Trim();
                    if (columnClause.ToUpper().StartsWith("DISTINCT "))
                    {
                        _selectDistinct = true;
                        columnClause = columnClause.Substring(9).Trim();
                    }
                    else
                        _selectDistinct = false;
                    _rowsLimit = 0;
                    if (columnClause == "*" && TableList.Count > 0)
                    {
                        _columnList.Clear();
                        foreach (var tablename in TableList)
                        {
                            foreach (DatabaseSchema.DbColumnRow dbColumnRow in DatabaseContent.GetDbColumns(tablename))
                                _columnList.Add(dbColumnRow.DbTable + "." + dbColumnRow.DbColumn);
                        }
                        if (_columnList.Count == 0)
                            _columnList.Add("*");
                    }
                    else
                        Expr.ParseColumnClause(columnClause, _columnList);
                }
                return _columnList;
            }
            set
            {
                _columnList = value;
            }
        }

        public bool SelectDistinct
        {
            get
            {
                return _selectDistinct;
            }
            set
            {
                var columnList = ColumnList;
                _selectDistinct = value;
            }
        }

        public int RowsLimit
        {
            get
            {
                return _rowsLimit;
            }
            set
            {
                var columnList = ColumnList;
                _rowsLimit = value;
            }
        }

        public string From
        {
            get
            {
                var str1 = string.Empty;
                if (TableList.Count > 0)
                {
                    foreach (var key in TableList)
                    {
                        var str2 = "";
                        if (JoinList.TryGetValue(key, out str2))
                        {
                            str1 = str1 + str2;
                        }
                        else
                        {
                            if (str1.Length > 0)
                                str1 = str1 + ", ";
                            str1 = str1 + key;
                        }
                    }
                    str1 = " FROM " + str1;
                }
                return str1;
            }
            set
            {
                var str1 = value.Trim();
                if (str1.ToUpper().StartsWith("FROM "))
                    str1 = str1.Substring(5).Trim();
                TableList.Clear();
                JoinList.Clear();
                var str2 = str1;
                var separator = new char[1]
        {
          ','
        };
                var num = 1;
                foreach (var str3 in str2.Split(separator, (StringSplitOptions)num))
                {
                    var from1 = str3.Trim();
                    var startIndex1 = 0;
                    var index = ParseJoinClause(from1, ref startIndex1);
                    if (startIndex1 >= 0)
                    {
                        TableList.Add(from1.Substring(0, startIndex1).Trim());
                        var from2 = from1.Substring(startIndex1);
                        while (true)
                        {
                            var startIndex2 = 1;
                            var str4 = ParseJoinClause(from2, ref startIndex2);
                            if (startIndex2 >= 0)
                            {
                                var str5 = from2.Substring(0, startIndex2);
                                TableList.Add(index);
                                JoinList[index] = str5;
                                from2 = from2.Substring(startIndex2);
                                index = str4;
                            }
                            else
                                break;
                        }
                        TableList.Add(index);
                        JoinList[index] = from2;
                    }
                    else
                        TableList.Add(from1);
                }
            }
        }

        public string Group
        {
            get
            {
                var str = string.Empty;
                if (GroupList.Count > 0)
                    str = " GROUP BY " + string.Join(", ", GroupList.ToArray());
                return str;
            }
            set
            {
                var columnClause = value.Trim();
                if (columnClause.ToUpper().StartsWith("GROUP BY "))
                    columnClause = columnClause.Substring(9).Trim();
                Expr.ParseColumnClause(columnClause, GroupList);
            }
        }

        public string Order
        {
            get
            {
                var str = string.Empty;
                if (OrderList.Count > 0)
                    str = " ORDER BY " + string.Join(", ", OrderList.ToArray());
                return str;
            }
            set
            {
                var columnClause = value.Trim();
                if (columnClause.ToUpper().StartsWith("ORDER BY "))
                    columnClause = columnClause.Substring(9).Trim();
                Expr.ParseColumnClause(columnClause, OrderList);
            }
        }

        public string Where
        {
            get
            {
                var stringBuilder = new StringBuilder();
                if (WhereList.Count > 0)
                {
                    var strArray = WhereList.ToArray();
                    stringBuilder.Append(" ");
                    strArray[0] = strArray[0].Trim();
                    if (!strArray[0].StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
                        stringBuilder.Append("WHERE ");
                    if (strArray[0].StartsWith("AND ", StringComparison.OrdinalIgnoreCase))
                        strArray[0] = strArray[0].Substring(4);
                    else if (strArray[0].StartsWith("OR ", StringComparison.OrdinalIgnoreCase))
                        strArray[0] = strArray[0].Substring(3);
                    stringBuilder.Append(string.Join(" ", strArray));
                    if (_rowsLimit > 0 && DatabaseHelper.DbSyntax == DatabaseType.ORACLE)
                        stringBuilder.Append(" AND ROWNUM < " + _rowsLimit);
                }
                return stringBuilder.ToString();
            }
            set
            {
                var str1 = value.Trim();
                if (str1.StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
                    str1 = str1.Substring(6).Trim();
                WhereList.Clear();
                int num;
                do
                {
                    str1 = str1.Trim();
                    num = -1;
                    if (str1.Length > 3)
                    {
                        num = str1.ToUpper().IndexOf(" AND ", 3);
                        if (num < 0)
                            num = str1.ToUpper().IndexOf(" OR ", 3);
                    }
                    var str2 = string.Empty;
                    string str3;
                    if (num >= 0)
                    {
                        str3 = str1.Substring(0, num).Trim();
                        str1 = str1.Substring(num);
                    }
                    else
                        str3 = str1.Trim();
                    if (str3.Length > 0)
                        WhereList.Add(str3);
                }
                while (num > 0);
            }
        }

        public string Having
        {
            get
            {
                var stringBuilder = new StringBuilder();
                if (HavingList.Count > 0)
                    stringBuilder.Append(" HAVING ").Append(string.Join(" ", HavingList.ToArray()));
                return stringBuilder.ToString();
            }
            set
            {
                var str1 = value.Trim();
                if (str1.ToUpper().StartsWith("HAVING "))
                    str1 = str1.Substring(7).Trim();
                HavingList.Clear();
                int num;
                do
                {
                    str1 = str1.Trim();
                    num = -1;
                    if (str1.Length > 3)
                    {
                        num = str1.ToUpper().IndexOf(" AND ", 3);
                        if (num < 0)
                            num = str1.ToUpper().IndexOf(" OR ", 3);
                    }
                    var str2 = string.Empty;
                    string str3;
                    if (num >= 0)
                    {
                        str3 = str1.Substring(0, num).Trim();
                        str1 = str1.Substring(num);
                    }
                    else
                        str3 = str1.Trim();
                    if (str3.Length > 0)
                        HavingList.Add(str3);
                }
                while (num > 0);
            }
        }

        public string DataTableName
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var str in TableList)
                {
                    if (stringBuilder.Length > 0)
                        stringBuilder.Append("_");
                    stringBuilder.Append(str);
                }
                return stringBuilder.ToString();
            }
        }

        public QueryBuilder()
        {
        }

        public QueryBuilder(string newQuery)
        {
            Query = newQuery;
        }

        public override string ToString()
        {
            return Query;
        }

        private string ParseJoinClause(string from, ref int startIndex)
        {
            var num1 = -1;
            var str1 = "";
            var strArray = new string[4]
      {
        " INNER JOIN ",
        " CROSS JOIN ",
        " LEFT OUTER JOIN ",
        " RIGHT OUTER JOIN "
      };
            foreach (var str2 in strArray)
            {
                var num2 = from.IndexOf(str2, startIndex, StringComparison.OrdinalIgnoreCase);
                if (num2 >= 0 && (num1 == -1 || num2 < num1))
                {
                    num1 = num2;
                    str1 = str2;
                }
            }
            startIndex = num1;
            var str3 = "";
            if (startIndex >= 0)
            {
                from = from.Substring(num1 + str1.Length).Trim();
                var length = @from.IndexOf(" ", System.StringComparison.Ordinal);
                str3 = length < 0 ? from : from.Substring(0, length);
            }
            return str3.Trim();
        }

        public void AddSelectCriteria(SelectCriteria selectCriteria)
        {
            if (selectCriteria == null)
                return;
            AddDbJoin(selectCriteria.DatabaseJoin);
            var addIsNull = selectCriteria.DateColumn.ToLower() == "timerange" || selectCriteria.DateColumn.ToLower() == "time range" || selectCriteria.DateColumn.ToLower() == "daterange" || selectCriteria.DateColumn.ToLower() == "date range";
            AddDateCriteria(selectCriteria.DateColumn, selectCriteria.BegTime, selectCriteria.EndTime, addIsNull);
            AddDbCriteria(selectCriteria.DatabaseCriteria);
        }

        public string AddDateCriteria(string datecolumn, DateTime begtime, DateTime endtime, bool addIsNull)
        {
            string str = DatabaseHelper.GetDateCriteriaClause(TableList, datecolumn, begtime, endtime, addIsNull);
            if (str.Length > 0)
            {
                str = "(" + str + ")";
                if (WhereList.Count > 0)
                    str = " AND " + str;
                WhereList.Add(str);
            }
            return str;
        }

        public string AddDbCriteria(params DatabaseCriteria[] dbCriteriaArray)
        {
            var list = new List<DatabaseCriteria>();
            foreach (var dbCriteria in dbCriteriaArray)
                list.Add(dbCriteria.Copy());
            var stringBuilder1 = new StringBuilder();
            for (var index = 0; index < list.Count; ++index)
            {
                var dbCriteria1 = list[index];
                DatabaseCriteria.AdjustParen(dbCriteria1);
                string dbCriteriaClause = DatabaseHelper.GetDbCriteriaClause(TableList, dbCriteria1);
                if (dbCriteriaClause.Length == 0)
                {
                    if (dbCriteria1.OpenParen.Length > 0)
                    {
                        if (index + 1 < list.Count)
                        {
                            list[index + 1].LogOperator = dbCriteria1.LogOperator;
                            var dbCriteria2 = list[index + 1];
                            string str = dbCriteria2.OpenParen + dbCriteria1.OpenParen;
                            dbCriteria2.OpenParen = str;
                        }
                    }
                    else if (dbCriteria1.CloseParen.Length > 0)
                    {
                        WhereList.Add(dbCriteria1.CloseParen);
                        stringBuilder1.Append(dbCriteria1.CloseParen);
                    }
                }
                else
                {
                    var stringBuilder2 = new StringBuilder();
                    if (WhereList.Count > 0)
                    {
                        if (dbCriteria1.LogOperator.Length < 2)
                            stringBuilder2.Append(" AND");
                        else
                            stringBuilder2.Append(" ").Append(dbCriteria1.LogOperator);
                    }
                    if (stringBuilder1.Length == 0)
                        stringBuilder2.Append(" (");
                    stringBuilder2.Append(" ").Append(dbCriteria1.OpenParen);
                    stringBuilder2.Append(" ").Append(dbCriteriaClause);
                    stringBuilder2.Append(" ").Append(dbCriteria1.CloseParen);
                    WhereList.Add(stringBuilder2.ToString());
                    stringBuilder1.Append(stringBuilder2.ToString());
                }
            }
            if (stringBuilder1.Length > 0)
            {
                WhereList.Add(") ");
                stringBuilder1.Append(") ");
            }
            return stringBuilder1.ToString();
        }

        public string AddDbJoin(params DatabaseJoin[] dbjoinarray)
        {
            var flag = false;
            for (var index = 0; index < dbjoinarray.Length; ++index)
            {
                if (dbjoinarray[index].DbTable != null && dbjoinarray[index].DbTable.Length != 0 && (dbjoinarray[index].DbColumn != null && dbjoinarray[index].DbColumn.Length != 0) && (dbjoinarray[index].JoinTable != null && dbjoinarray[index].JoinTable.Length != 0 && (dbjoinarray[index].JoinColumn != null && dbjoinarray[index].JoinColumn.Length != 0)) && (TableList.Contains(dbjoinarray[index].DbTable) || TableList.Contains(dbjoinarray[index].JoinTable)))
                    flag = true;
            }
            if (flag)
            {
                for (var index = 0; index < dbjoinarray.Length; ++index)
                {
                    if (dbjoinarray[index].DbTable != null && dbjoinarray[index].DbTable.Length != 0 && (dbjoinarray[index].DbColumn != null && dbjoinarray[index].DbColumn.Length != 0) && (dbjoinarray[index].JoinTable != null && dbjoinarray[index].JoinTable.Length != 0 && (dbjoinarray[index].JoinColumn != null && dbjoinarray[index].JoinColumn.Length != 0)))
                    {
                        if (!TableList.Contains(dbjoinarray[index].DbTable))
                            TableList.Add(dbjoinarray[index].DbTable);
                        if (!TableList.Contains(dbjoinarray[index].JoinTable))
                            TableList.Add(dbjoinarray[index].JoinTable);
                        var stringBuilder = new StringBuilder();
                        if (WhereList.Count > 0)
                            stringBuilder.Append(" AND");
                        stringBuilder.Append(" ").Append(dbjoinarray[index].DbTable).Append(".").Append(dbjoinarray[index].DbColumn).Append(" ").Append(dbjoinarray[index].JoinRelation).Append(" ").Append(dbjoinarray[index].JoinTable).Append(".").Append(dbjoinarray[index].JoinColumn);
                        if (stringBuilder.Length > 0)
                            WhereList.Add(stringBuilder.ToString());
                    }
                }
            }
            return Where;
        }

        public void AddWhereClause(string additionalWhere)
        {
            additionalWhere = additionalWhere.Trim();
            if (additionalWhere.Length == 0)
                return;
            if (additionalWhere.StartsWith("WHERE ", StringComparison.OrdinalIgnoreCase))
                additionalWhere = additionalWhere.Substring(6);
            if (WhereList.Count != 0 && !additionalWhere.StartsWith("AND ", StringComparison.OrdinalIgnoreCase) && !additionalWhere.StartsWith("OR ", StringComparison.OrdinalIgnoreCase))
            {
                if (additionalWhere.Trim('(', ')').Length != 0)
                {
                    WhereList.Add(" AND " + additionalWhere);
                    return;
                }
            }
            WhereList.Add(additionalWhere);
        }

        public void AddWhereClause(string dbColumn, string relationOperator, object dbValue)
        {
            if (dbColumn == null || relationOperator == null || dbValue == null)
                return;
            var str = dbColumn + relationOperator + DatabaseHelper.SqlValue(dbValue);
            if (WhereList.Count > 0)
                WhereList.Add(" AND " + str);
            else
                WhereList.Add(str);
        }

        public int ToGroupSql(string[] groupColumns, params AggregateColumn[] aggregateColumns)
        {
            if (groupColumns == null || groupColumns.Length == 0)
                return 0;
            for (var index = 0; index < ColumnList.Count; ++index)
            {
                var str1 = string.Empty;
                var tablename1 = string.Empty;
                var columnname = string.Empty;
                var str2 = ColumnList[index].Trim().ToLower();
                var length1 = str2.LastIndexOf(" ");
                if (length1 > 0)
                {
                    str1 = str2.Substring(length1 + 1).Trim();
                    str2 = str2.Substring(0, length1).Trim();
                }
                var length2 = str2.LastIndexOf(".");
                if (length2 > 0)
                {
                    tablename1 = str2.Substring(0, length2).Trim();
                    var num1 = Math.Max(tablename1.LastIndexOf("("), tablename1.LastIndexOf(" "));
                    if (num1 >= 0)
                        tablename1 = tablename1.Substring(num1 + 1);
                    var str3 = str2.Substring(length2 + 1).Trim();
                    var num2 = str3.Length;
                    var val2_1 = str3.IndexOf(" ");
                    if (val2_1 >= 0)
                        num2 = Math.Min(num2, val2_1);
                    var val2_2 = str3.IndexOf(")");
                    if (val2_2 >= 0)
                        num2 = Math.Min(num2, val2_2);
                    columnname = str3.Substring(0, num2);
                    if (str1.Length == 0)
                        str1 = columnname;
                }
                if (str1.Length == 0)
                    str1 = str2;
                if (columnname.Length == 0)
                    columnname = str1;
                if (!(str2 == "''") && !(str2 == "' '") && (str2.IndexOf(" 0 ") < 0 && str2.IndexOf("null") < 0) && (!str2.StartsWith("max(") && !str2.StartsWith("sum(") && !str2.StartsWith("avg(")))
                {
                    if (str2.Length == 0 || str2.EndsWith("."))
                        str2 = str2 + str1;
                    var flag = false;
                    foreach (var str3 in groupColumns)
                    {
                        if (str1.ToLower() == str3.ToLower())
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        if (!GroupList.Contains(str2))
                            GroupList.Add(str2);
                    }
                    else
                    {
                        var str3 = "";
                        DatabaseSchema.DbColumnRow dbColumn1 = DatabaseContent.GetDbColumn(tablename1, columnname);
                        if (dbColumn1 != null)
                        {
                            str3 = dbColumn1.DbType;
                        }
                        else
                        {
                            foreach (var tablename2 in TableList)
                            {
                                DatabaseSchema.DbColumnRow dbColumn2 = DatabaseContent.GetDbColumn(tablename2, columnname);
                                if (dbColumn2 != null)
                                {
                                    str3 = dbColumn2.DbType;
                                    break;
                                }
                            }
                        }
                        var str4 = "MAX";
                        if (str3 == "boolean")
                        {
                            str2 = "CASE(" + str2 + ") WHEN 1 THEN 1 ELSE 0 END";
                        }
                        else
                        {
                            if (!(str3 == "number"))
                            {
                                if (str2.Split('+', '-', '*', '/').Length <= 1)
                                    goto label_42;
                            }
                            str4 = "SUM";
                        }
                    label_42:
                        foreach (var aggregateColumn in aggregateColumns)
                        {
                            if (aggregateColumn.ColumnName == str1)
                            {
                                str4 = aggregateColumn.FunctionName;
                                break;
                            }
                        }
                        if (str4 == "MAX" && str1 == "begtime")
                            str4 = "MIN";
                        ColumnList[index] = str4 + "(" + str2 + ") " + str1;
                    }
                }
            }
            var query = Query;
            return GroupList.Count;
        }

        public int ToPagingSql(int pageSize, int pageNumber)
        {
            if (pageSize <= 0)
                return 0;
            if (pageNumber <= 0)
                pageNumber = 1;
            var queryBuilder = new QueryBuilder(Query);
            if (DatabaseHelper.DbSyntax == DatabaseType.MSSQL)
            {
                try
                {
                    var version = DatabaseHelper.ExecuteScalar("SELECT SERVERPROPERTY('productversion')") as string;
                    if (!string.IsNullOrEmpty(version))
                    {
                        if (new Version(version).Major < 9)
                            return 0;
                    }
                }
                catch
                {
                    //Todo: handle this
                    throw;
                }
                var str = queryBuilder.Order;
                if (str.Length > 0)
                    queryBuilder.Order = "";
                else
                    str = "ORDER BY " + ColumnList[0];
                queryBuilder.ColumnList.Add("ROW_NUMBER() OVER (" + str + ") RowNumber");
            }
            else
            {
                if (queryBuilder.Order.Length == 0)
                    queryBuilder.Order = "ORDER BY " + ColumnList[0];
                queryBuilder.ColumnList.Add("ROWNUM RowNumber");
            }
            var query1 = queryBuilder.Query;
            Select = "SELECT * ";
            From = "FROM (" + query1 + ") " + DataTableName;
            Where = string.Concat(new object[4]
      {
        "WHERE RowNumber>",
        pageSize * (pageNumber - 1),
        " AND RowNumber<=",
        pageSize * pageNumber + 1
      });
            Group = "";
            Having = "";
            Order = "";
            var query2 = Query;
            return pageNumber;
        }

        public class AggregateColumn
        {
            private string _columnName;
            private string _functionName;

            public string ColumnName
            {
                get
                {
                    return _columnName;
                }
            }

            public string FunctionName
            {
                get
                {
                    return _functionName;
                }
            }

            public AggregateColumn(string columnName, string functionName)
            {
                _columnName = columnName;
                _functionName = functionName;
            }
        }
    }
}
