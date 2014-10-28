using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CraigLib.Data
{
    public static class DatabaseContent
    {
        private static bool _cacheChanged;
        private static DatabaseSchema _dbSchema;

        public static bool ClearCache()
        {
            _cacheChanged = true;
            return true;
        }

        public static DatabaseSchema GetCurrentDbContent()
        {
            if (_dbSchema == null || _cacheChanged)
            {
                _dbSchema = (DatabaseSchema)CacheMachine.Get("DbContent", new string[0], null);
                _cacheChanged = false;
            }
            return _dbSchema;
        }

        public static DatabaseSchema GetDbContent()
        {
            return GetDbContent(string.Empty, ApplicationConfig.DbConnectInfo);
        }

        public static DatabaseSchema GetDbContent(string tableExpr)
        {
            return GetDbContent(tableExpr, ApplicationConfig.DbConnectInfo);
        }

        public static DatabaseSchema GetDbContent(string tableExpr, ConnectionInfo connInfo)
        {
            var dbSchema = new DatabaseSchema();
            using (var newConnection = DatabaseHelper.GetNewConnection(connInfo))
            {
                DatabaseHelper.OpenConnection(newConnection);
                FillSchema(dbSchema, tableExpr, newConnection);
                FillCheckConstraint(dbSchema, tableExpr, newConnection);
                FillForeignKey(dbSchema, tableExpr, newConnection);
                FillIndex(dbSchema, tableExpr, newConnection);
                newConnection.Close();
            }
            return dbSchema;
        }

        public static DatabaseSchema.DbTableRow GetDbTable(string tablename)
        {
            return GetCurrentDbContent().DbTable.FindByDbTable(tablename);
        }

        public static DatabaseSchema.DbColumnRow[] GetDbColumns(string tablename)
        {
            var dbTable = GetDbTable(tablename);
            return dbTable == null ? new DatabaseSchema.DbColumnRow[0] : dbTable.GetDbColumnRows();
        }

        public static DatabaseSchema.DbColumnRow[] GetDbColumns(string tablename, string filter, string sort)
        {
            var dbColumnRowArray = (DatabaseSchema.DbColumnRow[])null;
            var dbTable = GetDbTable(tablename);
            if (dbTable != null)
            {
                var filterExpression = "DbTable=" + Expr.Value(tablename);
                if (!string.IsNullOrEmpty(filter))
                    filterExpression = filterExpression + " AND " + filter;
                var dbSchema = (DatabaseSchema)dbTable.Table.DataSet;
                lock (dbSchema)
                    dbColumnRowArray = (DatabaseSchema.DbColumnRow[])dbSchema.DbColumn.Select(filterExpression, sort);
            }
            else
                dbColumnRowArray = new DatabaseSchema.DbColumnRow[0];
            return dbColumnRowArray;
        }

        public static DatabaseSchema.DbColumnRow GetDbColumn(string tablename, string columnname)
        {
            var dbColumnRow = (DatabaseSchema.DbColumnRow)null;
            var dbTable = GetDbTable(tablename);
            if (dbTable != null)
            {
                var dbSchema = (DatabaseSchema)dbTable.Table.DataSet;
                lock (dbSchema)
                    dbColumnRow = dbSchema.DbColumn.FindByDbTableDbColumn(tablename.ToLower(), columnname.ToLower());
            }
            return dbColumnRow;
        }

        public static DatabaseSchema.DbColumnRow[] GetPrimaryKeys(string tablename)
        {
            return GetDbColumns(tablename, "DbPrimaryKey=1", "DbSysGen");
        }

        public static string GetDbType(string tablename, string columnname)
        {
            if (string.IsNullOrEmpty(tablename))
            {
                var currentDbContent = GetCurrentDbContent();
                lock (currentDbContent)
                {
                    var local1 = (DatabaseSchema.DbColumnRow[])currentDbContent.DbColumn.Select("DbColumn = " + Expr.Value(columnname.ToLower()));
                    if (local1.Length > 0)
                        return local1[0].DbType;
                    foreach (var item0 in FindDbTablesByDbColumn(columnname))
                    {
                        var local4 = GetDbColumn(item0, columnname);
                        if (local4 != null)
                            return local4.DbType;
                    }
                    throw new Exception("Couldn't find column " + columnname);
                }
            }
            var dbColumn = GetDbColumn(tablename, columnname);
            if (dbColumn == null)
                throw new Exception("Couldn't find " + tablename + "." + columnname);
            return dbColumn.DbType;
        }

        private static string[] FindDbTablesByDbColumn(string columnname)
        {
            var list = new List<string>();
            using (var newConnection = DatabaseHelper.GetNewConnection())
            {
                DatabaseHelper.OpenConnection(newConnection);
                var cmdText = "";
                switch (DatabaseHelper.GetDbSyntax(newConnection))
                {
                    case DatabaseType.MSSQL:
                        cmdText = "SELECT sysobjects.name FROM sysobjects INNER JOIN syscolumns ON sysobjects.id=syscolumns.id WHERE syscolumns.name = " + DatabaseHelper.SqlChar(columnname);
                        break;
                    case DatabaseType.ORACLE:
                        cmdText = "SELECT lower(all_tab_columns.table_name) FROM all_tab_columns WHERE all_tab_columns.owner = " + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database) + " AND all_tab_columns.column_name = " + DatabaseHelper.SqlChar(columnname.ToUpper());
                        break;
                }
                using (var newCommand = DatabaseHelper.GetNewCommand(cmdText, newConnection))
                {
                    DatabaseHelper.WriteSqlLog(newCommand, 2);
                    using (var dbDataReader = newCommand.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            var @string = dbDataReader.GetString(0);
                            if (!string.IsNullOrEmpty(@string) && !list.Contains(@string))
                                list.Add(@string);
                        }
                        dbDataReader.Close();
                    }
                }
                newConnection.Close();
            }
            return list.ToArray();
        }

        public static DatabaseSchema.DbFKeyRow[] GetForeignKeys(string tablename)
        {
            var dbFkeyRowArray = new DatabaseSchema.DbFKeyRow[0];
            var filterExpression = "DbFKeyTable=" + Expr.Value(tablename);
            var currentDbContent = GetCurrentDbContent();
            lock (currentDbContent)
                return (DatabaseSchema.DbFKeyRow[])currentDbContent.DbFKey.Select(filterExpression);
        }

        public static DatabaseSchema.DbFKeyRow[] GetReferencedForeignKeys(string tablename)
        {
            var dbFkeyRowArray = new DatabaseSchema.DbFKeyRow[0];
            var filterExpression = "DbRefTable=" + Expr.Value(tablename);
            var currentDbContent = GetCurrentDbContent();
            lock (currentDbContent)
                return (DatabaseSchema.DbFKeyRow[])currentDbContent.DbFKey.Select(filterExpression);
        }

        public static int FillSchema(DatabaseSchema dbSchema, string tableExpr)
        {
            using (var newConnection = DatabaseHelper.GetNewConnection())
            {
                DatabaseHelper.OpenConnection(newConnection);
                var num = FillSchema(dbSchema, tableExpr, newConnection);
                newConnection.Close();
                return num;
            }
        }

        private static string ResolveMSSCheckCon(string dbExpr)
        {
            var num1 = dbExpr.IndexOf("[");
            var num2 = dbExpr.IndexOf("]", num1 + 1);
            var str1 = dbExpr.Substring(num1 + 1, num2 - num1 - 1);
            var num3 = 0;
            var str2 = string.Empty;
            while (true)
            {
                int num4;
                int num5;
                do
                {
                    num3 = dbExpr.IndexOf("[" + str1 + "]", num3 + 1);
                    if (num3 >= 0)
                    {
                        var length = dbExpr.IndexOf("[" + str1 + "]", num3 + 1);
                        if (length < 0)
                            length = dbExpr.Length;
                        num4 = dbExpr.IndexOf("'", num3 + 1);
                        num5 = dbExpr.Substring(0, length).LastIndexOf("'");
                        if (num4 < 0 && num5 < 0)
                        {
                            num4 = dbExpr.IndexOf("(", num3 + 1);
                            num5 = dbExpr.IndexOf(")", num4 + 1);
                        }
                    }
                    else
                        goto label_10;
                }
                while (num4 < 0 || num5 < 0);
                if (str2.Length > 0)
                    str2 = str2 + ",";
                var str3 = dbExpr.Substring(num4 + 1, num5 - num4 - 1).Replace("''", "'");
                str2 = str2 + str3;
            }
        label_10:
            return str2;
        }

        private static string GetTabTypeAdd(string tableExpr, string tableType)
        {
            var str = "";
            if (tableExpr.Length > 0)
                str = str + " AND sysobjects.name " + tableExpr;
            return !(tableType == "U") ? (!(tableType == "V") ? str + " AND (sysobjects.type='U' or sysobjects.type='V')" : str + " AND sysobjects.type='V'") : str + " AND sysobjects.type='U'";
        }

        public static int FillDbTableAzure(DatabaseSchema dbSchema, string tableExpr, string tableType, DbConnection conn)
        {
            var str1 = "SELECT (sysobjects.name) DbTable, (sysusers.name) DbGroup, (sysobjects.type) DbDefinition, '' ViewSQL FROM sysobjects INNER JOIN sysusers ON sysobjects.uid=sysusers.uid WHERE sysusers.name='dbo'";
            DatabaseHelper.FillDataTable(dbSchema.DbTable, str1 + GetTabTypeAdd(tableExpr, tableType));
            foreach (var dbTableRow in dbSchema.DbTable)
            {
                if (dbTableRow.DbDefinition.Trim() == "V")
                {
                    using (DatabaseHelper.GetNewConnection(true))
                    {
                        using (var newCommand = DatabaseHelper.GetNewCommand("sys.sp_helptext", conn))
                        {
                            newCommand.CommandType = CommandType.StoredProcedure;
                            newCommand.Parameters.Add(DatabaseHelper.GetNewParameter("@objname", dbTableRow.DbTable));
                            using (var dbDataReader = newCommand.ExecuteReader())
                            {
                                var str2 = "";
                                while (dbDataReader.Read())
                                    str2 = str2 + dbDataReader["Text"];
                                dbTableRow.ViewSQL = str2;
                            }
                        }
                    }
                }
            }
            dbSchema.DbTable.AcceptChanges();
            return dbSchema.DbTable.Count;
        }

        public static int FillSchemaAzure(DatabaseSchema dbSchema, string tableExpr, string tableType, DbConnection conn)
        {
            FillDbTableAzure(dbSchema, tableExpr, tableType, conn);
            DatabaseHelper.FillDataTable(dbSchema.DbColumn, "SELECT (sysobjects.name) DbTable, (syscolumns.name) DbColumn, (syscolumns.colid) DbSeq, (systypes.name) DbType, (syscolumns.prec) DbLen, (syscolumns.scale) DbPrecision, 0 DbPrimaryKey, (syscolumns.isnullable) DbNull, (syscolumns.colstat) DbIndex, '' DbDefault FROM sysobjects INNER JOIN sysusers ON sysobjects.uid=sysusers.uid INNER JOIN syscolumns ON sysobjects.id=syscolumns.id INNER JOIN systypes ON syscolumns.xusertype=systypes.xusertype WHERE sysusers.name='dbo' " + GetTabTypeAdd(tableExpr, tableType) + " order by sysobjects.name, syscolumns.colid");
            var str1 = "";
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var dbColumnRow in dbSchema.DbColumn)
            {
                var DbTable = dbColumnRow.DbTable.ToLower();
                if (DbTable != str1)
                {
                    dictionary.Clear();
                    using (DatabaseHelper.GetNewConnection(true))
                    {
                        using (var newCommand = DatabaseHelper.GetNewCommand("sys.sp_helpconstraint", conn))
                        {
                            newCommand.CommandType = CommandType.StoredProcedure;
                            newCommand.Parameters.Add(DatabaseHelper.GetNewParameter("@objname", DbTable));
                            newCommand.Parameters.Add(DatabaseHelper.GetNewParameter("@nomsg", "nomsg"));
                            using (var dbDataReader = newCommand.ExecuteReader())
                            {
                                while (dbDataReader.Read())
                                {
                                    if (dbDataReader.VisibleFieldCount >= 2)
                                    {
                                        var str2 = (string)dbDataReader["constraint_type"];
                                        if (str2.StartsWith("CHECK on column"))
                                        {
                                            var DbColumn = str2.Substring(16);
                                            dbColumnRow.Label = dbDataReader["constraint_name"].ToString();
                                            dbSchema.DbConstraint.AddDbConstraintRow(dbColumnRow.Label, "C", DbTable, DbColumn, "", ResolveMSSCheckCon(dbDataReader["constraint_keys"].ToString()));
                                        }
                                        else if (str2.StartsWith("DEFAULT on column"))
                                        {
                                            var index = str2.Substring(18);
                                            dictionary[index] = dbDataReader["constraint_keys"].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty);
                                            if (dictionary[index] == "getdate")
                                                dictionary[index] = "today";
                                        }
                                        else if (str2.StartsWith("PRIMARY KEY"))
                                            dbSchema.ExtendedProperties["_" + DbTable + "_pk"] = dbDataReader["constraint_name"];
                                    }
                                }
                            }
                        }
                    }
                    str1 = DbTable;
                }
                if (dbColumnRow.IsDbPrecisionNull())
                    dbColumnRow.DbPrecision = 0;
                dbColumnRow.DbSysGen = 0;
                string str3;
                if (dictionary.TryGetValue(dbColumnRow.DbColumn, out str3))
                    dbColumnRow.DbDefault = str3;
                var str4 = dbColumnRow.DbType.Trim().ToLower();
                if (str4 == "bit")
                    dbColumnRow.DbType = "boolean";
                else if (!str4.Contains("varchar") && !str4.Contains("date") && !str4.Contains("text"))
                    dbColumnRow.DbType = "number";
            }
            FillPrimaryKey(dbSchema, tableExpr, conn, 0);
            dbSchema.AcceptChanges();
            return dbSchema.DbTable.Count;
        }

        public static int FillSchema(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            return FillSchema(dbSchema, tableExpr, "", conn);
        }

        public static int FillSchema(DatabaseSchema dbSchema, string tableExpr, string tableType, DbConnection conn)
        {
            if (ApplicationConfig.IsSqlAzure)
                return FillSchemaAzure(dbSchema, tableExpr, tableType, conn);
            var count = dbSchema.DbTable.Count;
            var num = FillDbTable(dbSchema, tableExpr, tableType, conn);
            if (num == 0)
                return num;
            FillDbColumn(dbSchema, tableExpr, tableType, conn);
            var dbSyntax = DatabaseHelper.GetDbSyntax(conn);
            var strArray = new string[11]
      {
        "varchar",
        "varchar",
        "nvarchar",
        "number",
        "number",
        "date",
        "int",
        "text",
        "ntext",
        "image",
        "boolean"
      };
            var list = new List<string>();
            switch (dbSyntax)
            {
                case DatabaseType.MSSQL:
                    list.AddRange(new string[11]
                    {
                        "varchar",
                        "varchar",
                        "nvarchar",
                        "decimal",
                        "numeric",
                        "datetime",
                        "int",
                        "text",
                        "ntext",
                        "image",
                        "bit"
                    });
                    break;
                case DatabaseType.ORACLE:
                    list.AddRange(new string[11]
                    {
                        "varchar",
                        "varchar2",
                        "nvarchar2",
                        "number",
                        "number",
                        "date",
                        "int",
                        "clob",
                        "nclob",
                        "blob",
                        "boolean"
                    });
                    break;
            }
            for (var index1 = count; index1 < dbSchema.DbTable.Count; ++index1)
            {
                foreach (var dbColumnRow in dbSchema.DbTable[index1].GetDbColumnRows())
                {
                    dbColumnRow.BeginEdit();
                    var index2 = list.IndexOf(dbColumnRow.DbType);
                    if (index2 >= 0)
                        dbColumnRow.DbType = strArray[index2];
                    if (dbSyntax == DatabaseType.MSSQL)
                    {
                        if (dbColumnRow.DbIndex == 1)
                            dbColumnRow.DbType = "identity";
                        dbColumnRow.DbIndex = 0;
                    }
                    else if (dbSyntax == DatabaseType.ORACLE && dbColumnRow.DbType == "number" && dbColumnRow.DbIndex == 1)
                        dbColumnRow.DbType = "boolean";
                    switch (dbSyntax)
                    {
                        case DatabaseType.MSSQL:
                            if (dbColumnRow.DbType == "text" || dbColumnRow.DbType == "image")
                            {
                                dbColumnRow.DbLen = int.MaxValue;
                                break;
                            }
                            break;
                        case DatabaseType.ORACLE:
                            if (dbColumnRow.DbType == "number")
                                dbColumnRow.DbLen = dbColumnRow.DbIndex;
                            else if (dbColumnRow.DbType == "nvarchar")
                                dbColumnRow.DbLen = dbColumnRow.DbLen / 2;
                            dbColumnRow.DbIndex = 0;
                            if (dbColumnRow.DbDefinition == "N")
                                dbColumnRow.DbNull = 0;
                            dbColumnRow.DbDefinition = string.Empty;
                            break;
                    }
                    if (dbColumnRow.IsDbPrecisionNull())
                        dbColumnRow.DbPrecision = 0;
                    dbColumnRow.DbSysGen = 0;
                    if (dbColumnRow.DbDefault.Length > 0)
                    {
                        switch (dbSyntax)
                        {
                            case DatabaseType.MSSQL:
                                if (dbColumnRow.DbDefault.StartsWith("DEFAULT "))
                                    dbColumnRow.DbDefault = dbColumnRow.DbDefault.Substring(8).Trim();
                                dbColumnRow.DbDefault = dbColumnRow.DbDefault.Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty);
                                if (dbColumnRow.DbDefault == "getdate")
                                {
                                    dbColumnRow.DbDefault = "today";
                                    break;
                                }
                                break;
                            case DatabaseType.ORACLE:
                                dbColumnRow.DbDefault = dbColumnRow.DbDefault.Replace("'", string.Empty).Trim();
                                if (dbColumnRow.DbDefault == "sysdate")
                                {
                                    dbColumnRow.DbDefault = "today";
                                    break;
                                }
                                break;
                        }
                    }
                    dbColumnRow.EndEdit();
                }
            }
            FillPrimaryKey(dbSchema, tableExpr, conn, count);
            FillDefaultConstraint(dbSchema, tableExpr, conn);
            if (dbSyntax == DatabaseType.MSSQL)
            {
                using (var newCommand = DatabaseHelper.GetNewCommand("", conn))
                {
                    newCommand.CommandType = CommandType.Text;
                    newCommand.CommandText = "select (sysobjects.name) DbTable, (syscolumns.name) DbColumn from sysobjects, syscolumns where sysobjects.id=syscolumns.id and syscolumns.autoval is not null";
                    if (tableExpr.Length > 0)
                    {
                        var dbCommand = newCommand;
                        var str = dbCommand.CommandText + " and sysobjects.name " + tableExpr;
                        dbCommand.CommandText = str;
                    }
                    DatabaseHelper.WriteSqlLog(newCommand, 2);
                    var dbDataReader = newCommand.ExecuteReader();
                    while (dbDataReader.Read())
                    {
                        var DbTable = dbDataReader["DbTable"].ToString();
                        var DbColumn = dbDataReader["DbColumn"].ToString();
                        var byDbTableDbColumn = dbSchema.DbColumn.FindByDbTableDbColumn(DbTable, DbColumn);
                        if (byDbTableDbColumn != null)
                            byDbTableDbColumn.DbType = "identity";
                    }
                    dbDataReader.Close();
                }
            }
            dbSchema.DbColumn.AcceptChanges();
            return num;
        }

        public static int FillDbTable(DatabaseSchema dbSchema, string tableExpr, string tableType, DbConnection conn)
        {
            if (ApplicationConfig.IsSqlAzure)
                return FillDbTableAzure(dbSchema, tableExpr, tableType, conn);
            var count = dbSchema.DbTable.Count;
            var str1 = string.Empty;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    var str2 = "SELECT (sysobjects.name) DbTable, (sysusers.name) DbGroup, (sysobjects.type) DbDefinition, '' ViewSQL FROM sysobjects INNER JOIN sysusers ON sysobjects.uid=sysusers.uid WHERE sysusers.name='dbo' ";
                    if (tableExpr.Length > 0)
                        str2 = str2 + " AND sysobjects.name " + tableExpr;
                    var sqlCommand1 = !(tableType == "U") ? (!(tableType == "V") ? str2 + " AND (sysobjects.type='U' or sysobjects.type='V')" : str2 + " AND sysobjects.type='V'") : str2 + " AND sysobjects.type='U'";
                    DatabaseHelper.FillDataTable(dbSchema.DbTable, sqlCommand1, conn);
                    var dataRowArray = dbSchema.DbTable.Select("DbDefinition='V'");
                    if (dataRowArray.Length > 0)
                    {
                        using (var newCommand = DatabaseHelper.GetNewCommand("sys.sp_helptext", conn))
                        {
                            newCommand.CommandType = CommandType.StoredProcedure;
                            var newParameter = DatabaseHelper.GetNewParameter("@objname", "");
                            newCommand.Parameters.Add(newParameter);
                            foreach (DatabaseSchema.DbTableRow dbTableRow in dataRowArray)
                            {
                                newParameter.Value = dbTableRow.DbTable;
                                using (var dbDataReader = newCommand.ExecuteReader())
                                {
                                    var stringBuilder = new StringBuilder();
                                    while (dbDataReader.Read())
                                        stringBuilder.Append(dbDataReader["Text"]);
                                    dbTableRow.ViewSQL = stringBuilder.ToString();
                                }
                            }
                        }
                        dbSchema.DbTable.AcceptChanges();
                        break;
                    }
                    break;
                case DatabaseType.ORACLE:
                    var sqlCommand2 = "SELECT lower(all_tables.table_name) DbTable, (all_tables.owner) DbGroup, 'U' DbDefinition, (all_tables.tablespace_name) Label FROM all_tables WHERE all_tables.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database);
                    if (tableExpr.Length > 0)
                        sqlCommand2 = sqlCommand2 + " AND all_tables.table_name " + tableExpr.ToUpper();
                    if (string.IsNullOrEmpty(tableType) || tableType == "U")
                        DatabaseHelper.FillDataTable(dbSchema.DbTable, sqlCommand2, conn);
                    var sqlCommand3 = "SELECT lower(all_views.view_name) dbtable, (all_views.owner) dbgroup, 'V' DbDefinition, (all_views.text) ViewSQL, (all_views.owner) Label FROM all_views WHERE all_views.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database);
                    if (tableExpr.Length > 0)
                        sqlCommand3 = sqlCommand3 + " AND all_views.view_name " + tableExpr.ToUpper();
                    if (string.IsNullOrEmpty(tableType) || tableType == "V")
                    {
                        DatabaseHelper.FillDataTable(dbSchema.DbTable, sqlCommand3, conn);
                    }
                    break;
            }
            return dbSchema.DbTable.Count - count;
        }

        public static int FillDbColumn(DatabaseSchema dbSchema, string tableExpr, string tableType, DbConnection conn)
        {
            var count = dbSchema.DbColumn.Count;
            var str1 = string.Empty;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    var str2 = "SELECT (sysobjects.name) DbTable, (syscolumns.name) DbColumn, (syscolumns.colid) DbSeq, (systypes.name) DbType, (syscolumns.prec) DbLen, (syscolumns.scale) DbPrecision, 0 DbPrimaryKey, (syscolumns.isnullable) DbNull, (syscolumns.colstat) DbIndex, (syscomments.text) DbDefault FROM sysobjects INNER JOIN sysusers ON sysobjects.uid=sysusers.uid INNER JOIN syscolumns ON sysobjects.id=syscolumns.id INNER JOIN systypes ON syscolumns.xusertype=systypes.xusertype LEFT OUTER JOIN syscomments ON syscolumns.cdefault=syscomments.id WHERE sysusers.name='dbo' ";
                    if (tableExpr.Length > 0)
                        str2 = str2 + " AND sysobjects.name " + tableExpr;
                    var sqlCommand1 = !(tableType == "U") ? (!(tableType == "V") ? str2 + " AND (sysobjects.type='U' or sysobjects.type='V')" : str2 + " AND sysobjects.type='V'") : str2 + " AND sysobjects.type='U'";
                    DatabaseHelper.FillDataTable(dbSchema.DbColumn, sqlCommand1, conn);
                    break;
                case DatabaseType.ORACLE:
                    var str3 = "SELECT lower(all_tab_columns.table_name) DbTable, lower(all_tab_columns.column_name) DbColumn, (all_tab_columns.column_id) DbSeq, lower(all_tab_columns.data_type) DbType, (all_tab_columns.data_length) DbLen, (all_tab_columns.data_scale) DbPrecision, 0 DbPrimaryKey, 1 DbNull, upper(all_tab_columns.nullable) dbdefinition, (all_tab_columns.data_precision) DbIndex, (all_tab_columns.data_default) DbDefault FROM all_tab_columns ";
                    if (tableType == "U")
                        str3 = str3 + "INNER JOIN all_tables ON all_tables.table_name=all_tab_columns.table_name AND all_tables.owner=all_tab_columns.owner ";
                    else if (tableType == "V")
                        str3 = str3 + "INNER JOIN all_views ON all_views.view_name=all_tab_columns.table_name AND all_views.owner=all_tab_columns.owner ";
                    var sqlCommand2 = str3 + "WHERE all_tab_columns.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database);
                    if (tableExpr.Length > 0)
                        sqlCommand2 = sqlCommand2 + " AND all_tab_columns.table_name " + tableExpr.ToUpper();
                    DatabaseHelper.FillDataTable(dbSchema.DbColumn, sqlCommand2, conn);
                    break;
            }
            return dbSchema.DbColumn.Count - count;
        }

        private static int FillPrimaryKey(DatabaseSchema dbSchema, string tableExpr, DbConnection conn, int origCount)
        {
            var num1 = 0;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    for (var index = origCount; index < dbSchema.DbTable.Count; ++index)
                    {
                        var dbTableRow = dbSchema.DbTable[index];
                        dbTableRow.GetDbColumnRows();
                        using (var newCommand = DatabaseHelper.GetNewCommand(string.Empty, conn))
                        {
                            newCommand.CommandType = CommandType.StoredProcedure;
                            newCommand.CommandText = "dbo.sp_pkeys";
                            newCommand.Parameters.Add(DatabaseHelper.GetNewParameter("@table_name", dbTableRow.DbTable));
                            DatabaseHelper.WriteSqlLog(newCommand, 2);
                            var dbDataReader = newCommand.ExecuteReader();
                            while (dbDataReader.Read())
                            {
                                var DbColumn = dbDataReader["column_name"].ToString();
                                var num2 = Convert.ToInt32(dbDataReader["key_seq"]);
                                var byDbTableDbColumn = dbSchema.DbColumn.FindByDbTableDbColumn(dbTableRow.DbTable, DbColumn);
                                if (byDbTableDbColumn != null)
                                {
                                    byDbTableDbColumn.DbPrimaryKey = 1;
                                    byDbTableDbColumn.DbSysGen = num2;
                                    ++num1;
                                }
                            }
                            dbDataReader.Close();
                        }
                    }
                    break;
                case DatabaseType.ORACLE:
                    using (var newCommand = DatabaseHelper.GetNewCommand(string.Empty, conn))
                    {
                        newCommand.CommandType = CommandType.Text;
                        newCommand.CommandText = "SELECT lower(all_cons_columns.table_name) table_name, lower(all_cons_columns.column_name) column_name, (all_cons_columns.position) position FROM all_constraints,all_cons_columns WHERE all_constraints.owner=all_cons_columns.owner AND all_constraints.constraint_name=all_cons_columns.constraint_name AND all_constraints.table_name=all_cons_columns.table_name AND all_constraints.constraint_type='P' AND all_constraints.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database);
                        if (tableExpr.Length > 0)
                        {
                            var dbCommand = newCommand;
                            var str = dbCommand.CommandText + " AND all_constraints.table_name " + tableExpr.ToUpper();
                            dbCommand.CommandText = str;
                        }
                        DatabaseHelper.WriteSqlLog(newCommand, 2);
                        var dbDataReader = newCommand.ExecuteReader();
                        while (dbDataReader.Read())
                        {
                            var DbTable = dbDataReader["table_name"].ToString();
                            var DbColumn = dbDataReader["column_name"].ToString();
                            var num2 = Convert.ToInt32(dbDataReader["position"]);
                            var byDbTableDbColumn = dbSchema.DbColumn.FindByDbTableDbColumn(DbTable, DbColumn);
                            if (byDbTableDbColumn != null)
                            {
                                byDbTableDbColumn.DbPrimaryKey = 1;
                                byDbTableDbColumn.DbSysGen = num2;
                                ++num1;
                            }
                        }
                        dbDataReader.Close();
                        break;
                    }
            }
            return num1;
        }

        private static int FillDefaultConstraint(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            using (var constraintDataTable = new DatabaseSchema.DbConstraintDataTable())
            {
                var str = string.Empty;
                switch (DatabaseHelper.GetDbSyntax(conn))
                {
                    case DatabaseType.MSSQL:
                        var sqlCommand = "SELECT (def.name) DbConstraint, (def.type) ConstraintType, (tbl.name) DbTable, (col.name) DbColumn FROM sysobjects tbl, syscolumns col, sysconstraints const, sysobjects def WHERE (tbl.id = col.id) and ( col.id = const.id ) and (col.colid = const.colid) and (const.constid = def.id) and (def.type='D') ";
                        if (tableExpr.Length > 0)
                            sqlCommand = sqlCommand + " AND tbl.name " + tableExpr;
                        DatabaseHelper.FillDataTable(constraintDataTable, sqlCommand, conn);
                        break;
                }
                foreach (var dbConstraintRow in constraintDataTable)
                {
                    var byDbTableDbColumn = dbSchema.DbColumn.FindByDbTableDbColumn(dbConstraintRow.DbTable, dbConstraintRow.DbColumn);
                    if (byDbTableDbColumn != null)
                        byDbTableDbColumn.Label = dbConstraintRow.DbConstraint;
                }
                return constraintDataTable.Count;
            }
        }

        public static int FillCheckConstraint(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            if (ApplicationConfig.IsSqlAzure)
                return dbSchema.DbConstraint.Count;
            var count = dbSchema.DbConstraint.Count;
            var str1 = string.Empty;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    var sqlCommand1 = "SELECT (const.name) DbConstraint, (const.type) ConstraintType, convert(text,syscomments.text) DbExpr, (tbl.name) DbTable, (tbl.name) DbColumn FROM sysobjects tbl, sysobjects const, syscomments WHERE (tbl.id = const.parent_obj) and ( const.id = syscomments.id ) and (const.type='C') ";
                    if (tableExpr.Length > 0)
                        sqlCommand1 = sqlCommand1 + " AND tbl.name " + tableExpr;
                    DatabaseHelper.FillDataTable(dbSchema.DbConstraint, sqlCommand1, conn);
                    IEnumerator enumerator1 = dbSchema.DbConstraint.Rows.GetEnumerator();
                    try
                    {
                        while (enumerator1.MoveNext())
                        {
                            var dbConstraintRow = (DatabaseSchema.DbConstraintRow)enumerator1.Current;
                            var dbExpr = dbConstraintRow.DbExpr;
                            var num1 = dbExpr.IndexOf("[");
                            var num2 = dbExpr.IndexOf("]", num1 + 1);
                            var str2 = dbExpr.Substring(num1 + 1, num2 - num1 - 1);
                            var num3 = 0;
                            var str3 = string.Empty;
                            while (true)
                            {
                                int num4;
                                int num5;
                                do
                                {
                                    num3 = dbExpr.IndexOf("[" + str2 + "]", num3 + 1);
                                    if (num3 >= 0)
                                    {
                                        var length = dbExpr.IndexOf("[" + str2 + "]", num3 + 1);
                                        if (length < 0)
                                            length = dbExpr.Length;
                                        num4 = dbExpr.IndexOf("'", num3 + 1);
                                        num5 = dbExpr.Substring(0, length).LastIndexOf("'");
                                        if (num4 < 0 && num5 < 0)
                                        {
                                            num4 = dbExpr.IndexOf("(", num3 + 1);
                                            num5 = dbExpr.IndexOf(")", num4 + 1);
                                        }
                                    }
                                    else
                                        goto label_17;
                                }
                                while (num4 < 0 || num5 < 0);
                                if (str3.Length > 0)
                                    str3 = str3 + ",";
                                var str4 = dbExpr.Substring(num4 + 1, num5 - num4 - 1).Replace("''", "'");
                                str3 = str3 + str4;
                            }
                        label_17:
                            dbConstraintRow.BeginEdit();
                            dbConstraintRow.DbColumn = str2;
                            dbConstraintRow.DbExpr = str3;
                            dbConstraintRow.EndEdit();
                        }
                        break;
                    }
                    finally
                    {
                        var disposable = enumerator1 as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                case DatabaseType.ORACLE:
                    var sqlCommand2 = "SELECT lower(all_constraints.constraint_name) DbConstraint, (all_constraints.constraint_type) ConstraintType, (all_constraints.search_condition) DbExpr, lower(all_cons_columns.table_name) DbTable,lower(all_cons_columns.column_name) DbColumn  FROM all_cons_columns,all_constraints  WHERE (all_cons_columns.owner=all_constraints.owner) AND (all_cons_columns.constraint_name=all_constraints.constraint_name) AND (all_cons_columns.table_name=all_constraints.table_name) AND (all_constraints.constraint_type='C') AND (substr(all_constraints.constraint_name,1,3)='CK_') AND (all_constraints.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database) + ")";
                    if (tableExpr.Length > 0)
                        sqlCommand2 = sqlCommand2 + " AND all_constraints.table_name " + tableExpr.ToUpper();
                    DatabaseHelper.FillDataTable(dbSchema.DbConstraint, sqlCommand2, conn);
                    foreach (DatabaseSchema.DbConstraintRow dbConstraintRow in (InternalDataCollectionBase)dbSchema.DbConstraint.Rows)
                    {
                        var dbExpr = dbConstraintRow.DbExpr;
                        var num1 = dbExpr.IndexOf("(");
                        var num2 = dbExpr.LastIndexOf(")");
                        if (num1 >= 0 && num2 > num1)
                        {
                            var str2 = dbExpr.Substring(num1 + 1, num2 - num1 - 1).Replace("''", "^^").Replace("'", string.Empty).Replace("^^", "'");
                            dbConstraintRow.DbExpr = str2;
                        }
                    }
                    var sqlCommand3 = "SELECT lower(all_constraints.constraint_name) DbConstraint, (all_constraints.constraint_type) ConstraintType, lower(all_constraints.table_name) DbTable FROM all_constraints  WHERE (substr(all_constraints.constraint_name,1,3)='PK_') AND (all_constraints.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database) + ")";
                    if (tableExpr.Length > 0)
                        sqlCommand3 = sqlCommand3 + " AND all_constraints.table_name " + tableExpr.ToUpper();
                    DatabaseHelper.FillDataTable(dbSchema.DbConstraint, sqlCommand3, conn);
                    break;
            }
            dbSchema.DbConstraint.AcceptChanges();
            return dbSchema.DbConstraint.Count - count;
        }

        public static int FillForeignKey(DatabaseSchema dbSchema, string tableExpr)
        {
            using (var newConnection = DatabaseHelper.GetNewConnection())
            {
                DatabaseHelper.OpenConnection(newConnection);
                var num = FillForeignKey(dbSchema, tableExpr, newConnection);
                newConnection.Close();
                return num;
            }
        }

        public static int FillForeignKey(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            var count = dbSchema.DbFKey.Count;
            var str = string.Empty;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    var sqlCommand1 = "SELECT (sysobjects_a.name) DbFKey, (sysobjects_b.name) DbFKeyTable, (syscolumns.colid) DbFKeyColSeq, (syscolumns.name) DbFKeyColumn, (sysobjects_c.name) DbRefTable FROM sysobjects sysobjects_a, sysobjects sysobjects_b, sysobjects sysobjects_c, sysreferences, syscolumns WHERE ( sysobjects_a.id = sysreferences.constid ) AND ( sysreferences.fkeyid = sysobjects_b.id ) AND ( sysreferences.rkeyid = sysobjects_c.id ) AND ( sysobjects_b.id = syscolumns.id ) AND ( ( sysreferences.fkey1 = syscolumns.colid ) OR ( sysreferences.fkey2 = syscolumns.colid ) OR ( sysreferences.fkey3 = syscolumns.colid ) OR ( sysreferences.fkey4 = syscolumns.colid ) ) ";
                    if (tableExpr.Length > 0)
                        sqlCommand1 = sqlCommand1 + " AND sysobjects_b.name " + tableExpr;
                    DatabaseHelper.FillDataTable(dbSchema.DbFKey, sqlCommand1, conn);
                    break;
                case DatabaseType.ORACLE:
                    var sqlCommand2 = "SELECT lower(all_constraints.constraint_name) DbFKey, lower(all_constraints.table_name) DbFKeyTable, (all_cons_columns.position) DbFKeyColSeq, lower(all_cons_columns.column_name) DbFKeyColumn, lower(substr(r_constraint_name,4)) DbRefTable FROM all_cons_columns, all_constraints WHERE ( all_cons_columns.owner = all_constraints.owner ) AND ( all_cons_columns.constraint_name = all_constraints.constraint_name ) AND ( all_cons_columns.table_name = all_constraints.table_name ) AND ( all_constraints.constraint_type = 'R' ) AND ( all_constraints.owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database) + " )";
                    if (tableExpr.Length > 0)
                        sqlCommand2 = sqlCommand2 + " AND all_constraints.table_name " + tableExpr.ToUpper();
                    DatabaseHelper.FillDataTable(dbSchema.DbFKey, sqlCommand2, conn);
                    break;
            }
            var dataRowArray = dbSchema.DbFKey.Select(string.Empty, "DbFKey ASC, DbFKeyColSeq ASC");
            var dbFkeyRow1 = (DatabaseSchema.DbFKeyRow)null;
            foreach (var dbFkeyRow2 in dataRowArray.Cast<DatabaseSchema.DbFKeyRow>())
            {
                if (dbFkeyRow1 == null || dbFkeyRow1.DbFKey != dbFkeyRow2.DbFKey)
                {
                    dbFkeyRow1 = dbFkeyRow2;
                }
                else
                {
                    dbFkeyRow1.DbFKeyColumn = dbFkeyRow1.DbFKeyColumn + "," + dbFkeyRow2.DbFKeyColumn;
                    dbSchema.DbFKey.Rows.Remove(dbFkeyRow2);
                }
            }
            dbSchema.DbFKey.AcceptChanges();
            return dbSchema.DbFKey.Count - count;
        }

        public static int FillIndexAzure(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            foreach (var dbTableRow in dbSchema.DbTable)
            {
                using (DatabaseHelper.GetNewConnection(true))
                {
                    using (var newCommand = DatabaseHelper.GetNewCommand("sys.sp_helpindex", conn))
                    {
                        newCommand.CommandType = CommandType.StoredProcedure;
                        newCommand.Parameters.Add(DatabaseHelper.GetNewParameter("@objname", dbTableRow.DbTable));
                        using (var dbDataReader = newCommand.ExecuteReader())
                        {
                            while (dbDataReader.Read())
                            {
                                var DbIndex = (string)dbDataReader["index_name"];
                                if (!DbIndex.StartsWith("pk_", StringComparison.OrdinalIgnoreCase))
                                    dbSchema.DbIndex.AddDbIndexRow(DbIndex, dbTableRow.DbTable, 1, dbDataReader["index_keys"].ToString().ToLower().Replace(", ", ","));
                            }
                        }
                    }
                }
            }
            return dbSchema.DbIndex.Count;
        }

        public static int FillIndex(DatabaseSchema dbSchema, string tableExpr, DbConnection conn)
        {
            if (ApplicationConfig.IsSqlAzure)
                return FillIndexAzure(dbSchema, tableExpr, conn);
            var count = dbSchema.DbIndex.Count;
            var str = string.Empty;
            switch (DatabaseHelper.GetDbSyntax(conn))
            {
                case DatabaseType.MSSQL:
                    var sqlCommand1 = "SELECT (sysindexes.name) DbIndex, (sysobjects.name) DbIndexTable, (sysindexkeys.keyno) DbIndexColSeq, (syscolumns.name) DbIndexColumn FROM sysindexes, sysobjects, syscolumns, sysindexkeys WHERE sysobjects.id=sysindexes.id AND sysobjects.id=sysindexkeys.id AND sysindexes.indid=sysindexkeys.indid AND sysobjects.id=syscolumns.id AND sysindexkeys.colid=syscolumns.colid AND (sysobjects.type='U' or sysobjects.type='V') AND keycnt>0 ";
                    if (tableExpr.Length > 0)
                        sqlCommand1 = sqlCommand1 + " AND sysobjects.name " + tableExpr;
                    DatabaseHelper.FillDataTable(dbSchema.DbIndex, sqlCommand1, conn);
                    break;
                case DatabaseType.ORACLE:
                    var sqlCommand2 = "SELECT lower(all_ind_columns.index_name) DbIndex,lower(all_ind_columns.table_name) DbIndexTable,all_ind_columns.column_position DbIndexColSeq,lower(all_ind_columns.column_name) DbIndexColumn FROM all_ind_columns WHERE all_ind_columns.table_owner=" + DatabaseHelper.SqlChar(ApplicationConfig.DbConnectInfo.Database) + " AND all_ind_columns.index_name not like 'PK_%'";
                    if (tableExpr.Length > 0)
                        sqlCommand2 = sqlCommand2 + " AND all_ind_columns.table_name " + tableExpr.ToUpper();
                    DatabaseHelper.FillDataTable(dbSchema.DbIndex, sqlCommand2, conn);
                    break;
            }
            var dataRowArray = dbSchema.DbIndex.Select(string.Empty, "DbIndex ASC, DbIndexColSeq ASC");
            var dbIndexRow1 = (DatabaseSchema.DbIndexRow)null;
            foreach (DatabaseSchema.DbIndexRow dbIndexRow2 in dataRowArray)
            {
                if (dbIndexRow1 == null || dbIndexRow1.DbIndex != dbIndexRow2.DbIndex || dbIndexRow1.DbIndexTable != dbIndexRow2.DbIndexTable)
                {
                    dbIndexRow1 = dbIndexRow2;
                }
                else
                {
                    dbIndexRow1.DbIndexColumn = dbIndexRow1.DbIndexColumn + "," + dbIndexRow2.DbIndexColumn;
                    dbSchema.DbIndex.Rows.Remove(dbIndexRow2);
                }
            }
            dbSchema.DbIndex.AcceptChanges();
            return dbSchema.DbIndex.Count - count;
        }

        public static string[] SortTablesByForeignKey(string[] tables)
        {
            var tableList = new List<string>(tables);
            var list = new List<string>(tables);
            list.Sort((x, y) => GetTableLevelByForeignKey(x, tableList).CompareTo(GetTableLevelByForeignKey(y, tableList)));
            return list.ToArray();
        }

        public static DataTable[] SortTablesByForeignKey(DataTable[] dts)
        {
            var tableList = new List<string>();
            foreach (var dt in dts)
                tableList.Add(DataSetHelper.GetDbTable(dt));
            var list = new List<DataTable>(dts);
            list.Sort((x, y) => GetTableLevelByForeignKey(DataSetHelper.GetDbTable(x), tableList).CompareTo(GetTableLevelByForeignKey(DataSetHelper.GetDbTable(y), tableList)));
            return list.ToArray();
        }

        public static int GetTableLevelByForeignKey(string table, List<string> tableList)
        {
            var val1 = 0;
            foreach (var dbFkeyRow in GetForeignKeys(table))
            {
                var dbRefTable = dbFkeyRow.DbRefTable;
                if (tableList.Contains(dbRefTable) && !(dbRefTable == table))
                {
                    var levelByForeignKey = GetTableLevelByForeignKey(dbRefTable, tableList);
                    val1 = Math.Max(val1, levelByForeignKey + 1);
                }
            }
            return val1;
        }

        public static Decimal? GetNullableDecimal(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return new Decimal?();
            return new Decimal?(reader.GetDecimal(ordinal));
        }

        public static DateTime? GetNullableDateTime(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return new DateTime?();
            return new DateTime?(reader.GetDateTime(ordinal));
        }

        public static string GetNullableString(this DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetString(ordinal);
        }

        public static T GetEnum<T>(this DbDataReader reader, int ordinal) where T : struct
        {
            if (reader.IsDBNull(ordinal))
                return default(T);
            return (T)Enum.Parse(typeof(T), reader.GetString(ordinal), true);
        }
    }
}
