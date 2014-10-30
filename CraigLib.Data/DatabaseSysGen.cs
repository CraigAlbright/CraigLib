using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Principal;

namespace CraigLib.Data
{
    public static class DbSysGen
    {
        public static long SetSurrogate(DataTable dtUpdate, string tableName)
        {
            var num1 = 0L;
            const string index1 = "surrogate";
            var dataRowArray = dtUpdate.Select(null, null, DataViewRowState.Added);
            if (dataRowArray.Length > 0 && DatabaseContent.GetDbColumn(tableName, index1) != null)
            {
                num1 = Convert.ToInt64(GetUniqueID(tableName, index1, "", "", dataRowArray.Length));
                var index2 = dtUpdate.Columns.Contains(index1) ? dtUpdate.Columns[index1] : dtUpdate.Columns.Add(index1, typeof(long));
                for (var index3 = 0; index3 < dataRowArray.Length; ++index3)
                    dataRowArray[index3][index2] = -index3;
                var num2 = num1 - dataRowArray.Length;
                foreach (var dataRow in dataRowArray)
                {
                    ++num2;
                    dataRow[index2] = num2;
                }
            }
            return num1;
        }

        public static void SetDbDefaultValue(DataTable dtUpdate, string tableName)
        {
            if (dtUpdate.Select(null, null, DataViewRowState.Added).Length == 0)
                return;
            foreach (var dbColumnRow in DatabaseContent.GetDbColumns(tableName))
            {
                if (dbColumnRow.DbNull != 1 && !dbColumnRow.IsDbDefaultNull() && (dbColumnRow.DbDefault.Trim().Length != 0 && dtUpdate.Columns.Contains(dbColumnRow.DbColumn)))
                {
                    var column = dtUpdate.Columns[dbColumnRow.DbColumn];
                    var filterExpression = column.ColumnName + " is null";
                    var dataRowArray = dtUpdate.Select(filterExpression, null, DataViewRowState.Added);
                    if (dataRowArray.Length != 0)
                    {
                        var obj = dbColumnRow.DbType.IndexOf("char", StringComparison.Ordinal) >= 0 ||
                                  dbColumnRow.DbType.IndexOf("text", StringComparison.Ordinal) >= 0
                            ? dbColumnRow.DbDefault
                            : (dbColumnRow.DbType.IndexOf("date", StringComparison.Ordinal) < 0
                                ? Convert.ToInt32(dbColumnRow.DbDefault)
                                : (object) DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified));
                        foreach (var dataRow in dataRowArray)
                        {
                            dataRow[column] = obj;
                            dataRow.SetColumnError(column, "");
                        }
                    }
                }
            }
        }

        public static void SetCreationValue(DataTable dtUpdate, string creationName)
        {
            if (!dtUpdate.Columns.Contains("creationname") || !dtUpdate.Columns.Contains("creationdate"))
                return;
            var dataRowArray = dtUpdate.Select(null, null, DataViewRowState.Added);
            if (dataRowArray.Length <= 0)
                return;
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                var currentUser = windowsIdentity.Name;
                var str = creationName.Length > 0 ? creationName : currentUser;
                var serverDate = DatabaseHelper.GetServerDate();
                foreach (var dataRow in dataRowArray)
                {
                    dataRow.BeginEdit();
                    dataRow["creationname"] = str;
                    dataRow["creationdate"] = serverDate;
                    dataRow.EndEdit();
                }
            }
        }

        public static void SetRevisionValue(DataTable dtUpdate)
        {
            if (!dtUpdate.Columns.Contains("revisionname") || !dtUpdate.Columns.Contains("revisiondate"))
                return;
            var dataRowArray = dtUpdate.Select(null, null, DataViewRowState.ModifiedCurrent);
            if (dataRowArray.Length <= 0)
                return;
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                var userName = windowsIdentity.Name;
                var serverDate = DatabaseHelper.GetServerDate();
                foreach (var dataRow in dataRowArray)
                {
                    dataRow.BeginEdit();
                    dataRow["revisionname"] = userName;
                    dataRow["revisiondate"] = serverDate;
                    dataRow.EndEdit();
                }
            }
        }

        public static void SetSysGenValue(DataTable dtUpdate, string tableName, DbConnection connection, DbTransaction transaction)
        {
            var dataRowArray = dtUpdate.Select(null, null, DataViewRowState.Added);
            if (dataRowArray.Length == 0 || tableName == "dbtable" || (tableName == "dbview" || tableName == "dbobject") || tableName == "dbconstraint")
                return;
            foreach (DatabaseSchema.DbColumnRow dbColumnRow in DatabaseModel.GetDbColumns(tableName, "(DbSysGen=1 or DbColumn='collaboration')", ""))
            {
                var column = (DataColumn)null;
                foreach (DataColumn dc in dtUpdate.Columns)
                {
                    if (dc.GetDbTable().Equals(tableName, StringComparison.OrdinalIgnoreCase) && dc.GetDbColumn().Equals(dbColumnRow.DbColumn, StringComparison.OrdinalIgnoreCase))
                    {
                        column = dc;
                        break;
                    }
                }
                if (column != null)
                {
                    var list = dataRowArray.Where(dataRow => IsTempSysGen(dataRow[column])).ToList();
                    if (list.Count > 0)
                    {
                        var num = Convert.ToInt64(connection != null ? GetUniqueID(tableName, dbColumnRow.DbColumn, list.Count) : GetUniqueID(tableName, dbColumnRow.DbColumn, "", "", list.Count)) - list.Count;
                        foreach (var dataRow in list)
                        {
                            ++num;
                            dataRow[column] = dbColumnRow.DbType.IndexOf("char", StringComparison.Ordinal) >= 0 ||
                                              dbColumnRow.DbType.IndexOf("text", StringComparison.Ordinal) >= 0
                                ? num.ToString(CultureInfo.InvariantCulture)
                                : (object) num;
                            dataRow.SetColumnError(column, "");
                        }
                    }
                }
            }
        }

        public static bool IsTempSysGen(object colValue)
        {
            var flag = false;
            if (Expr.IsNull(colValue))
                flag = true;
            else
            {
                var s = colValue as string;
                if (s != null)
                {
                    var strNumber = s;
                    if (Expr.IsInteger(strNumber))
                    {
                        if (Convert.ToDouble(strNumber) <= 0.0 || strNumber.StartsWith("0"))
                            flag = true;
                    }
                    else if (strNumber.Length == 0)
                        flag = true;
                }
                else if (Expr.IsNumericValue(colValue) && Convert.ToDouble(colValue) <= 0.0)
                    flag = true;
            }
            return flag;
        }

        public static string GetUniqueID(string tablename, string columnname, string exprcol, string expr, int incr)
        {
            var str = "100001";
            var dbConnection = (DbConnection)null;
            var transaction = (DbTransaction)null;
            try
            {
                dbConnection = DatabaseHelper.GetNewConnection();
                DatabaseHelper.OpenConnection(dbConnection);
                transaction = ApplicationConfig.DbConnectInfo.DbSyntax != DatabaseType.MSSQL ? dbConnection.BeginTransaction() : dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                //str = GetUniqueID(tablename, columnname, exprcol, expr, incr, dbConnection, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                DatabaseHelper.WriteSqlLog(ex.ToString());
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
                if (dbConnection != null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return str;
        }

        private static string GetUniqueID(string tablename, string columnname, int incr)
        {
            var str = "100001";
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                var current = windowsIdentity.Name;
                for (var index = 0; index < 10; ++index)
                {
                    try
                    {
                        using (var newConnection = DatabaseHelper.GetNewConnection(true))
                        {
                            using (var dbTransaction = newConnection.BeginTransaction())
                            {
                                if (ApplicationConfig.DbConnectInfo.DbSyntax == DatabaseType.ORACLE)
                                {
                                    using (var command = newConnection.CreateCommand())
                                    {
                                        command.Transaction = dbTransaction;
                                        command.CommandType = CommandType.Text;
                                        command.CommandText = "select dbnumber from dbsysgen where dbtable = " + DatabaseHelper.SqlChar(tablename) + " and dbcolumn = " + DatabaseHelper.SqlChar(columnname) + " for update nowait";
                                        command.ExecuteScalar();
                                    }
                                }
                                using (var command = newConnection.CreateCommand())
                                {
                                    command.Transaction = dbTransaction;
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = "update dbsysgen set dbnumber = " + (ApplicationConfig.DbConnectInfo.DbSyntax == DatabaseType.MSSQL ? "cast(dbnumber as decimal) + " : (object)"dbnumber + ") + (string)(object)incr + " where dbtable = " + DatabaseHelper.SqlChar(tablename) + " and dbcolumn = " + DatabaseHelper.SqlChar(columnname);
                                    if (command.ExecuteNonQuery() == 0)
                                    {
                                        command.CommandText = "insert into dbsysgen (dbtable,dbcolumn,dbnumber,creationname,creationdate) values (" + DatabaseHelper.SqlChar(tablename) + ", " + DatabaseHelper.SqlChar(columnname) + ", 100001, " + DatabaseHelper.SqlChar(current) + ", " + DatabaseHelper.DefaultDate + ")";
                                        command.ExecuteNonQuery();
                                        command.CommandText = "update dbsysgen set dbnumber = (select case when max(" + (object)columnname + ") is null then 100001 else max(" + columnname + ")+" + (string)(object)incr + " end from " + tablename + ") where dbtable = " + DatabaseHelper.SqlChar(tablename) + " and dbcolumn = " + DatabaseHelper.SqlChar(columnname);
                                        command.ExecuteNonQuery();
                                    }
                                    command.CommandText = "select dbnumber from dbsysgen where dbtable = " + DatabaseHelper.SqlChar(tablename) + " and dbcolumn = " + DatabaseHelper.SqlChar(columnname);
                                    var obj = command.ExecuteScalar();
                                    dbTransaction.Commit();
                                    if (obj != null)
                                    {
                                        str = obj.ToString();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("ORA-00060", StringComparison.OrdinalIgnoreCase) >= 0 || ex.Message.IndexOf("ORA-00054", StringComparison.OrdinalIgnoreCase) >= 0 || ex.Message.IndexOf("deadlocked on lock", StringComparison.OrdinalIgnoreCase) >= 0)
                            --index;
                        if (index == 9)
                            throw;
                    }
                }
            }
            return str;
        }
    }
}
