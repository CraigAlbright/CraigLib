using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace CraigLib.Data
{
    public static class DatabaseHelper
    {
        private static readonly List<string> BinaryTypeNames = new List<string>(new[]
        {
            "Text",
            "NText",
            "Image",
            "Clob",
            "NClob",
            "Blob",
            "Long",
            "LongVarChar",
            "LongVarBinary"
        });
        public static DateTime SqlMinDate = new DateTime(1753, 1, 1);
        public static DateTime SqlMaxDate = new DateTime(9999, 12, 31);

        public static List<string> SqlReservedWords = new List<string>
        {
            "ADD",
            "EXTERNAL",
            "PROCEDURE",
            "ALL",
            "FETCH",
            "PUBLIC",
            "ALTER",
            "FILE",
            "RAISERROR",
            "AND",
            "FILLFACTOR",
            "READ",
            "ANY",
            "FOR",
            "READTEXT",
            "AS",
            "FOREIGN",
            "RECONFIGURE",
            "ASC",
            "FREETEXT",
            "REFERENCES",
            "AUTHORIZATION",
            "FREETEXTTABLE",
            "REPLICATION",
            "BACKUP",
            "FROM",
            "RESTORE",
            "BEGIN",
            "FULL",
            "RESTRICT",
            "BETWEEN",
            "FUNCTION",
            "RETURN",
            "BREAK",
            "GOTO",
            "REVERT",
            "BROWSE",
            "GRANT",
            "REVOKE",
            "BULK",
            "GROUP",
            "RIGHT",
            "BY",
            "HAVING",
            "ROLLBACK",
            "CASCADE",
            "HOLDLOCK",
            "ROWCOUNT",
            "CASE",
            "IDENTITY",
            "ROWGUIDCOL",
            "CHECK",
            "IDENTITY_INSERT",
            "RULE",
            "CHECKPOINT",
            "IDENTITYCOL",
            "SAVE",
            "CLOSE",
            "IF",
            "SCHEMA",
            "CLUSTERED",
            "IN",
            "SECURITYAUDIT",
            "COALESCE",
            "INDEX",
            "SELECT",
            "COLLATE",
            "INNER",
            "SEMANTICKEYPHRASETABLE",
            "COLUMN",
            "INSERT",
            "SEMANTICSIMILARITYDETAILSTABLE",
            "COMMIT",
            "INTERSECT",
            "SEMANTICSIMILARITYTABLE",
            "COMPUTE",
            "INTO",
            "SESSION_USER",
            "CONSTRAINT",
            "IS",
            "SET",
            "CONTAINS",
            "JOIN",
            "SETUSER",
            "CONTAINSTABLE",
            "KEY",
            "SHUTDOWN",
            "CONTINUE",
            "KILL",
            "SOME",
            "CONVERT",
            "LEFT",
            "STATISTICS",
            "CREATE",
            "LIKE",
            "SYSTEM_USER",
            "CROSS",
            "LINENO",
            "TABLE",
            "CURRENT",
            "LOAD",
            "TABLESAMPLE",
            "CURRENT_DATE",
            "MERGE",
            "TEXTSIZE",
            "CURRENT_TIME",
            "NATIONAL",
            "THEN",
            "CURRENT_TIMESTAMP",
            "NOCHECK",
            "TO",
            "CURRENT_USER",
            "NONCLUSTERED",
            "TOP",
            "CURSOR",
            "NOT",
            "TRAN",
            "DATABASE",
            "NULL",
            "TRANSACTION",
            "DBCC",
            "NULLIF",
            "TRIGGER",
            "DEALLOCATE",
            "OF",
            "TRUNCATE",
            "DECLARE",
            "OFF",
            "TRY_CONVERT",
            "DEFAULT",
            "OFFSETS",
            "TSEQUAL",
            "DELETE",
            "ON",
            "UNION",
            "DENY",
            "OPEN",
            "UNIQUE",
            "DESC",
            "OPENDATASOURCE",
            "UNPIVOT",
            "DISK",
            "OPENQUERY",
            "UPDATE",
            "DISTINCT",
            "OPENROWSET",
            "UPDATETEXT",
            "DISTRIBUTED",
            "OPENXML",
            "USE",
            "DOUBLE",
            "OPTION",
            "USER",
            "DROP",
            "OR",
            "VALUES",
            "DUMP",
            "ORDER",
            "VARYING",
            "ELSE",
            "OUTER",
            "VIEW",
            "END",
            "OVER",
            "WAITFOR",
            "ERRLVL",
            "PERCENT",
            "WHEN",
            "ESCAPE",
            "PIVOT",
            "WHERE",
            "EXCEPT",
            "PLAN",
            "WHILE",
            "EXEC",
            "PRECISION",
            "WITH",
            "EXECUTE",
            "PRIMARY",
            "WITHIN GROUP",
            "EXISTS",
            "PRINT",
            "WRITETEXT",
            "EXIT",
            "PROC"
        };
        private static readonly FileInfo SqlLogFile;

        public static string SqlConcat
        {
            get
            {
                var str = string.Empty;
                switch (DbSyntax)
                {
                    case DatabaseType.MSSQL:
                        str = "+";
                        break;
                    case DatabaseType.ORACLE:
                        str = "||";
                        break;
                }
                return str;
            }
        }

        public static string NullChar
        {
            get
            {
                var str = string.Empty;
                switch (DbSyntax)
                {
                    case DatabaseType.MSSQL:
                        str = "convert(char,null)";
                        break;
                    case DatabaseType.ORACLE:
                        str = "to_char(null)";
                        break;
                }
                return str;
            }
        }

        public static string NullDate
        {
            get
            {
                var str = string.Empty;
                switch (DbSyntax)
                {
                    case DatabaseType.MSSQL:
                    
                        str = "convert(datetime,null)";
                        break;
                    case DatabaseType.ORACLE:
                        str = "to_date(null)";
                        break;
                }
                return str;
            }
        }

        public static string NullNumber
        {
            get
            {
                var str = string.Empty;
                switch (DbSyntax)
                {
                    case DatabaseType.MSSQL:
                        str = "convert(decimal,null)";
                        break;
                    case DatabaseType.ORACLE:
                        str = "to_number(null)";
                        break;
                }
                return str;
            }
        }

        public static string DefaultChar
        {
            get
            {
                return "''";
            }
        }

        public static string DefaultDate
        {
            get
            {
                return GetDefaultDate(DbSyntax);
            }
        }

        public static string DefaultNumber
        {
            get
            {
                return "0.0";
            }
        }

        public static string DbCurrentDate
        {
            get
            {
                return ApplicationConfig.DbConnectInfo != null && ApplicationConfig.DbConnectInfo.DbSyntax == DatabaseType.ORACLE ? "sysdate" : "getdate()";
            }
        }

        public static DatabaseType DbSyntax
        {
            get
            {
                if (ApplicationConfig.DbConnectInfo != null)
                    return ApplicationConfig.DbConnectInfo.DbSyntax;
                return DatabaseType.MSSQL;
            }
        }

        static DatabaseHelper()
        {
            SqlLogFile = new FileInfo(Path.Combine(ApplicationConfig.LogPath, "sql.log"));
        }

        private static void AttachParameters(DbCommand command, IEnumerable<DbParameter> commandParameters)
        {
            foreach (var dbParameter in commandParameters)
            {
                if (dbParameter.Direction == ParameterDirection.InputOutput && dbParameter.Value == null)
                    dbParameter.Value = DBNull.Value;
                command.Parameters.Add(dbParameter);
            }
        }

        private static void AssignParameterValues(IList<DbParameter> commandParameters, IList<object> parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
                return;
            if (commandParameters.Count != parameterValues.Count)
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            var index = 0;
            for (var length = commandParameters.Count; index < length; ++index)
                commandParameters[index].Value = parameterValues[index];
        }

        private static void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters)
        {
            if (connection.State != ConnectionState.Open)
                OpenConnection(connection);
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandTimeout = ApplicationConfig.DbCommandTimeout;
            if (transaction != null)
                command.Transaction = transaction;
            command.CommandType = commandType;
            if (commandParameters == null)
                return;
            AttachParameters(command, commandParameters);
        }

        public static DbConnection GetNewConnection()
        {
            return GetNewConnection(ApplicationConfig.DbConnectInfo);
        }

        public static DbConnection GetNewConnection(bool open)
        {
            var newConnection = GetNewConnection(ApplicationConfig.DbConnectInfo);
            if (open)
                OpenConnection(newConnection);
            return newConnection;
        }

        public static DbConnection GetNewConnection(ConnectionInfo connInfo)
        {
            var connection = DbProviderFactories.GetFactory(connInfo.DbProvider).CreateConnection();
            if (connection == null)
            {
                return null;
            }
            connection.ConnectionString = connInfo.ConnectionString;
            return connection;
        }

        public static DbCommand GetNewCommand(string cmdText, DbConnection connection)
        {
            return GetNewCommand(cmdText, connection, null);
        }

        public static DbCommand GetNewCommand(string cmdText, DbTransaction transaction)
        {
            return GetNewCommand(cmdText, transaction.Connection, transaction);
        }

        public static DbCommand GetNewCommand(string cmdText, DbConnection connection, DbTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.CommandText = cmdText;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.Transaction = transaction;
            return command;
        }

        public static DbDataAdapter GetNewDataAdapter(string selectCommandText)
        {
            return GetNewDataAdapter(selectCommandText, GetNewConnection());
        }

        public static DbDataAdapter GetNewDataAdapter(string selectCommandText, DbConnection selectConnection)
        {
            return GetNewDataAdapter(selectCommandText, selectConnection, null);
        }

        public static DbDataAdapter GetNewDataAdapter(string selectCommandText, DbConnection selectConnection, DbTransaction selectTransaction)
        {
            return GetNewDataAdapter(GetNewCommand(selectCommandText, selectConnection, selectTransaction));
        }

        public static DbDataAdapter GetNewDataAdapter(DbCommand selectCommand)
        {
            var type = selectCommand.GetType();
            var fullName = type.Assembly.FullName;
            var dbDataAdapter =
                (DbDataAdapter)
                    Activator.CreateInstance(
                        Type.GetType(type.FullName.Replace("Command", "DataAdapter") + "," + fullName), new object[]
                        {
                            selectCommand
                        });

            return dbDataAdapter;
        }

        public static string GetParameterSql(DbParameter dataParameter)
        {
            return GetParameterSql(dataParameter, dataParameter.ParameterName);
        }

        public static string GetParameterSql(DbParameter dataParameter, string parameterName)
        {
            parameterName = parameterName.TrimStart('@', ':', '?');
            var type = dataParameter.GetType();
            parameterName = type.Name != "SqlParameter" ? (type.Name != "OracleParameter" ? "?" : ":" + parameterName) : "@" + parameterName;
            return parameterName;
        }

        public static string SetParameterName(DbParameter dataParameter)
        {
            return SetParameterName(dataParameter, dataParameter.ParameterName);
        }

        public static string SetParameterName(DbParameter dataParameter, string parameterName)
        {
            parameterName = parameterName.TrimStart('@', ':', '?');
            dataParameter.ParameterName = dataParameter.GetType().Name != "SqlParameter" ? parameterName : "@" + parameterName;
            return parameterName;
        }

        public static void SetParameterDbType(DbParameter parameter, DatabaseSchema.DbColumnRow colRow)
        {
            SetParameterDbType(parameter, colRow.DbType, colRow.DbLen, colRow.DbPrecision);
        }

        public static void SetParameterDbType(DbParameter parameter, string dbType, int dbLen, int dbPrecision)
        {
            var type = parameter.GetType();
            if (type.Name == "OracleParameter")
            {
                var str = "";
                var property1 = type.GetProperty("OracleDbType");
                if (property1 != null)
                {
                    if (dbType == "text")
                        str = "Clob";
                    else if (dbType == "ntext")
                        str = "NClob";
                    else if (dbType == "image")
                        str = "Blob";
                    else if (dbType == "long")
                        str = "Long";
                    else if (dbType == "date")
                        str = "Date";
                    else if (dbType == "boolean")
                        str = "Byte";
                    else if (dbType == "varchar")
                        str = "Varchar2";
                    else if (dbType == "nvarchar")
                        str = "NVarchar2";
                    if (str.Length > 0)
                    {
                        var obj = Enum.Parse(property1.PropertyType, str);
                        property1.SetValue(parameter, obj, null);
                        if (str != "Varchar2" && str != "NVarchar2")
                            return;
                        parameter.Size = dbLen;
                    }
                    else
                        type.GetProperty("Scale").SetValue(parameter, (byte)dbPrecision, null);
                }
                else
                {
                    var property2 = type.GetProperty("OracleType");
                    if (!(property2 != null))
                        return;
                    switch (dbType)
                    {
                        case "text":
                            str = "Clob";
                            break;
                        case "ntext":
                            str = "NClob";
                            break;
                        case "image":
                            str = "Blob";
                            break;
                        case "long":
                            str = "LongVarChar";
                            break;
                        case "date":
                            str = "DateTime";
                            break;
                        case "boolean":
                            str = "Byte";
                            break;
                        case "varchar":
                            str = "VarChar";
                            break;
                        case "nvarchar":
                            str = "NVarChar";
                            break;
                    }
                    if (str.Length <= 0)
                        return;
                    var obj = Enum.Parse(property2.PropertyType, str);
                    property2.SetValue(parameter, obj, null);
                    if (str != "VarChar" && str != "NVarChar")
                        return;
                    parameter.Size = dbLen;
                }
            }
            else if (type.Name == "SqlParameter")
            {
                var str = "";
                switch (dbType)
                {
                    case "text":
                        str = "Text";
                        break;
                    case "ntext":
                        str = "NText";
                        break;
                    case "image":
                        str = "Image";
                        break;
                    case "date":
                        str = "DateTime";
                        break;
                    case "boolean":
                        str = "Bit";
                        break;
                    case "varchar":
                        str = "VarChar";
                        break;
                    case "nvarchar":
                        str = "NVarChar";
                        break;
                }
                if (str.Length <= 0)
                    return;
                var property = type.GetProperty("SqlDbType");
                var obj = Enum.Parse(property.PropertyType, str);
                property.SetValue(parameter, obj, null);
                if (str != "VarChar")
                    return;
                parameter.Size = dbLen;
            }
            else
            {
                if (type.Name != "OleDbParameter")
                    return;
                switch (dbType)
                {
                    case "long":
                    case "ntext":
                    case "text":
                    {
                        var property = type.GetProperty("OleDbType");
                        var obj = Enum.Parse(property.PropertyType, "LongVarChar");
                        property.SetValue(parameter, obj, null);
                    }
                        break;
                    case "image":
                    {
                        var property = type.GetProperty("OleDbType");
                        var obj = Enum.Parse(property.PropertyType, "LongVarBinary");
                        property.SetValue(parameter, obj, null);
                    }
                        break;
                    default:
                        parameter.Size = dbLen;
                        type.GetProperty("Scale").SetValue(parameter, (byte)dbPrecision, null);
                        break;
                }
            }
        }

        public static DbParameter CloneParameter(DbParameter parameter)
        {
            return (DbParameter)((ICloneable)parameter).Clone();
        }

        public static bool IsBinaryParameter(DbParameter parameter)
        {
            var flag = false;
            var type = parameter.GetType();
            var propertyInfo = (PropertyInfo)null;
            switch (type.Name)
            {
                case "OracleParameter":
                    propertyInfo = type.GetProperty("OracleDbType") ?? type.GetProperty("OracleType");
                    break;
                case "SqlParameter":
                    propertyInfo = type.GetProperty("SqlDbType");
                    break;
                case "OleDbParameter":
                    propertyInfo = type.GetProperty("OleDbType");
                    break;
            }
            if (propertyInfo == null) return false;
            var obj = propertyInfo.GetValue(parameter, null);
            var name = Enum.GetName(propertyInfo.PropertyType, obj);
            if (BinaryTypeNames.Contains(name))
                flag = true;
            return flag;
        }

        public static DbParameter GetNewParameter()
        {
            return GetNewParameter(ApplicationConfig.DbConnectInfo);
        }

        public static DbParameter GetNewParameter(ConnectionInfo connInfo)
        {
            return DbProviderFactories.GetFactory(connInfo.DbProvider).CreateParameter();
        }

        public static DbParameter GetNewParameter(string parameterName, object value)
        {
            return GetNewParameter(ApplicationConfig.DbConnectInfo, parameterName, value);
        }

        public static DbParameter GetNewParameter(ConnectionInfo connInfo, string parameterName, object value)
        {
            var newParameter = GetNewParameter(connInfo);
            SetParameterName(newParameter, parameterName);
            newParameter.Value = value ?? DBNull.Value;
            return newParameter;
        }

        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(CommandType.Text, commandText);
        }

        public static int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, null);
        }

        public static int ExecuteNonQuery(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            using (var newConnection = GetNewConnection())
            {
                OpenConnection(newConnection);
                var num = ExecuteNonQuery(newConnection, commandType, commandText, commandParameters);
                newConnection.Close();
                return num;
            }
        }

        public static int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteNonQuery(CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(DbConnection connection, string commandText)
        {
            return ExecuteNonQuery(connection, CommandType.Text, commandText);
        }

        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = connection.CreateCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            WriteSqlLog(commandText, commandParameters, 1);
            var num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }

        public static int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int ExecuteNonQuery(DbTransaction transaction, string commandText)
        {
            return ExecuteNonQuery(transaction, CommandType.Text, commandText);
        }

        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = transaction.Connection.CreateCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            WriteSqlLog(commandText, commandParameters, 1);
            var rows = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return rows;
        }

        public static int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(CommandType.Text, commandText);
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return ExecuteDataset(commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            using (var newConnection = GetNewConnection())
            {
                OpenConnection(newConnection);
                var dataSet = ExecuteDataset(newConnection, commandType, commandText, commandParameters);
                newConnection.Close();
                return dataSet;
            }
        }

        public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteDataset(CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(DbConnection connection, string commandText)
        {
            return ExecuteDataset(connection, CommandType.Text, commandText);
        }

        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = connection.CreateCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            var newDataAdapter = GetNewDataAdapter(command);
            var dataSet = new DataSet();
            WriteSqlLog(commandText, commandParameters, 1);
            newDataAdapter.Fill(dataSet);
            command.Parameters.Clear();
            return dataSet;
        }

        public static DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DataSet ExecuteDataset(DbTransaction transaction, string commandText)
        {
            return ExecuteDataset(transaction, CommandType.Text, commandText);
        }

        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = transaction.Connection.CreateCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            var newDataAdapter = GetNewDataAdapter(command);
            var dataSet = new DataSet();
            WriteSqlLog(commandText, commandParameters, 1);
            newDataAdapter.Fill(dataSet);
            command.Parameters.Clear();
            return dataSet;
        }

        public static DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static int FillDataSet(DataSet dataSet, string tableName, string sqlCommand)
        {
            using (var newConnection = GetNewConnection())
            {
                var num = 0;
                try
                {
                    OpenConnection(newConnection);
                    num = FillDataSet(dataSet, tableName, sqlCommand, newConnection);
                }
                catch (Exception ex)
                {
                    WriteSqlLog(ex.Message);
                }
                finally
                {
                    newConnection.Close();
                }
                return num;
            }
        }

        public static int FillDataSet(DataSet dataSet, string tableName, string sqlCommand, DbConnection conn)
        {
            int rows;
            using (var newCommand = GetNewCommand(sqlCommand, conn))
            {
                using (var newDataAdapter = GetNewDataAdapter(newCommand))
                {
                    try
                    {
                        WriteSqlLog(sqlCommand, 2);
                        rows = newDataAdapter.Fill(dataSet, tableName);
                    }
                    catch (Exception ex)
                    {
                        rows = -1;
                        WriteSqlLog(ex.Message);
                    }
                }
            }
            return rows;
        }

        public static int FillDataTable(DataTable dataTable, string sqlCommand)
        {
            return FillDataTable(dataTable, sqlCommand, true);
        }

        public static int FillDataTable(DataTable dataTable, string sqlCommand, bool acceptChangesDuringFill)
        {
            using (var newConnection = GetNewConnection())
            {
                var num = 0;
                try
                {
                    OpenConnection(newConnection);
                    num = FillDataTable(dataTable, sqlCommand, acceptChangesDuringFill, newConnection);
                }
                catch (Exception ex)
                {
                    WriteSqlLog(ex.Message);
                }
                finally
                {
                    newConnection.Close();
                }
                return num;
            }
        }

        public static int FillDataTable(DataTable dataTable, string sqlCommand, DbConnection conn)
        {
            return FillDataTable(dataTable, sqlCommand, true, conn);
        }

        public static int FillDataTable(DataTable dataTable, string sqlCommand, bool acceptChangesDuringFill, DbConnection conn)
        {
            return FillDataTable(dataTable, sqlCommand, acceptChangesDuringFill, conn, true);
        }

        public static int FillDataTable(DataTable dataTable, string sqlCommand, bool acceptChangesDuringFill, DbConnection conn, bool log)
        {
            int rows;
            using (var newCommand = GetNewCommand(sqlCommand, conn))
            {
                var type = newCommand.GetType();
                if (type.Name == "OracleCommand")
                {
                    var property = type.GetProperty("InitialLONGFetchSize");
                    if (property != null)
                        property.SetValue(newCommand, 2048, null);
                }
                using (var newDataAdapter = GetNewDataAdapter(newCommand))
                {
                    try
                    {
                        if (log)
                        {
                            WriteSqlLog(sqlCommand, 2);
                        }
                        dataTable.BeginLoadData();
                        newDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        newDataAdapter.AcceptChangesDuringFill = acceptChangesDuringFill;
                        rows = newDataAdapter.Fill(dataTable);
                        dataTable.EndLoadData();
                    }
                    catch (Exception ex)
                    {
                        rows = -1;
                        if (log)
                            WriteSqlLog(ex.Message);
                    }
                    
                }
            }
            return rows;
        }

        public static void PreInitFill(DataTable dt, string sql)
        {
            using (var newConnection = GetNewConnection(true))
            {
                using (var command = newConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    using (var dbDataReader = command.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            var row = dt.NewRow();
                            for (var index = 0; index < dbDataReader.FieldCount; ++index)
                                row[index] = !(dt.Columns[index].DataType == typeof(int)) ? dbDataReader[index] : Convert.ToInt32(dbDataReader[index]);
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            dt.AcceptChanges();
        }

        private static DbDataReader ExecuteReader(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, DbParameter[] commandParameters, DbConnectionOwnership connectionOwnership)
        {
            var command = connection.CreateCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters);
            WriteSqlLog(commandText, commandParameters, 1);
            var dbDataReader = connectionOwnership != DbConnectionOwnership.External ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader();
            command.Parameters.Clear();
            return dbDataReader;
        }

        public static IEnumerable<DbDataReader> ExecuteSelect(string sql)
        {
            return ExecuteSelect(sql, false);
        }

        public static IEnumerable<DbDataReader> ExecuteSelect(string sql, bool preinit)
        {
            var count = 0;
            if (!preinit)
            {
                WriteSqlLog(sql);
            }
            using (var newConnection = GetNewConnection(true))
            {
                using (var dbDataReader = ExecuteReader(newConnection, sql))
                {
                    while (dbDataReader.Read())
                    {
                        ++count;
                        yield return dbDataReader;
                    }
                }
            }
        }

        public static DbDataReader ExecuteReader(DbConnection connection, string commandText)
        {
            return ExecuteReader(connection, CommandType.Text, commandText);
        }

        public static DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        public static DbDataReader ExecuteReader(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        public static DbDataReader ExecuteReader(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static DbDataReader ExecuteReader(DbTransaction transaction, string commandText)
        {
            return ExecuteReader(transaction, CommandType.Text, commandText, null);
        }

        public static DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        public static DbDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, DbConnectionOwnership.External);
        }

        public static DbDataReader ExecuteReader(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(CommandType.Text, commandText);
        }

        public static object ExecuteScalar(CommandType commandType, string commandText)
        {
            return ExecuteScalar(commandType, commandText, null);
        }

        public static object ExecuteScalar(CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            using (var newConnection = GetNewConnection())
            {
                OpenConnection(newConnection);
                var obj = ExecuteScalar(newConnection, commandType, commandText, commandParameters);
                newConnection.Close();
                return obj;
            }
        }

        public static object ExecuteScalar(string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteScalar(CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(DbConnection connection, string commandText)
        {
            return ExecuteScalar(connection, CommandType.Text, commandText);
        }

        public static object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = connection.CreateCommand();
            PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
            WriteSqlLog(commandText, commandParameters, 1);
            var obj = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj;
        }

        public static object ExecuteScalar(DbConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static object ExecuteScalar(DbTransaction transaction, string commandText)
        {
            return ExecuteScalar(transaction, CommandType.Text, commandText);
        }

        public static object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            var command = transaction.Connection.CreateCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
            WriteSqlLog(commandText, commandParameters, 1);
            var obj = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj;
        }

        public static object ExecuteScalar(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length <= 0)
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            var spParameterSet = DbParameterCache.GetSpParameterSet(spName);
            AssignParameterValues(spParameterSet, parameterValues);
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
        }

        public static string SqlFormat(string format, params object[] args)
        {
            var list = new List<string>();
            foreach (var obj in args)
            {
                if (obj is Array)
                {
                    var objArray = obj as object[];
                    if (objArray != null)
                    {
                        for (var index = 0; index < objArray.Length; ++index)
                            objArray[index] = SqlValue(objArray[index]);
                        list.Add(string.Join(",", (string[])objArray));
                    }
                }
                else
                    list.Add(SqlValue(obj));
            }
            return string.Format(format, list.ToArray());
        }

        public static string SqlValue(string value, string dbType)
        {
            return value == null || value == "<null>" ? "NULL" : (value != "<blank>" ? (dbType.IndexOf("char", StringComparison.Ordinal) >= 0 || dbType.IndexOf("text", StringComparison.Ordinal) >= 0 ? SqlChar(value) : (dbType.IndexOf("date", StringComparison.Ordinal) < 0 ? (dbType.IndexOf("bool", StringComparison.Ordinal) < 0 ? value : SqlBool(value)) : SqlTime(value))) : "''");
        }

        public static string SqlValue(object value)
        {
            return !Expr.IsNull(value) ? (!(value is string) ? (!(value is DateTime) ? (!(value is bool) ? (!(value is Decimal) ? (!(value is double) ? value.ToString() : ((double)value).ToString(CultureInfo.InvariantCulture)) : ((Decimal)value).ToString(CultureInfo.InvariantCulture)) : SqlBool((bool)value)) : SqlTime((DateTime)value)) : SqlChar((string)value)) : "null";
        }

        public static string SqlChar(string charValue)
        {
            charValue = charValue.Replace("'", "''");
            return "'" + charValue + "'";
        }

        public static string SqlDate(string dateString)
        {
            var dateValue = SqlMinDate;

            if (dateString.Equals("today", StringComparison.OrdinalIgnoreCase))
                dateValue = GetServerDate().Date;
            else if (dateString.Equals("now", StringComparison.OrdinalIgnoreCase))
            {
                dateValue = GetServerDate();
            }

            return SqlDate(dateValue);
        }

        public static string SqlDate(DateTime dateValue)
        {
            if (dateValue < SqlMinDate)
                dateValue = SqlMinDate;
            else if (dateValue > SqlMaxDate)
                dateValue = SqlMaxDate;
            var str = string.Empty;
            switch (DbSyntax)
            {
                case DatabaseType.MSSQL:
                
                    str = "'" + dateValue.ToString("yyyy-MM-dd") + "'";
                    break;
                case DatabaseType.ORACLE:
                    str = "to_date('" + dateValue.ToString("yyyy-MM-dd") + "','yyyy mm dd')";
                    break;
            }
            return str;
        }

        public static string SqlTime(string timeString)
        {
            var timeValue = SqlMinDate;

            if (timeString.Equals("today", StringComparison.OrdinalIgnoreCase))
                timeValue = GetServerDate().Date;
            else if (timeString.Equals("now", StringComparison.OrdinalIgnoreCase))
            {
                timeValue = GetServerDate();
            }
            else
            {
                if (timeString == DbCurrentDate)
                    return timeString;
            }
            return SqlTime(timeValue);
        }

        public static string SqlTime(DateTime timeValue)
        {
            if (timeValue < SqlMinDate)
                timeValue = SqlMinDate;
            else if (timeValue > SqlMaxDate)
                timeValue = SqlMaxDate;
            var str = string.Empty;
            switch (DbSyntax)
            {
                case DatabaseType.MSSQL:
                
                    str = "'" + timeValue.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                    break;
                case DatabaseType.ORACLE:
                    str = "to_date('" + timeValue.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')";
                    break;
            }
            return str;
        }

        public static string SqlBool(string boolString)
        {
            boolString = boolString.Trim();
                var b = boolString != "0" && boolString != "N" && (boolString == "1" || boolString == "Y" || Convert.ToBoolean(boolString));

            return SqlBool(b);
        }

        public static string SqlBool(bool b)
        {
            return b ? "1" : "0";
        }

        public static string SqlToChar(string dbcolumn)
        {
            var str = string.Empty;
            switch (DbSyntax)
            {
                case DatabaseType.MSSQL:
                
                    str = ApplicationConfig.UnicodeDB ? "convert(nvarchar," + dbcolumn + ")" : "convert(varchar," + dbcolumn + ")";
                    break;
                case DatabaseType.ORACLE:
                    str = ApplicationConfig.UnicodeDB ? "to_nchar(" + dbcolumn + ")" : "to_char(" + dbcolumn + ")";
                    break;
            }
            return str;
        }

        public static string SqlNvl(string col, string def)
        {
            if (DbSyntax == DatabaseType.ORACLE)
                return "Nvl(" + col + ", " + def + ")";
            return "isnull(" + col + ", " + def + ")";
        }

        public static string GetDefaultDate(DatabaseType dbSyntax)
        {
            var str = string.Empty;
            switch (dbSyntax)
            {
                case DatabaseType.MSSQL:
                
                    str = "getdate()";
                    break;
                case DatabaseType.ORACLE:
                    str = "sysdate";
                    break;
            }
            return str;
        }

        public static string GetSelectSql(DataTable dt, params string[] dbtables)
        {
            var queryBuilder = new QueryBuilder(dt.GetSelectSql());
            var tableList = new List<string>();
            foreach (DataColumn dataColumn in dt.Columns)
            {
                var dc = dataColumn;
                if(string.IsNullOrEmpty(dc.Expression))
                    continue;
                var indexCount = queryBuilder.ColumnList.FindIndex(s =>
                {
                    var local0 = s.TrimEnd(' ', ')');

                    if (!local0.Equals(dc.ColumnName, StringComparison.OrdinalIgnoreCase) &&
                        !local0.EndsWith(" " + dc.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return local0.EndsWith("." +dc.ColumnName,StringComparison
                                        .OrdinalIgnoreCase);
                    }
                    return true;
                }
                );
                if (indexCount >= 0)
                    continue;
                var dbTable = dc.GetDbTable();
                var dbcolumn = dc.GetDbColumn();
                //var dbExpr = dc.GetDbExpr();
                if (dbtables.Length > 0)
                {
                    if (dbcolumn.Length == 0)
                        dbcolumn = dc.ColumnName;
                    if (dbTable.Length == 0 || !IsMatch(dc, dbTable, dbcolumn) || Array.IndexOf(dbtables, dbTable) < 0)
                    {
                        dbTable = "";
                        foreach (var dbtable in dbtables.Where(dbtable => IsMatch(dc, dbtable, dbcolumn)))
                        {
                            dbTable = dbtable;
                            break;
                        }
                    }
                }
                var str3 = "";
                //var dbColumn = DatabaseContent.GetDbColumn(dbTable, dbcolumn);
                var str5 = CheckSqlReservedWord(dbTable);
                //var str6 = CheckSqlReservedWord(dbcolumn);
                var str7 = CheckSqlReservedWord(dc.ColumnName);
                //if (!string.IsNullOrEmpty(dbExpr))
                //{
                //    str3 = "(" + dbExpr + ") " + str7;
                //    str4 = str5;
                //}
                //else if (dbColumn != null)
                //{
                    //if (dc.IsProxy())
                    //{
                    //    if (dc.DefaultValue != null && dc.DefaultValue != DBNull.Value)
                    //        str3 = "(null) " + str7;
                    //}
                    //else
                    //{
                    //var dbCrosstab = dc.GetDbCrosstab();
                    //if (string.IsNullOrEmpty(dbCrosstab))
                    //    str3 = str5 + "." + str6 + " " + str7;
                    //else
                    //    str3 = "SUM(CASE WHEN " + dbCrosstab + " THEN " + str5 + "." + str6 + " ELSE 0 END) " + str7;
                    string str4 = str5;
                    //}
                //}
                //else 
                if (dc.DefaultValue != null && dc.DefaultValue != DBNull.Value && !dc.IsDynamicColumn())
                    str3 = "(null) " + str7;
                if (str3.Length <= 0) continue;
                queryBuilder.ColumnList.Add(str3);
                if (str4.Length <= 0 || queryBuilder.TableList.Contains(str4) ||queryBuilder.TableList.Contains(str4.ToUpper())) 
                    continue;
                queryBuilder.TableList.Add(str4);
                tableList.Add(str4);
            }
            if (queryBuilder.TableList.Count == 0)
                return string.Empty;
            if (dt.IsSelectDistinct())
                queryBuilder.SelectDistinct = true;
            var joinSql = GetJoinSql(tableList, dt.JoinByMatchColumnsOnly());
            if (!string.IsNullOrEmpty(joinSql))
                queryBuilder.AddWhereClause(joinSql);
            var dbWhere = dt.GetDbWhere();
            if (!string.IsNullOrEmpty(dbWhere))
                queryBuilder.AddWhereClause(dbWhere);
               
            var pageSize = dt.GetPageSize();
            var rowsLimit = dt.GetRowsLimit();
            if (pageSize > 0)
            {
                var pageNumber1 = dt.GetPageNumber();
                var pageNumber2 = queryBuilder.ToPagingSql(pageSize, pageNumber1);
                dt.SetPageNumber(pageNumber2);
            }
            else if (rowsLimit > 0)
                queryBuilder.RowsLimit = rowsLimit;
            return queryBuilder.Query;
        }

        private static bool IsMatch(DataColumn dc, string dbtable, string dbcolumn)
        {
            var dbColumn = DatabaseContent.GetDbColumn(dbtable, dbcolumn);
            return dbColumn != null &&
                   (!(dc.DataType == typeof (string)) || dbColumn.DbType.IndexOf("char", StringComparison.Ordinal) >= 0 ||
                    dbColumn.DbType.IndexOf("text", StringComparison.Ordinal) >= 0) &&
                   ((!(dc.DataType == typeof (bool)) || dbColumn.DbType.IndexOf("bool", StringComparison.Ordinal) >= 0) &&
                    (!(dc.DataType == typeof (DateTime)) ||
                     dbColumn.DbType.IndexOf("date", StringComparison.Ordinal) >= 0) &&
                    (!dc.IsNumericColumn() || dbColumn.DbType == "number" || dbColumn.DbType == "int"));
        }

        public static string GetSelectSql(DataTable dt, SelectCriteria criteria, params string[] dbtables)
        {
            var sql = GetSelectSql(dt, dbtables);
            return sql;
        }

        public static string CleanSqlString(string sql)
        {
            var strArray = sql.Split(new[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var stringBuilder = new StringBuilder();
            for (var index = 0; index < strArray.Length; ++index)
            {
                if (index != 0)
                    stringBuilder.Append(" ");
                stringBuilder.Append(strArray[index].Trim());
            }
            return stringBuilder.ToString();
        }

        private static List<DatabaseSchema.DbFKeyRow> GetForeignKeysForJoin(string tablename1, string tablename2, JoinDirection direction, List<DatabaseSchema.DbFKeyRow> trailList)
        {
            var list = new List<DatabaseSchema.DbFKeyRow>();
            if (trailList == null)
                trailList = new List<DatabaseSchema.DbFKeyRow>();
            foreach (var dbFkeyRow in direction == JoinDirection.Parent ? DatabaseContent.GetForeignKeys(tablename1) : DatabaseContent.GetReferencedForeignKeys(tablename1))
            {
                if (!trailList.Contains(dbFkeyRow))
                {
                    if (direction == JoinDirection.Parent && dbFkeyRow.DbRefTable.Equals(tablename2, StringComparison.OrdinalIgnoreCase) || direction == JoinDirection.Child && dbFkeyRow.DbFKeyTable.Equals(tablename2, StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(dbFkeyRow);
                        break;
                    }
                    trailList.Add(dbFkeyRow);
                    var foreignKeysForJoin = GetForeignKeysForJoin(direction == JoinDirection.Parent ? dbFkeyRow.DbRefTable : dbFkeyRow.DbFKeyTable, tablename2, direction, trailList);
                    if (foreignKeysForJoin.Count > 0)
                    {
                        list.AddRange(foreignKeysForJoin);
                        list.Add(dbFkeyRow);
                        break;
                    }
                }
            }
            return list;
        }

        public static string GetDbCriteriaClause(List<string> tableList, DatabaseCriteria databaseCriteria)
        {
            const string str1 = "";
            if (string.IsNullOrEmpty(databaseCriteria.DbColumn) || string.IsNullOrEmpty(databaseCriteria.DbValue))
                return str1;
            var str2 = "";
            var dbType = "";
            if (!string.IsNullOrEmpty(databaseCriteria.DbTable))
            {
                if (tableList.Contains(databaseCriteria.DbTable, StringComparer.OrdinalIgnoreCase))
                {
                    var dbColumn = DatabaseContent.GetDbColumn(databaseCriteria.DbTable, databaseCriteria.DbColumn);
                    if (dbColumn != null)
                    {
                        str2 = CheckSqlReservedWord(dbColumn.DbTable) + "." + CheckSqlReservedWord(dbColumn.DbColumn);
                        dbType = dbColumn.DbType;
                    }
                }
            }
            else
            {
                foreach (var dbColumn in tableList.Select(tablename => DatabaseContent.GetDbColumn(tablename, databaseCriteria.DbColumn)).Where(dbColumn => dbColumn != null))
                {
                    str2 = CheckSqlReservedWord(dbColumn.DbTable) + "." + CheckSqlReservedWord(dbColumn.DbColumn);
                    dbType = dbColumn.DbType;
                    break;
                }
            }
            if (string.IsNullOrEmpty(str2))
                return str1;
            var str3 = databaseCriteria.RelOperator.ToUpper();
            var sqlParamValue = databaseCriteria.DbValue;
            if (sqlParamValue == "<blank>")
                sqlParamValue = "";
            var timeperiod = SqlParamValue(sqlParamValue);
            if (timeperiod == "<null>")
            {
                str3 = str3.IndexOf("IS", StringComparison.Ordinal) < 0 ? (str3.IndexOf("<", StringComparison.Ordinal) >= 0 || str3.IndexOf(">", StringComparison.Ordinal) >= 0 || str3.IndexOf("NOT", StringComparison.Ordinal) >= 0 ? "IS NOT" : "IS") : databaseCriteria.RelOperator;
                timeperiod = "NULL";
            }
            else if (str3.IndexOf("IS", StringComparison.Ordinal) < 0)
            {
                
                if (str3.IndexOf("IN", StringComparison.Ordinal) >= 0)
                {
                    var str4 = timeperiod.Trim();
                    if (str4.StartsWith("(") && str4.EndsWith(")"))
                    {
                        var str5 = str4.Remove(0, 1);
                        str4 = str5.Remove(str5.Length - 1, 1);
                    }
                    if (!str4.Trim().StartsWith("SELECT ", StringComparison.OrdinalIgnoreCase))
                    {
                        var strArray = str4.Split(new[]{','});
                        for (var index = 0; index < strArray.Length; ++index)
                        {
                            strArray[index] = strArray[index].Trim();
                            if (strArray[index].StartsWith("'") && strArray[index].EndsWith("'"))
                            {
                                strArray[index] = strArray[index].Remove(0, 1);
                                strArray[index] = strArray[index].Remove(strArray[index].Length - 1, 1);
                            }
                            else if (strArray[index].StartsWith("'") && index + 1 < strArray.Length && !strArray[index + 1].StartsWith("'"))
                            {
                                strArray[index + 1] = strArray[index] + "," + strArray[index + 1];
                                strArray[index] = "";
                                continue;
                            }
                            strArray[index] = SqlValue(strArray[index], dbType);
                        }
                        str4 = "";
                        foreach (var str5 in strArray.Where(str5 => !string.IsNullOrEmpty(str5)))
                        {
                            if (str4.Length > 0)
                                str4 = str4 + ",";
                            str4 = str4 + str5;
                        }
                    }
                    timeperiod = "(" + str4 + ")";
                }
                else if (timeperiod.IndexOf("SELECT ", StringComparison.OrdinalIgnoreCase) >= 0 && timeperiod.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    if (!timeperiod.Trim().StartsWith("(") && !timeperiod.Trim().EndsWith(")"))
                        timeperiod = "(" + timeperiod + ")";
                }
                else
                    timeperiod = SqlValue(timeperiod, dbType);
            }
            if (dbType.IndexOf("text", StringComparison.Ordinal) >= 0 && DbSyntax == DatabaseType.ORACLE && timeperiod != "NULL")
                str2 = "dbms_lob.substr(" + str2 + ")";
            if (databaseCriteria.DbValue.Trim().ToUpper() == ":USERID")
            {
                str2 = "lower(" + str2 + ")";
                timeperiod = timeperiod.ToLower();
            }
            string str6;
            if (str3 == "<>" && timeperiod != "NULL")
                str6 = "(" + str2 + " " + str3 + " " + timeperiod + " OR " + str2 + " IS NULL)";
            else
                str6 = str2 + " " + str3 + " " + timeperiod;
            return str6;
        }

        public static string SqlParamValue(string sqlParamValue)
        {
            switch (sqlParamValue.Trim().ToUpper())
            {
                   
                case ":USERID":
                    var windowsIdentity = WindowsIdentity.GetCurrent();
                    if (windowsIdentity != null)
                        sqlParamValue = windowsIdentity.Name;
                    break;
                case ":USERNAME":
                case ":CREATIONNAME":
                case ":REVISIONNAME":
                    var wi = WindowsIdentity.GetCurrent();
                    if (wi != null)
                        sqlParamValue = wi.Name;
                    break;
            }
            return sqlParamValue;
        }

        public static string GetJoinSql(List<string> tableList)
        {
            return GetJoinSql(tableList, false);
        }

        public static string GetJoinSql(List<string> tableList, bool byMatchColumnsOnly)
        {
            var stringBuilder = new StringBuilder();
            if (tableList.Count <= 1)
                return stringBuilder.ToString();
            for (var index = 0; index < tableList.Count; ++index)
                tableList[index] = tableList[index].ToLower();
            var list1 = new List<string>();
            foreach (var tablename1 in tableList)
            {
                var foreignKeys = DatabaseContent.GetForeignKeys(tablename1);
                var dictionary = new Dictionary<string, List<DatabaseSchema.DbFKeyRow>>();
                foreach (var dbFkeyRow in foreignKeys.Where(dbFkeyRow => dbFkeyRow.DbRefTable != tablename1 && tableList.Contains(dbFkeyRow.DbRefTable)))
                {
                    List<DatabaseSchema.DbFKeyRow> list2;
                    if (!dictionary.TryGetValue(dbFkeyRow.DbRefTable, out list2))
                        dictionary[dbFkeyRow.DbRefTable] = list2 = new List<DatabaseSchema.DbFKeyRow>();
                    list2.Add(dbFkeyRow);
                }
                foreach (var keyValuePair in dictionary)
                {
                    var str1 = "";
                    foreach (var dbFkeyRow in keyValuePair.Value)
                    {
                        var str2 = "";
                        var primaryKeys = DatabaseContent.GetPrimaryKeys(dbFkeyRow.DbRefTable);
                        var strArray = dbFkeyRow.DbFKeyColumn.Split(new[]{','});
                        if (primaryKeys.Length != 0 && primaryKeys.Length == strArray.Length)
                        {
                            var flag = true;
                            for (var index = 0; index < primaryKeys.Length; ++index)
                            {
                                if (primaryKeys[index].DbColumn != strArray[index])
                                    flag = false;
                                if (str2.Length > 0)
                                    str2 = str2 + " AND ";
                                str2 = str2 + dbFkeyRow.DbRefTable + "." + primaryKeys[index].DbColumn + "=" + tablename1 + "." + strArray[index];
                            }
                            if ((!byMatchColumnsOnly || flag) && str2.Length > 0)
                            {
                                if (str2.IndexOf(" AND ", StringComparison.Ordinal) > 0)
                                    str2 = "(" + str2 + ")";
                                if (str1.Length > 0)
                                    str1 = str1 + " OR ";
                                str1 = str1 + str2;
                                if (!list1.Contains(tablename1))
                                    list1.Add(tablename1);
                                if (!list1.Contains(dbFkeyRow.DbRefTable))
                                    list1.Add(dbFkeyRow.DbRefTable);
                            }
                        }
                    }
                    if (str1.Length > 0)
                    {
                        if (str1.IndexOf(" OR ", StringComparison.Ordinal) > 0)
                            str1 = "(" + str1 + ")";
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append(" AND ");
                        stringBuilder.Append(str1);
                    }
                }
                if (!list1.Contains(tablename1))
                {
                    var dbColumns1 = DatabaseContent.GetDbColumns(tablename1);
                    var primaryKeys = DatabaseContent.GetPrimaryKeys(tablename1);
                    foreach (var tablename2 in tableList)
                    {
                        if (tablename2 == tablename1) continue;
                        var list2 = new List<DatabaseSchema.DbColumnRow>();
                        var list3 = new List<DatabaseSchema.DbColumnRow>();
                        foreach (var dbColumnRow1 in DatabaseContent.GetPrimaryKeys(tablename2))
                        {
                            if (!dbColumnRow1.DbColumn.Equals("surrogate", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var dbColumnRow2 in dbColumns1.Where(dbColumnRow2 => dbColumnRow1.DbColumn.Equals(dbColumnRow2.DbColumn,
                                    StringComparison.OrdinalIgnoreCase)))
                                {
                                    list2.Add(dbColumnRow1);
                                    list3.Add(dbColumnRow2);
                                    break;
                                }
                            }
                            else
                                break;
                        }
                        if (list2.Count == 0 || list2.Count != list3.Count)
                        {
                            list2.Clear();
                            list3.Clear();
                            var dbColumns2 = DatabaseContent.GetDbColumns(tablename2);
                            foreach (var dbColumnRow1 in primaryKeys)
                            {
                                if (!dbColumnRow1.DbColumn.Equals("surrogate", StringComparison.OrdinalIgnoreCase))
                                {
                                    foreach (var dbColumnRow2 in dbColumns2)
                                    {
                                        if (dbColumnRow1.DbColumn.Equals(dbColumnRow2.DbColumn, StringComparison.OrdinalIgnoreCase))
                                        {
                                            list2.Add(dbColumnRow1);
                                            list3.Add(dbColumnRow2);
                                            break;
                                        }
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        if (list2.Count > 0 && list2.Count == list3.Count)
                        {
                            for (var index = 0; index < list2.Count; ++index)
                            {
                                if (stringBuilder.Length > 0)
                                    stringBuilder.Append(" AND ");
                                var dbColumnRow1 = list2[index];
                                var dbColumnRow2 = list3[index];
                                stringBuilder.Append(dbColumnRow1.DbTable).Append(".").Append(dbColumnRow1.DbColumn).Append("=").Append(dbColumnRow2.DbTable).Append(".").Append(dbColumnRow2.DbColumn);
                            }
                            if (!list1.Contains(tablename1))
                                list1.Add(tablename1);
                            if (!list1.Contains(tablename2))
                                list1.Add(tablename2);
                        }
                    }
                }
            }
            return stringBuilder.ToString();
        }

        public static string[] GetSelectTables(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return new string[0];
            var str1 = new QueryBuilder(sql).From + " ";
            var dictionary = new Dictionary<string, string>();
            foreach (var str2 in from DataRow dataRow in (InternalDataCollectionBase)((DataSet)CacheMachine.Get("DatabaseModel")).Tables["dbtable"].Rows select " " + 
                                     (string)dataRow["dbtable"] + " " into str2 where str1.IndexOf(str2, StringComparison.OrdinalIgnoreCase) >= 0 select str2)
            {
                dictionary[str2.Trim()] = "";
            }
            return dictionary.Keys.ToArray();
        }

        public static string[] GetViewTables(string dbviewname)
        {
            return GetSelectTables(ExecuteScalar("select viewsql from dbview where dbtable = " + SqlChar(dbviewname)) as string);
        }

        public static string GetDateCriteriaClause(List<string> tableList, string timeColumn, DateTime begtime, DateTime endtime, bool addIsNull)
        {
            var str1 = "";
            if (string.IsNullOrEmpty(timeColumn) || begtime <= SqlMinDate && endtime >= SqlMaxDate)
                return str1;
            if (begtime <= SqlMinDate || endtime >= SqlMaxDate)
                addIsNull = false;
            if (timeColumn.ToLower() == "timerange" || timeColumn.ToLower() == "time range" || (timeColumn.ToLower() == "daterange" || timeColumn.ToLower() == "date range"))
            {
                var dbColumnRow = (DatabaseSchema.DbColumnRow)null;
                foreach (var tablename in tableList)
                {
                    dbColumnRow = DatabaseContent.GetDbColumn(tablename, "begtime");
                    if (dbColumnRow != null)
                        break;
                }
                if (dbColumnRow == null) return str1;
                string str2;
                if (endtime < SqlMaxDate)
                {
                    str2 = dbColumnRow.DbTable + "." + dbColumnRow.DbColumn;
                    if (addIsNull)
                        str1 = str1 + "(" + str2 + "<" + SqlTime(endtime) + " OR " + str2 + " is null)";
                    else
                        str1 = str1 + str2 + "<" + SqlTime(endtime);
                }
                var dbColumn = DatabaseContent.GetDbColumn(dbColumnRow.DbTable, "endtime");
                if (dbColumn == null || begtime <= SqlMinDate) return str1;
                str2 = dbColumn.DbTable + "." + dbColumn.DbColumn;
                if (str1.Length > 0)
                    str1 = str1 + " AND ";
                if (addIsNull)
                    str1 = str1 + "(" + str2 + ">" + SqlTime(begtime) + " OR " + str2 + " is null)";
                else
                    str1 = str1 + str2 + ">" + SqlTime(begtime);
            }
            else
            {
                var dbColumnRow = (DatabaseSchema.DbColumnRow)null;
                foreach (var tablename in tableList)
                {
                    dbColumnRow = DatabaseContent.GetDbColumn(tablename, timeColumn);
                    if (dbColumnRow != null)
                        break;
                }
                if (dbColumnRow == null)
                    return str1;
                var str2 = dbColumnRow.DbTable + "." + dbColumnRow.DbColumn;
                if (addIsNull)
                {
                    if (begtime > SqlMinDate)
                        str1 = str1 + "(" + str2 + ">=" + SqlTime(begtime) + " OR " + str2 + " is null)";
                    if (endtime >= SqlMaxDate) return str1;
                    if (str1.Length > 0)
                        str1 = str1 + " AND ";
                    str1 = str1 + "(" + str2 + "<" + SqlTime(endtime) + " OR " + str2 + " is null)";
                }
                else
                {
                    if (begtime > SqlMinDate)
                        str1 = str1 + str2 + ">=" + SqlTime(begtime);
                    if (endtime >= SqlMaxDate) return str1;
                    if (str1.Length > 0)
                        str1 = str1 + " AND ";
                    str1 = str1 + str2 + "<" + SqlTime(endtime);
                }
            }
            return str1;
        }

        //private static string GetDateCriteriaClauseFromRelation(DataTable dt, string timeColumn, DateTime begtime, DateTime endtime, bool addIsNull, JoinDirection direction, QueryBuilder qb)
        //{
        //    var list = new List<string>();
        //    foreach (DataRelation relation in direction == JoinDirection.Parent ? dt.ParentRelations : dt.ChildRelations)
        //    {
        //        if (!DataSetHelper.IsRecursive(relation))
        //        {
        //            var dt1 = direction == JoinDirection.Parent ? relation.ParentTable : relation.ChildTable;
        //            if (!(relation.RelationName == dt1.TableName) || !dt1.TableName.StartsWith(dt.TableName + "_"))
        //            {
        //                var selectSql = GetSelectSql(dt1, new string[0]);
        //                if (selectSql.Length != 0)
        //                {
        //                    var qb1 = new QueryBuilder(selectSql);
        //                    var additionalWhere = GetDateCriteriaClause(qb1.TableList, timeColumn, begtime, endtime, addIsNull);
        //                    if (additionalWhere.Length == 0)
        //                        additionalWhere = GetDateCriteriaClauseFromRelation(dt1, timeColumn, begtime, endtime, addIsNull, direction, qb1);
        //                    if (additionalWhere.Length > 0)
        //                    {
        //                        foreach (var str in qb.TableList)
        //                        {
        //                            if (qb1.TableList.Contains(str))
        //                                qb1.TableList.Remove(str);
        //                        }
        //                        if (qb1.TableList.Count > 0)
        //                        {
        //                            qb1.Select = "SELECT 1";
        //                            qb1.Where = "";
        //                            qb1.AddWhereClause(DataSetHelper.GetJoinSql(relation));
        //                            qb1.AddWhereClause(GetJoinSql(qb1.TableList));
        //                            qb1.AddWhereClause(additionalWhere);
        //                            additionalWhere = "EXISTS ( " + qb1.Query + " )";
        //                        }
        //                        list.Add(additionalWhere);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    var str1 = "";
        //    if (list.Count > 0)
        //    {
        //        str1 = string.Join(" OR ", list.ToArray());
        //        if (list.Count > 1)
        //            str1 = "(" + str1 + ")";
        //    }
        //    return str1;
        //}

        public static bool Exists(string objectname)
        {
            var strArray = objectname.Split(new[]{'.'});
            return strArray.Length != 2 ? DatabaseContent.GetDbColumns(objectname.Trim()).Length > 0 : DatabaseContent.GetDbColumn(strArray[0].Trim(), strArray[1].Trim()) != null;
        }

        public static DateTime GetServerDate()
        {
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            using (var newConnection = GetNewConnection())
            {
                using (var command = newConnection.CreateCommand())
                {
                    try
                    {
                        var commandText = DbSyntax == DatabaseType.ORACLE ? "SELECT sysdate FROM dual" : "SELECT DISTINCT getdate() FROM systypes";
                        PrepareCommand(command, newConnection, null, CommandType.Text, commandText, null);
                        WriteSqlLog(commandText, 2);
                        return (DateTime)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        WriteSqlLog(ex.Message, 2);
                        return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    }
                    finally
                    {
                        newConnection.Close();
                    }
                }
            }
        }

        public static void WriteSqlLog(DbCommand dbCommand)
        {
            if (!ApplicationConfig.SqlLogEnabled)
                return;
            WriteSqlLog(GetCommandText(dbCommand), null, 0);
        }

        public static void WriteSqlLog(string commandText)
        {
            if (!ApplicationConfig.SqlLogEnabled)
                return;
            WriteSqlLog(commandText, null, 0);
        }

        public static void WriteSqlLog(DbCommand dbCommand, int logLevel)
        {
            if (!ApplicationConfig.SqlLogEnabled || ApplicationConfig.SqlLogLevel < logLevel)
                return;
            WriteSqlLog(GetCommandText(dbCommand), null, logLevel);
        }

        public static void WriteSqlLog(string commandText, int logLevel)
        {
            if (!ApplicationConfig.SqlLogEnabled || ApplicationConfig.SqlLogLevel < logLevel)
                return;
            WriteSqlLog(commandText, null, logLevel);
        }

        private static void WriteSqlLog(string commandText, IEnumerable<DbParameter> commandParameters, int logLevel)
        {
            if (!ApplicationConfig.SqlLogEnabled)
                return;
            if (ApplicationConfig.SqlLogLevel < logLevel)
                return;

            var stringBuilder = new StringBuilder(commandText);
            if (commandParameters != null)
            {
                foreach (var dbParameter in commandParameters)
                    stringBuilder.Append(" ")
                        .Append(dbParameter.ParameterName)
                        .Append(" = ")
                        .Append(SqlValue(dbParameter.Value));
            }
            lock (SqlLogFile)
            {
                using (var resource1 = SqlLogFile.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var resource0 = new StreamWriter(resource1))
                        resource0.WriteLine(
                            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified)
                                .ToString("yyyy-MM-dd HH:mm:ss.ff") + "\t" + stringBuilder);
                }
            }
        }

        public static string GetCommandText(DbCommand dbCommand)
        {
            if (dbCommand == null)
                return string.Empty;
            var type = dbCommand.GetType();
            var str1 = dbCommand.CommandText;
            foreach (DbParameter dbParameter in dbCommand.Parameters)
            {
                var str2 = dbParameter.ParameterName;
                if (type.Name == "OleDbCommand")
                    str2 = "?";
                else if (type.Name == "OracleCommand")
                    str2 = ":" + str2;
                var length = str1.IndexOf(str2, StringComparison.Ordinal);
                if (length >= 0)
                    str1 = str1.Substring(0, length) + SqlValue(dbParameter.Value) + str1.Substring(length + str2.Length);
            }
            return str1;
        }

        public static string GetDbType(DatabaseSchema.DbColumnRow colRow, DatabaseType targetSyntax)
        {
            return GetDbType(colRow.DbType, colRow.DbLen, colRow.DbPrecision, targetSyntax);
        }

        public static string GetDbType(string dbtype, int dblen, int dbprecision, DatabaseType targetSyntax)
        {
            var length = dbtype.IndexOf(",", StringComparison.Ordinal);
            if (length > 0)
                dbtype = dbtype.Substring(0, length);
            if (dbtype == "identity")
            {
                switch (targetSyntax)
                {
                    case DatabaseType.MSSQL:
                        dbtype = "int identity";
                        break;
                    case DatabaseType.ORACLE:
                        dbtype = "number(8,0)";
                        break;
                }
            }
            else if (dbtype.StartsWith("date"))
                dbtype = targetSyntax != DatabaseType.ORACLE ? "datetime" : "date";
            else if (dbtype.StartsWith("bool"))
                dbtype = targetSyntax != DatabaseType.ORACLE ? "bit" : "number(1,0)";
            else if (dbtype.StartsWith("int"))
                dbtype = targetSyntax != DatabaseType.ORACLE ? "int" : "number(8,0)";
            else if (dbtype == "number")
            {
                if (targetSyntax == DatabaseType.ORACLE)
                    dbtype = "number(" + dblen + "," + (string)(object)dbprecision + ")";
                else
                    dbtype = "decimal(" + dblen + "," + (string)(object)dbprecision + ")";
            }
            else if (dbtype == "varchar")
                dbtype = targetSyntax != DatabaseType.ORACLE ? "varchar(" + dblen + ")" : "varchar2(" + dblen + ")";
            else if (dbtype == "nvarchar")
                dbtype = targetSyntax != DatabaseType.ORACLE ? "nvarchar(" + dblen + ")" : "nvarchar2(" + dblen + ")";
            else if (dbtype == "text")
                dbtype = targetSyntax != DatabaseType.ORACLE ? "text" : "clob";
            else if (dbtype == "ntext")
                dbtype = targetSyntax != DatabaseType.ORACLE ? "ntext" : "nclob";
            else if (dbtype == "image")
                dbtype = targetSyntax != DatabaseType.ORACLE ? "image" : "blob";
            return dbtype;
        }

        public static string ValidateConnection(string connectionString)
        {
            return ValidateConnection(new ConnectionInfo(connectionString));
        }

        public static string ValidateConnection(ConnectionInfo connInfo)
        {
            try
            {
                using (var newConnection1 = GetNewConnection(connInfo))
                {
                    connInfo.Pooling = false;
                    using (var newConnection2 = GetNewConnection(connInfo))
                    {
                        OpenConnection(newConnection2);
                        newConnection2.Close();
                    }
                    OpenConnection(newConnection1);
                    using (var newCommand = GetNewCommand(connInfo.DbSyntax == DatabaseType.ORACLE ? "SELECT sysdate FROM dual" : "SELECT DISTINCT getdate() FROM systypes", newConnection1))
                        newCommand.ExecuteScalar();
                    newConnection1.Close();
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void OpenConnection(DbConnection conn)
        {
            OpenConnection(conn, ApplicationConfig.DbConnectInfo);
        }

        public static void OpenConnection(DbConnection conn, ConnectionInfo ci)
        {
            conn.Open();
            if (ci == null || ci.DbSyntax != DatabaseType.ORACLE || ci.UserEqualsSchema)
                return;
            AlterOracleSession(conn, ApplicationConfig.DbConnectInfo.Database);
        }

        //private static void OnOracleFailover(Type eventType, object[] args)
        //{
        //    var conn = (DbConnection)args[0];
        //    var obj1 = args[1];
        //    var obj2 = args[2];
        //    var type1 = obj1.GetType();
        //    var property = type1.GetProperty("FailoverEvent");
        //    var obj3 = property.GetValue(obj1, new object[0]);
        //    var name = Enum.GetName(property.PropertyType, obj3);
        //    var fullName = type1.Assembly.FullName;
        //    var type2 = Type.GetType(type1.FullName.Replace("FailoverEvent", "FailoverReturnCode") + "," + fullName);
        //    switch (name)
        //    {
        //        case "Begin":
        //            WriteSqlLog("Callback method called :Failover Begin");
        //            break;
        //        case "Abort":
        //            WriteSqlLog("Callback method called :Failover Aborted");
        //            break;
        //        case "End":
        //            WriteSqlLog("Callback method called :Failover End");
        //            AlterOracleSession(conn, ApplicationConfig.DbConnectInfo.Database);
        //            WriteSqlLog("Set session on re-established connection :Failover End");
        //            break;
        //        case "Error":
        //            WriteSqlLog("Failover Error -Sleeping and Retrying to connect to database");
        //            Enum.Parse(type2, "Retry");
        //            break;
        //        case "Reauth":
        //            WriteSqlLog("Callback method called :Failover reauthenticating");
        //            break;
        //        default:
        //            WriteSqlLog("Bad Failover");
        //            break;
        //    }
        //    Enum.Parse(type2, "Success");
        //}

        private static void AlterOracleSession(DbConnection conn, string schemaname)
        {
            using (var command = conn.CreateCommand())
            {
                try
                {
                    var commandText = "alter session set current_schema=" + schemaname;
                    PrepareCommand(command, conn, null, CommandType.Text, commandText, null);
                    WriteSqlLog(command, 2);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    WriteSqlLog(ex.Message, 2);
                }
            }
        }

        public static string ChangeDbPassword(string newPassword)
        {
            var commandText = string.Empty;
            var dbConnectInfo = ApplicationConfig.DbConnectInfo;
            var str = dbConnectInfo.Password;
            if (str == null || str.Trim().Length == 0)
                str = "null";
            else if (DbSyntax == DatabaseType.MSSQL)
                str = SqlValue(str);
            if (newPassword == null || newPassword.Trim().Length == 0)
                newPassword = "null";
            else if (DbSyntax == DatabaseType.MSSQL)
                newPassword = SqlValue(newPassword);
            switch (DbSyntax)
            {
                case DatabaseType.MSSQL:
                    commandText = "dbo.sp_password " + str + ", " + newPassword;
                    break;
                case DatabaseType.ORACLE:
                    commandText = "ALTER USER " + dbConnectInfo.UserId + " IDENTIFIED BY \"" + newPassword + "\"";
                    break;
            }
            try
            {
                ExecuteNonQuery(commandText);
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static DatabaseType GetDbSyntax(DbConnection conn)
        {
            var type = conn.GetType();
            if (type.Name == "SqlConnection")
                return DatabaseType.MSSQL;
            return type.Name.IndexOf("Oracle", StringComparison.Ordinal) >= 0 ? DatabaseType.ORACLE : new ConnectionInfo(conn.ConnectionString).DbSyntax;
        }

        public static string GetSysConfig(string configKey)
        {
            var stringToUnzip = RetrieveUserConfig("SYSTEM", configKey);
            if (stringToUnzip.Length > 0)
                stringToUnzip = CompressionHelper.Unzip(stringToUnzip);
            return stringToUnzip;
        }

        public static string RetrieveUserConfig(string userId, string configKey)
        {
            var str1 = "";
            var newConnection = GetNewConnection();
            OpenConnection(newConnection);
            var str2 = "SELECT userid, configkey, configvalue FROM userconfig WHERE (userid='SYSTEM' or lower(userid) = " + SqlChar(userId.ToLower()) + ") AND lower(configkey) = " + SqlChar(configKey.ToLower());
            var newCommand = GetNewCommand(str2, newConnection);
            WriteSqlLog(str2);
            try
            {
                var dbDataReader = newCommand.ExecuteReader();
                if (dbDataReader.Read())
                {
                    str1 = (string)dbDataReader[2];
                    if ((string)dbDataReader[0] == "SYSTEM" && dbDataReader.Read())
                        str1 = (string)dbDataReader[2];
                }
                dbDataReader.Close();
            }
            catch (Exception ex)
            {
                WriteSqlLog(ex.Message);
            }
            finally
            {
                newConnection.Close();
            }
            return str1;
        }

        public static int CascadeDelete(string tableName, SelectCriteria criteria)
        {
            if (DatabaseContent.GetDbTable(tableName) == null)
                return 0;
            var criteriaClause = GetCriteriaClause(new List<string> {tableName}, criteria);
            var selectSql = "SELECT 1 FROM " + tableName;
            if (criteriaClause.Length > 0)
                selectSql = selectSql + " WHERE " + criteriaClause;
            var list = CascadeDelete(DatabaseContent.GetDbContent(), tableName, selectSql);
            var str = "";
            using (var newConnection = GetNewConnection())
            {
                newConnection.Open();
                var transaction = newConnection.BeginTransaction();
                try
                {
                    foreach (var commandText in list)
                        ExecuteNonQuery(transaction, commandText);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                    transaction.Rollback();
                }
                newConnection.Close();
            }
            if (str.Length <= 0)
                return list.Count;
            return -1;
        }

        public static string GetCriteriaClause(List<string> tableList, SelectCriteria criteria)
        {
            var addIsNull = criteria.DateColumn.ToLower() == "timerange" || criteria.DateColumn.ToLower() == "time range" || criteria.DateColumn.ToLower() == "daterange" || criteria.DateColumn.ToLower() == "date range";
            var str1 = GetDateCriteriaClause(tableList, criteria.DateColumn, criteria.BegTime, criteria.EndTime, addIsNull);
            for (var index = 0; index < criteria.DatabaseCriteria.Length; ++index)
            {
                var databaseCriteria1 = criteria.DatabaseCriteria[index];
                DatabaseCriteria.AdjustParen(databaseCriteria1);
                var str2 = "";
                if (databaseCriteria1.DbValue != null && (!string.IsNullOrEmpty(databaseCriteria1.DbColumn) && databaseCriteria1.DbValue != null || databaseCriteria1.DbValue.Length > 0))
                    str2 = GetDbCriteriaClause(tableList, databaseCriteria1);
                if (str2.Length == 0)
                {
                    if (databaseCriteria1.OpenParen.Length > 0)
                    {
                        if (index + 1 >= criteria.DatabaseCriteria.Length) continue;
                        criteria.DatabaseCriteria[index + 1].LogOperator = databaseCriteria1.LogOperator;
                        var databaseCriteria2 = criteria.DatabaseCriteria[index + 1];
                        var str3 = databaseCriteria2.OpenParen + databaseCriteria1.OpenParen;
                        databaseCriteria2.OpenParen = str3;
                    }
                    else if (databaseCriteria1.CloseParen.Length > 0)
                        str1 = str1 + databaseCriteria1.CloseParen;
                }
                else
                {
                    var str3 = databaseCriteria1.LogOperator + " " + databaseCriteria1.OpenParen + str2 + databaseCriteria1.CloseParen;
                    str1 = (str1.Length <= 0 || databaseCriteria1.LogOperator != "" ? str1 + " " : str1 + " AND ") + str3;
                }
            }
            return str1;
        }

        private static List<string> CascadeDelete(DatabaseSchema dbContent, string tableName, string selectSql)
        {
            var list1 = new List<string>();
            var dbColumnRowArray =
                (DatabaseSchema.DbColumnRow[])
                    dbContent.DbColumn.Select(Expr.Format("DbTable={0} AND DbPrimaryKey=1", new object[] {tableName}),
                        "DbSysGen");
            if (dbColumnRowArray.Length > 0)
            {
                var strArray1 = new string[dbColumnRowArray.Length];
                for (var index = 0; index < dbColumnRowArray.Length; ++index)
                    strArray1[index] = dbColumnRowArray[index].DbColumn;
                var dbFkey = dbContent.DbFKey;
                var filterExpression = Expr.Format("DbRefTable={0}", new object[]{tableName});
                foreach (var dbFkeyRow in (DatabaseSchema.DbFKeyRow[])dbFkey.Select(filterExpression))
                {
                    var strArray2 = dbFkeyRow.DbFKeyColumn.Split(new[]{','});
                    if (strArray2.Length == dbColumnRowArray.Length)
                    {
                        var queryBuilder = new QueryBuilder(selectSql);
                        queryBuilder.TableList.Add(dbFkeyRow.DbFKeyTable);
                        for (var index = 0; index < dbColumnRowArray.Length; ++index)
                            queryBuilder.AddWhereClause(tableName + "." + strArray1[index] + "=" + dbFkeyRow.DbFKeyTable + "." + strArray2[index]);
                        var list2 = CascadeDelete(dbContent, dbFkeyRow.DbFKeyTable, queryBuilder.Query);
                        list1.AddRange(list2);
                    }
                }
            }
            var queryBuilder1 = new QueryBuilder(selectSql);
            queryBuilder1.TableList.Remove(tableName);
            if (queryBuilder1.TableList.Count == 0)
                list1.Add("DELETE " + tableName + queryBuilder1.Where);
            else
                list1.Add("DELETE " + tableName + " WHERE EXISTS (SELECT 1" + queryBuilder1.From + queryBuilder1.Where + ")");
            return list1;
        }

        public static void InitializeDataSet(this DataSet ds)
        {
            ds.CaseSensitive = DbSyntax == DatabaseType.ORACLE;
            ds.EnforceConstraints = false;
            foreach (DataTable dataTable in ds.Tables)
            {
                var list = new List<DataColumn>(dataTable.PrimaryKey);
                foreach (DataColumn dc in dataTable.Columns)
                {
                    var dbColumn = DatabaseModel.GetDbColumn(dc.GetDbTable(), dc.GetDbColumn());

                    if (dbColumn != null && !dbColumn.IsLabelNull() && !string.IsNullOrEmpty(dbColumn.Label))
                        dc.Caption = dbColumn.Label;
                    if (!dc.AllowDBNull)
                        dc.AllowDBNull = true;
                    if (dc.ColumnName == "surrogate" && list.Contains(dc))
                    {
                        dc.AutoIncrement = true;
                        dc.AutoIncrementSeed = 0L;
                        dc.AutoIncrementStep = -1L;
                    }
                    else if (dbColumn != null && dbColumn.DbDefault.Length > 0)
                        dc.DefaultValue = DataSetHelper.GetDefaultValue(dbColumn.DbDefault, dc.DataType);
                    else if (dc.DataType == typeof (bool))
                        dc.DefaultValue = false;
                    else if (list.Contains(dc))
                    {
                        if (dc.AutoIncrement) continue;
                        if (dc.DataType == typeof (string))
                            dc.DefaultValue = string.Empty;
                        else if (dc.DataType == typeof (bool))
                            dc.DefaultValue = false;
                        else if (dc.DataType == typeof (DateTime))
                            dc.DefaultValue = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
                        else if (dc.IsNumericColumn())
                            dc.DefaultValue = 0;
                    }
                }
            }
            foreach (DataRelation dataRelation in ds.Relations)
            {
                var relationName = dataRelation.RelationName;
                if (!dataRelation.ChildTable.Constraints.Contains(relationName))
                    dataRelation.ChildTable.Constraints.Add(new ForeignKeyConstraint(relationName, dataRelation.ParentColumns, dataRelation.ChildColumns)
                    {
                        AcceptRejectRule = AcceptRejectRule.None,
                        DeleteRule = Rule.Cascade,
                        UpdateRule = Rule.Cascade
                    });
            }
        }

        public static bool IsPkViolation(Exception e)
        {
            var errorCode = GetErrorCode(e);
            return DbSyntax == DatabaseType.ORACLE && errorCode == 1 || DbSyntax == DatabaseType.MSSQL && errorCode == 2627;
        }

        public static int GetErrorCode(Exception e)
        {
            var propertyInfo = e.GetType().GetProperty("Code") ?? e.GetType().GetProperty("Number");
            return !(propertyInfo == null) ? Convert.ToInt32(propertyInfo.GetValue(e, null)) : 0;
        }

        public static string CheckSqlReservedWord(string word)
        {
            return CheckSqlReservedWord(word, QuoteType.DoubleQuote);
        }

        public static string CheckSqlReservedWord(string word, QuoteType quoteType)
        {
            if (SqlReservedWords.IndexOf(word.ToUpper()) > 0)
            {
                if (DbSyntax == DatabaseType.ORACLE)
                    word = word.ToUpper();
                switch (quoteType)
                {
                    case QuoteType.SingleQuote:
                        word = "'" + word + "'";
                        break;
                    case QuoteType.DoubleQuote:
                        word = "\"" + word + "\"";
                        break;
                    case QuoteType.SquareBracket:
                        word = "[" + word + "]";
                        break;
                }
            }
            return word;
        }

        public static string RemoveReservedWordQuote(string word)
        {
            if ((!word.StartsWith("\"") || !word.EndsWith("\"")) && (!word.StartsWith("[") || !word.EndsWith("]")) &&
                (!word.StartsWith("'") || !word.EndsWith("'"))) return word;
            var word1 = word.Substring(1, word.Length - 2);
            if (word.Equals(CheckSqlReservedWord(word1), StringComparison.OrdinalIgnoreCase))
                word = word1;
            return word;
        }

        public static bool TableContainsRows(string tableName)
        {
            if (!Exists(tableName))
                return false;
            switch (DbSyntax)
            {
                case DatabaseType.MSSQL:
                    return ExecuteSelect(" SELECT TOP 1 1 FROM " + tableName).Any();
                case DatabaseType.ORACLE:
                    return ExecuteSelect(" SELECT 1 FROM " + tableName + " WHERE rownum < 2 ").Any();
                default:
                    var obj = ExecuteScalar(" SELECT COUNT(*) FROM " + tableName);
                    if (obj != null)
                        return Convert.ToInt32(obj) > 0;
                    return false;
            }
        }

        private enum DbConnectionOwnership
        {
            Internal,
            External,
        }

        private enum JoinDirection
        {
            Parent,
            Child,
            Both,
        }

        public enum QuoteType
        {
            SingleQuote,
            DoubleQuote,
            SquareBracket,
        }
    }
}
