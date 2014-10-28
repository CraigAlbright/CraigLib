using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CraigLib.Data
{
    public class CommandBuilder
    {
        private string _quotePrefix = string.Empty;
        private string _quoteSuffix = string.Empty;
        private string _creationName = string.Empty;
        private string _updateTable = string.Empty;
        private DbDataAdapter _dataAdapter;
        private List<DbCommand> _textCommands;
        private List<DbCommand> _imageCommands;

        public DbDataAdapter DataAdapter
        {
            get
            {
                return _dataAdapter;
            }
            set
            {
                _dataAdapter = value;
                RefreshSchema();
            }
        }

        public string QuotePrefix
        {
            get
            {
                return _quotePrefix;
            }
            set
            {
                _quotePrefix = value;
                RefreshSchema();
            }
        }

        public string QuoteSuffix
        {
            get
            {
                return _quoteSuffix;
            }
            set
            {
                _quoteSuffix = value;
                RefreshSchema();
            }
        }

        public string CreationName
        {
            get
            {
                return _creationName;
            }
            set
            {
                _creationName = value;
                RefreshSchema();
            }
        }

        public string UpdateTable
        {
            get
            {
                return _updateTable;
            }
            set
            {
                _updateTable = value;
                RefreshSchema();
            }
        }

        public List<string> ConcurrentCheckColumns { get; set; }

        public List<DbCommand> TextCommands
        {
            get
            {
                return _textCommands;
            }
        }

        public List<DbCommand> ImageCommands
        {
            get
            {
                return _imageCommands;
            }
        }

        public CommandBuilder(DbDataAdapter dataAdapter)
            : this(dataAdapter, string.Empty)
        {
        }

        public CommandBuilder(DbDataAdapter dataAdapter, string updateTable)
        {
            _dataAdapter = dataAdapter;
            _updateTable = updateTable;
            _textCommands = new List<DbCommand>();
            _imageCommands = new List<DbCommand>();
        }

        public void RefreshSchema()
        {
            GetInsertCommand();
            GetUpdateCommand();
            GetDeleteCommand();
        }

        public DbCommand GetInsertCommand()
        {
            if (_dataAdapter == null)
                return null;
            var selectCommand = _dataAdapter.SelectCommand;
            if (selectCommand == null)
                return null;
            var newCommand = DatabaseHelper.GetNewCommand(string.Empty, selectCommand.Connection, selectCommand.Transaction);
            var queryBuilder = new QueryBuilder(selectCommand.CommandText);
            if (queryBuilder.TableList.Count == 0)
                return null;
            var list1 = new List<string>();
            var list2 = new List<string>();
            var tablename = (queryBuilder.TableList.Contains(_updateTable) ? _updateTable : queryBuilder.TableList[0]).ToLower();
            var dbParameter1 = (DbParameter)null;
            var dbParameter2 = (DbParameter)null;
            var dbParameter3 = (DbParameter)null;
            for (var index = 0; index < queryBuilder.ColumnList.Count; ++index)
            {
                var sqltblName = tablename;
                var sqlcolName = queryBuilder.ColumnList[index].ToLower();
                var sourceName = sqlcolName;
                ParseColumnSql(ref sqltblName, ref sqlcolName, ref sourceName);
                if (!(sqltblName != tablename) && !(sourceName == "groupdesc") && (!(sqlcolName == "revisionname") && !(sqlcolName == "revisiondate")))
                {
                    DatabaseSchema.DbColumnRow dbColumn = DatabaseContent.GetDbColumn(sqltblName, sqlcolName);
                    if (dbColumn != null)
                    {
                        var str = sqltblName + "." + sqlcolName;
                        if (!list1.Contains(str))
                        {
                            list1.Add(str);
                            var parameter = newCommand.CreateParameter();
                            DatabaseHelper.SetParameterName(parameter, sourceName);
                            parameter.Direction = ParameterDirection.Input;
                            DatabaseHelper.SetParameterDbType(parameter, dbColumn);
                            parameter.SourceColumn = sourceName;
                            parameter.SourceVersion = DataRowVersion.Current;
                            newCommand.Parameters.Add(parameter);
                            list2.Add(DatabaseHelper.GetParameterSql(parameter, sourceName));
                            if (sqlcolName == "creationname")
                                dbParameter1 = parameter;
                            else if (sqlcolName == "creationdate")
                                dbParameter2 = parameter;
                            else if (sqlcolName == "surrogate")
                                dbParameter3 = parameter;
                        }
                    }
                }
            }
            if (dbParameter3 == null && DatabaseContent.GetDbColumn(tablename, "surrogate") != null)
            {
                list1.Add(tablename + ".surrogate");
                var parameter = newCommand.CreateParameter();
                DatabaseHelper.SetParameterName(parameter, "surrogate");
                parameter.Direction = ParameterDirection.Input;
                parameter.SourceColumn = "surrogate";
                parameter.SourceVersion = DataRowVersion.Current;
                newCommand.Parameters.Add(parameter);
                list2.Add(DatabaseHelper.GetParameterSql(parameter, "surrogate"));
            }
            if (dbParameter1 == null && DatabaseContent.GetDbColumn(tablename, "creationname") != null)
            {
                list1.Add(tablename + ".creationname");
                var parameter = newCommand.CreateParameter();
                DatabaseHelper.SetParameterName(parameter, "creationname");
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                    parameter.Value = _creationName.Length > 0 ? _creationName : windowsIdentity.Name;
                newCommand.Parameters.Add(parameter);
                list2.Add(DatabaseHelper.GetParameterSql(parameter, "creationname"));
            }
            if (dbParameter2 == null && DatabaseContent.GetDbColumn(tablename, "creationdate") != null)
            {
                list1.Add(tablename + ".creationdate");
                var parameter = newCommand.CreateParameter();
                DatabaseHelper.SetParameterName(parameter, "creationdate");
                parameter.Value = DatabaseHelper.GetServerDate();
                newCommand.Parameters.Add(parameter);
                list2.Add(DatabaseHelper.GetParameterSql(parameter, "creationdate"));
            }
            var str1 = string.Join(", ", list1.ToArray());
            var str2 = string.Join(", ", list2.ToArray());
            var str3 = "INSERT INTO " + tablename + " (" + str1 + ") VALUES (" + str2 + ") ";
            newCommand.CommandText = str3;
            _dataAdapter.InsertCommand = newCommand;
            return newCommand;
        }

        public DbCommand GetUpdateCommand()
        {
            _textCommands.Clear();
            _imageCommands.Clear();
            if (_dataAdapter == null)
                return null;
            var selectCommand = _dataAdapter.SelectCommand;
            if (selectCommand == null)
                return null;
            var newCommand1 = DatabaseHelper.GetNewCommand(string.Empty, selectCommand.Connection, selectCommand.Transaction);
            var qb = new QueryBuilder(selectCommand.CommandText);
            if (qb.TableList.Count == 0)
                return null;
            var str1 = (qb.TableList.Contains(_updateTable) ? _updateTable : qb.TableList[0]).ToLower();
            var list1 = new List<string>();
            var list2 = new List<string>();
            var list3 = new List<string>();
            var dbParameter1 = (DbParameter)null;
            var dbParameter2 = (DbParameter)null;
            var list4 = new List<DbParameter>();
            var flag = IsPKeyMissing(qb, str1);
            var list5 = new List<DatabaseSchema.DbColumnRow>();
            var list6 = new List<DatabaseSchema.DbColumnRow>();
            for (var index = 0; index < qb.ColumnList.Count; ++index)
            {
                var sqltblName = str1;
                var sqlcolName = qb.ColumnList[index].ToLower();
                var sourceName = sqlcolName;
                ParseColumnSql(ref sqltblName, ref sqlcolName, ref sourceName);
                if (!(sqltblName != str1) && !(sourceName == "groupdesc") && (!(sqlcolName == "creationname") && !(sqlcolName == "creationdate")))
                {
                    DatabaseSchema.DbColumnRow dbColumn = DatabaseContent.GetDbColumn(sqltblName, sqlcolName);
                    if (dbColumn != null)
                    {
                        var str2 = sqltblName + "." + sqlcolName;
                        if (!list1.Contains(str2))
                        {
                            list1.Add(str2);
                            var parameter1 = newCommand1.CreateParameter();
                            DatabaseHelper.SetParameterName(parameter1, sourceName);
                            parameter1.Direction = ParameterDirection.Input;
                            DatabaseHelper.SetParameterDbType(parameter1, dbColumn);
                            parameter1.SourceColumn = sourceName;
                            parameter1.SourceVersion = DataRowVersion.Current;
                            newCommand1.Parameters.Add(parameter1);
                            list2.Add(str2 + " = " + DatabaseHelper.GetParameterSql(parameter1, sourceName));
                            if (sqlcolName == "revisionname")
                                dbParameter1 = parameter1;
                            else if (sqlcolName == "revisiondate")
                                dbParameter2 = parameter1;
                            if (dbColumn.DbPrimaryKey == 1 || flag || ConcurrentCheckColumns != null && ConcurrentCheckColumns.Contains(sqlcolName))
                            {
                                var parameterName1 = sourceName + "0";
                                var parameter2 = newCommand1.CreateParameter();
                                DatabaseHelper.SetParameterName(parameter2, parameterName1);
                                parameter2.Direction = ParameterDirection.Input;
                                DatabaseHelper.SetParameterDbType(parameter2, dbColumn);
                                parameter2.SourceColumn = sourceName;
                                parameter2.SourceVersion = DataRowVersion.Original;
                                list4.Add(parameter2);
                                var str3 = str2 + " = " + DatabaseHelper.GetParameterSql(parameter1, parameterName1);
                                if (dbColumn.DbPrimaryKey != 1 && dbColumn.DbNull == 1)
                                {
                                    var parameterName2 = parameterName1 + "9";
                                    var dataParameter = DatabaseHelper.CloneParameter(parameter2);
                                    DatabaseHelper.SetParameterName(dataParameter, parameterName2);
                                    dataParameter.SourceColumnNullMapping = true;
                                    dataParameter.DbType = DbType.Int32;
                                    list4.Add(dataParameter);
                                    var str4 = DatabaseHelper.GetParameterSql(dataParameter, parameterName2) + " = 1 AND " + str2 + " IS NULL";
                                    str3 = "((" + str3 + ") OR (" + str4 + "))";
                                }
                                list3.Add(str3);
                            }
                        }
                    }
                }
            }
            if (dbParameter1 == null && DatabaseContent.GetDbColumn(str1, "revisionname") != null)
            {
                var parameter = newCommand1.CreateParameter();
                DatabaseHelper.SetParameterName(parameter, "revisionname");
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                    parameter.Value = windowsIdentity.Name;
                newCommand1.Parameters.Add(parameter);
                list2.Add(str1 + ".revisionname = " + DatabaseHelper.GetParameterSql(parameter, "revisionname"));
            }
            if (dbParameter2 == null && DatabaseContent.GetDbColumn(str1, "revisiondate") != null)
            {
                var parameter = newCommand1.CreateParameter();
                DatabaseHelper.SetParameterName(parameter, "revisiondate");
                parameter.Value = DatabaseHelper.GetServerDate();
                newCommand1.Parameters.Add(parameter);
                list2.Add(str1 + ".revisiondate = " + DatabaseHelper.GetParameterSql(parameter, "revisiondate"));
            }
            foreach (var dbParameter3 in list4)
                newCommand1.Parameters.Add(dbParameter3);
            var str5 = string.Join(", ", list2.ToArray());
            var str6 = string.Join(" AND ", list3.ToArray());
            var str7 = "UPDATE " + str1 + " SET " + str5 + " WHERE " + str6;
            newCommand1.CommandText = str7;
            foreach (var colRow in list5)
            {
                var newCommand2 = DatabaseHelper.GetNewCommand(string.Empty, selectCommand.Connection, selectCommand.Transaction);
                var parameter1 = newCommand2.CreateParameter();
                DatabaseHelper.SetParameterName(parameter1, colRow.DbColumn);
                parameter1.Direction = ParameterDirection.Input;
                DatabaseHelper.SetParameterDbType(parameter1, colRow);
                parameter1.SourceColumn = colRow.DbColumn;
                parameter1.SourceVersion = DataRowVersion.Current;
                newCommand2.Parameters.Add(parameter1);
                foreach (var parameter2 in list4)
                {
                    var dbParameter3 = DatabaseHelper.CloneParameter(parameter2);
                    newCommand2.Parameters.Add(dbParameter3);
                }
                newCommand2.CommandText = "UPDATE " + colRow.DbTable + " SET " + colRow.DbColumn + " = " + DatabaseHelper.GetParameterSql(parameter1, colRow.DbColumn) + " WHERE " + str6;
                _textCommands.Add(newCommand2);
            }
            foreach (var colRow in list6)
            {
                var newCommand2 = DatabaseHelper.GetNewCommand(string.Empty, selectCommand.Connection, selectCommand.Transaction);
                var parameter1 = newCommand2.CreateParameter();
                DatabaseHelper.SetParameterName(parameter1, colRow.DbColumn);
                parameter1.Direction = ParameterDirection.Input;
                DatabaseHelper.SetParameterDbType(parameter1, colRow);
                parameter1.SourceColumn = colRow.DbColumn;
                parameter1.SourceVersion = DataRowVersion.Current;
                newCommand2.Parameters.Add(parameter1);
                foreach (var parameter2 in list4)
                {
                    var dbParameter3 = DatabaseHelper.CloneParameter(parameter2);
                    newCommand2.Parameters.Add(dbParameter3);
                }
                newCommand2.CommandText = "UPDATE " + colRow.DbTable + " SET " + colRow.DbColumn + " = " + DatabaseHelper.GetParameterSql(parameter1, colRow.DbColumn) + " WHERE " + str6;
                _imageCommands.Add(newCommand2);
            }
            _dataAdapter.UpdateCommand = newCommand1;
            return newCommand1;
        }

        public DbCommand GetDeleteCommand()
        {
            if (_dataAdapter == null)
                return null;
            var selectCommand = _dataAdapter.SelectCommand;
            if (selectCommand == null)
                return null;
            var newCommand = DatabaseHelper.GetNewCommand(string.Empty, selectCommand.Connection, selectCommand.Transaction);
            var qb = new QueryBuilder(selectCommand.CommandText);
            if (qb.TableList.Count == 0)
                return null;
            var tableName = (qb.TableList.Contains(_updateTable) ? _updateTable : qb.TableList[0]).ToLower();
            var list1 = new List<string>();
            var list2 = new List<string>();
            var flag = IsPKeyMissing(qb, tableName);
            for (var index = 0; index < qb.ColumnList.Count; ++index)
            {
                var sqltblName = tableName;
                var sqlcolName = qb.ColumnList[index].ToLower();
                var sourceName = sqlcolName;
                ParseColumnSql(ref sqltblName, ref sqlcolName, ref sourceName);
                if (!(sqltblName != tableName) && !(sourceName == "groupdesc") && (!(sqlcolName == "creationname") && !(sqlcolName == "creationdate")) && (!(sqlcolName == "revisionname") && !(sqlcolName == "revisiondate")))
                {
                    DatabaseSchema.DbColumnRow dbColumn = DatabaseContent.GetDbColumn(sqltblName, sqlcolName);
                    if (dbColumn != null)
                    {
                        var str1 = sqltblName + "." + sqlcolName;
                        if (!list1.Contains(str1))
                        {
                            list1.Add(str1);
                            if (dbColumn.DbPrimaryKey == 1 || flag)
                            {
                                var parameterName1 = sourceName + "0";
                                var parameter = newCommand.CreateParameter();
                                DatabaseHelper.SetParameterName(parameter, parameterName1);
                                parameter.Direction = ParameterDirection.Input;
                                DatabaseHelper.SetParameterDbType(parameter, dbColumn);
                                parameter.SourceColumn = sourceName;
                                parameter.SourceVersion = DataRowVersion.Original;
                                newCommand.Parameters.Add(parameter);
                                var str2 = str1 + " = " + DatabaseHelper.GetParameterSql(parameter, parameterName1);
                                if (dbColumn.DbPrimaryKey != 1 && dbColumn.DbNull == 1)
                                {
                                    var parameterName2 = parameterName1 + "9";
                                    var dataParameter = DatabaseHelper.CloneParameter(parameter);
                                    DatabaseHelper.SetParameterName(dataParameter, parameterName2);
                                    dataParameter.SourceColumnNullMapping = true;
                                    dataParameter.DbType = DbType.Int32;
                                    newCommand.Parameters.Add(dataParameter);
                                    var str3 = DatabaseHelper.GetParameterSql(dataParameter, parameterName2) + " = 1 AND " + str1 + " IS NULL";
                                    str2 = "((" + str2 + ") OR (" + str3 + "))";
                                }
                                list2.Add(str2);
                            }
                        }
                    }
                }
            }
            var str4 = string.Join(" AND ", list2.ToArray());
            var str5 = "DELETE " + tableName + " WHERE " + str4;
            newCommand.CommandText = str5;
            _dataAdapter.DeleteCommand = newCommand;
            return newCommand;
        }

        private bool IsPKeyMissing(QueryBuilder qb, string tableName)
        {
            if (DatabaseContent.GetPrimaryKeys(tableName).Length == 0)
                return true;
            for (var index = 0; index < qb.ColumnList.Count; ++index)
            {
                var sqltblName = tableName;
                var sqlcolName = qb.ColumnList[index].ToLower();
                var sourceName = sqlcolName;
                ParseColumnSql(ref sqltblName, ref sqlcolName, ref sourceName);
                if (!(sqltblName != tableName))
                {
                    DatabaseSchema.DbColumnRow dbColumn = DatabaseContent.GetDbColumn(sqltblName, sqlcolName);
                    if (dbColumn != null && dbColumn.DbPrimaryKey == 1)
                        return false;
                }
            }
            return true;
        }

        private void ParseColumnSql(ref string sqltblName, ref string sqlcolName, ref string sourceName)
        {
            sqlcolName = sqlcolName.Replace("(", string.Empty);
            sqlcolName = sqlcolName.Replace(")", string.Empty);
            var length1 = sqlcolName.LastIndexOf(" ");
            if (length1 > 0)
            {
                sourceName = sqlcolName.Substring(length1 + 1);
                sourceName = DatabaseHelper.RemoveReservedWordQuote(sourceName);
                sqlcolName = sqlcolName.Substring(0, length1);
            }
            var length2 = sqlcolName.LastIndexOf(".");
            if (length2 <= 0)
                return;
            sqltblName = sqlcolName.Substring(0, length2);
            sqlcolName = sqlcolName.Substring(length2 + 1);
            if (length1 >= 0)
                return;
            sourceName = sqlcolName;
        }
    }
}
