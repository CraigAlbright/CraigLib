using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Data
{
    public class DatabaseAdapter : IDisposable
    {
        private bool _pushchanges = true;
        private readonly IsolationLevel _isolatationlevel = IsolationLevel.ReadCommitted;
        private readonly bool _autoCommit = true;
        private int _commandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeOut"]);
        private bool _vote = true;
        private bool _useevents = true;
        private string _creationName = string.Empty;
        private bool _acceptChangesDuringFill = true;
        private readonly ConnectionInfo _connectInfo;
        private bool _disposed;
        private readonly bool _catchDbError;
        private bool _usesnaphot;
        private DbCommand _updatingCommand;

        public DbConnection Connection { get; private set; }

        public DbTransaction Transaction { get; private set; }

        public bool AutoCommit
        {
            get
            {
                return _autoCommit;
            }
        }

        public bool CatchDbError
        {
            get
            {
                return _catchDbError;
            }
        }

        public bool IgnoreSecuriyPermission { get; set; }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        public bool Vote
        {
            get
            {
                return _vote;
            }
        }

        public bool Useevents
        {
            get
            {
                return _useevents;
            }
            set
            {
                _useevents = value;
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
            }
        }

        public bool AcceptChangesDuringFill
        {
            get
            {
                return _acceptChangesDuringFill;
            }
            set
            {
                _acceptChangesDuringFill = value;
            }
        }

        public bool PushChanges
        {
            get
            {
                return _pushchanges;
            }
            set
            {
                _pushchanges = value;
            }
        }

        public bool UseSnapshot
        {
            get
            {
                return _usesnaphot;
            }
            set
            {
                if (value && _connectInfo.AllowSnapshot)
                    _usesnaphot = true;
                else
                    _usesnaphot = false;
            }
        }

        public DbCommand UpdatingCommand
        {
            get
            {
                return _updatingCommand;
            }
            set
            {
                _updatingCommand = value;
            }
        }

        public DatabaseAdapter(IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(ApplicationConfig.DbConnectInfo, true, false, messageUpdater, workflowUpdater)
        {
        }

        public DatabaseAdapter(bool autoCommit, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(ApplicationConfig.DbConnectInfo, autoCommit, false, messageUpdater, workflowUpdater)
        {
        }

        public DatabaseAdapter(IsolationLevel ilevel, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(ApplicationConfig.DbConnectInfo, true, false, messageUpdater, workflowUpdater)
        {
            _isolatationlevel = ilevel;
        }

        public DatabaseAdapter(bool autoCommit, bool catchDbError, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(ApplicationConfig.DbConnectInfo, autoCommit, catchDbError, messageUpdater, workflowUpdater)
        {
        }

        public DatabaseAdapter(ConnectionInfo connInfo, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(connInfo, true, false, messageUpdater, workflowUpdater)
        {
        }

        public DatabaseAdapter(ConnectionInfo connInfo, bool autoCommit, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
            : this(connInfo, autoCommit, false, messageUpdater, workflowUpdater)
        {
        }

        public DatabaseAdapter(ConnectionInfo connInfo, bool autoCommit, bool catchDbError, IDbAdptUpdater messageUpdater, IDbAdptUpdater workflowUpdater)
        {
            _connectInfo = connInfo;
            Connection = DatabaseHelper.GetNewConnection(connInfo);
            _autoCommit = autoCommit;
            _catchDbError = catchDbError;
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null && (connInfo.AllowSnapshot && windowsIdentity.Name.Equals("System")))
                _usesnaphot = true;
            var connectionExists = DatabaseContent.GetDbTable("connection") != null;
            var messageExists = DatabaseContent.GetDbTable("message") != null;
            _useevents = connectionExists || messageExists;
        }

        public DatabaseAdapter()
        {
            // TODO: Complete member initialization
        }

        ~DatabaseAdapter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_vote)
                    Commit();
                else
                    Rollback();
                Connection.Dispose();
                Connection = null;
            }
            _disposed = true;
        }

        public bool BeginTransaction()
        {
            if (Connection.State != ConnectionState.Open)
                DatabaseHelper.OpenConnection(Connection, _connectInfo);
            if (Transaction != null)
                return false;
            Transaction = !_usesnaphot ? (_isolatationlevel != IsolationLevel.ReadCommitted ? Connection.BeginTransaction(_isolatationlevel) : Connection.BeginTransaction()) : Connection.BeginTransaction(IsolationLevel.Snapshot);
            return true;
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                finally
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
            _vote = true;
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Rollback();
                }
                finally
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
            _vote = true;
        }

        public int Fill(DataSet dsFill)
        {
            return Fill(dsFill, (SelectCriteria)null);
        }

        public int Fill(DataSet dsFill, SelectCriteria selectCriteria)
        {
            var num = 0;
            DataSetHelper.AttachSqlLog(dsFill, "", "");
            foreach (var dataTable in dsFill.SortTables())
            {
                var selectSql = DatabaseHelper.GetSelectSql(dataTable, new string[0]);
                if (selectSql.Length != 0)
                    num = Fill(dataTable, selectSql, selectCriteria);
            }
            return num;
        }

        public int Fill(DataSet dsFill, string dtName)
        {
            var selectSql = "";
            if (dsFill.Tables.Contains(dtName))
                selectSql = DatabaseHelper.GetSelectSql(dsFill.Tables[dtName], new string[0]);
            if (selectSql.Length == 0)
                selectSql = "SELECT * FROM " + dtName;
            return Fill(dsFill, dtName, selectSql);
        }

        public int Fill(DataSet dsFill, string dtName, string selectSql)
        {
            return Fill(dsFill, dtName, selectSql, null);
        }

        public int Fill(DataSet dsFill, string dtName, string selectSql, SelectCriteria selectCriteria)
        {
            return Fill(dsFill.Tables.Contains(dtName) ? dsFill.Tables[dtName] : dsFill.Tables.Add(dtName), selectSql, selectCriteria);
        }

        public int Fill(DataTable dtFill)
        {
            return Fill(dtFill, (SelectCriteria)null);
        }

        public int Fill(DataTable dtFill, SelectCriteria selectCriteria)
        {
            var selectSql = DatabaseHelper.GetSelectSql(dtFill, new string[0]);
            if (selectSql.Length == 0)
                selectSql = "select * from " + dtFill.GetDbTable();
            return Fill(dtFill, selectSql, selectCriteria);
        }

        public int Fill(DataTable dtFill, string selectSql)
        {
            return Fill(dtFill, selectSql, null);
        }

        public int Fill(DataTable dtFill, string selectSql, SelectCriteria selectCriteria)
        {
            var dataSet = dtFill.DataSet;
            var queryBuilder = new QueryBuilder(selectSql);
            
            if (selectCriteria != null)
            {
                queryBuilder.AddSelectCriteria(selectCriteria);
                selectSql = queryBuilder.Query;
            }
            if (DatabaseHelper.DbSyntax == DatabaseType.ORACLE)
            {
                if (dataSet != null)
                    dataSet.CaseSensitive = true;
                else
                    dtFill.CaseSensitive = true;
            }
            var rows = 0;
            var flag1 = false;
            var flag2 = false;
            if (Transaction == null)
            {
                
                    BeginTransaction();
                flag2 = true;
            }
            using (var newDataAdapter = DatabaseHelper.GetNewDataAdapter(selectSql, Connection, Transaction))
            {
                try
                {
                    DatabaseHelper.WriteSqlLog(selectSql);
                    DataSetHelper.AttachSqlLog(dataSet, selectSql, "");
                    var flag3 = false;
                    if (dataSet != null)
                    {
                        flag3 = dataSet.EnforceConstraints;
                        dataSet.EnforceConstraints = false;
                    }
                    dtFill.BeginLoadData();
                    newDataAdapter.AcceptChangesDuringFill = _acceptChangesDuringFill;
                    if (Connection.State != ConnectionState.Open)
                    {
                        DatabaseHelper.OpenConnection(Connection, _connectInfo);
                        flag1 = true;
                    }
                    if (newDataAdapter.SelectCommand != null)
                        newDataAdapter.SelectCommand.CommandTimeout = _commandTimeout;
                    if (dtFill.ExtendedProperties.ContainsKey("_maxrows"))
                        rows = newDataAdapter.Fill(0, Convert.ToInt32(dtFill.ExtendedProperties["_maxrows"]), new DataTable[1]
            {
              dtFill
            });
                    else
                        rows = newDataAdapter.Fill(dtFill);
                    if (flag2)
                        Commit();
                    dtFill.EndLoadData();
                    if (dataSet != null)
                    {
                        dataSet.EnforceConstraints = flag3;
                        dataSet.SetDateTimeKind();
                    }
                    else
                        dtFill.SetDateTimeKind();
                    DataSetHelper.AttachSqlLog(dataSet, selectSql, rows.ToString());
                }
                catch (Exception ex)
                {
                    if (flag2)
                        Rollback();
                    if (flag1)
                        Connection.Close();
                    rows = -1;
                    DatabaseHelper.WriteSqlLog(ex.Message);
                    DataSetHelper.AttachSqlLog(dataSet, selectSql, ex.Message);
                    if (!_catchDbError)
                        throw new DatabaseAdapterException(ex.Message, selectSql, ex);
                    DataSetHelper.AttachDbError(dataSet, ex.Message, selectSql);
                }
               
            }
            if (flag1)
                Connection.Close();
            return rows;
        }

        public DataTable[] FillSchema(DataSet dataSet, string selectSql)
        {
            return FillSchema(dataSet, selectSql, SchemaType.Mapped);
        }

        public DataTable[] FillSchema(DataSet dataSet, string selectSql, SchemaType schemaType)
        {
            var dataTableArray = new DataTable[0];
            using (var newDataAdapter = DatabaseHelper.GetNewDataAdapter(selectSql, Connection))
            {
                var flag = false;
                if (Connection.State != ConnectionState.Open)
                {
                    DatabaseHelper.OpenConnection(Connection, _connectInfo);
                    flag = true;
                }
                dataTableArray = newDataAdapter.FillSchema(dataSet, schemaType);
                if (flag)
                    Connection.Close();
            }
            return dataTableArray;
        }

        public DataTable FillSchema(DataTable dataTable, string selectSql)
        {
            return FillSchema(dataTable, selectSql, SchemaType.Mapped);
        }

        public DataTable FillSchema(DataTable dataTable, string selectSql, SchemaType schemaType)
        {
            var dataTable1 = (DataTable)null;
            using (var newDataAdapter = DatabaseHelper.GetNewDataAdapter(selectSql, Connection))
            {
                var flag = false;
                if (Connection.State != ConnectionState.Open)
                {
                    DatabaseHelper.OpenConnection(Connection, _connectInfo);
                    flag = true;
                }
                dataTable1 = newDataAdapter.FillSchema(dataTable, schemaType);
                if (flag)
                    Connection.Close();
            }
            return dataTable1;
        }

        public int Update(params DataTable[] dtUpdates)
        {
            var selectSqls = new string[dtUpdates.Length];
            for (var index = 0; index < dtUpdates.Length; ++index)
                selectSqls[index] = DatabaseHelper.GetSelectSql(dtUpdates[index], new string[0]);
            return Update(dtUpdates, selectSqls);
        }

        public int Update(DataTable dtUpdate, bool sysGen)
        {
            var selectSql = DatabaseHelper.GetSelectSql(dtUpdate, new string[0]);
            return Update(new DataTable[1]
      {
        dtUpdate
      }, new[]
      {
        selectSql
      }, (sysGen ? 1 : 0) != 0);
        }

        public int Update(DataTable dtUpdate, string selectSql)
        {
            return Update(new[]
      {
        dtUpdate
      }, new[]
      {
        selectSql
      }, 1 != 0);
        }

        public int Update(DataTable dtUpdate, string selectSql, bool sysGen)
        {
            return Update(new[]
      {
        dtUpdate
      }, new[]
      {
        selectSql
      }, (sysGen ? 1 : 0) != 0);
        }

        public int Update(DataTable[] dtUpdates, bool sysGen)
        {
            var selectSqls = new string[dtUpdates.Length];
            for (var index = 0; index < dtUpdates.Length; ++index)
                selectSqls[index] = DatabaseHelper.GetSelectSql(dtUpdates[index], new string[0]);
            return Update(dtUpdates, selectSqls, sysGen);
        }

        public int Update(DataTable[] dtUpdates, string[] selectSqls)
        {
            return Update(dtUpdates, selectSqls, true);
        }

        public int Update(DataTable[] dtUpdates, string[] selectSqls, bool sysGen)
        {
            const bool backgroundProcess = true;
            return Update(dtUpdates, selectSqls, sysGen, backgroundProcess);
        }

        public int Update(DataTable[] dtUpdates, string[] selectSqls, bool sysGen, bool backgroundProcess)
        {
            if (!_vote)
                throw new DatabaseAdapterException("Update does not execute due to previous failed transaction votes");
            var num1 = 0;
            var dbDataAdapterArray = new DbDataAdapter[dtUpdates.Length];
            var commandBuilderArray = new CommandBuilder[dtUpdates.Length];
            var dts = new Dictionary<DataTable, string>();
            var list = (List<string>)null;
            var dataSet = (DataSet)null;
            try
            {
                BeginTransaction();
                var updatetable = new string[dtUpdates.Length];
                for (var index = 0; index < dtUpdates.Length; ++index)
                {
                    if (selectSqls[index].Length > 0)
                    {
                        var queryBuilder = new QueryBuilder(selectSqls[index]);
                        updatetable[index] = !queryBuilder.TableList.Contains(dtUpdates[index].TableName) ? (queryBuilder.TableList.Count <= 0 ? dtUpdates[index].TableName : queryBuilder.TableList[0]) : dtUpdates[index].TableName;
                        if (DatabaseContent.GetDbColumns(updatetable[index]).Length == 0)
                            updatetable[index] = string.Empty;
                    }
                    else
                        updatetable[index] = string.Empty;
                    if (sysGen)
                    {
                        DbSysGen.SetSurrogate(dtUpdates[index], updatetable[index]);
                        DbSysGen.SetSysGenValue(dtUpdates[index], dtUpdates[index].TableName, Connection, Transaction);
                        DbSysGen.SetCreationValue(dtUpdates[index], _creationName);
                        DbSysGen.SetRevisionValue(dtUpdates[index]);
                    }
                    DbSysGen.SetDbDefaultValue(dtUpdates[index], updatetable[index]);
                    dbDataAdapterArray[index] = DatabaseHelper.GetNewDataAdapter(selectSqls[index], Connection, Transaction);
                    dbDataAdapterArray[index].AcceptChangesDuringUpdate = false;
                    commandBuilderArray[index] = new CommandBuilder(dbDataAdapterArray[index], updatetable[index]);
                    if (_creationName.Length > 0)
                        commandBuilderArray[index].CreationName = _creationName;
                }
                for (var index = dtUpdates.Length - 1; index >= 0; --index)
                {
                    var dbtable = updatetable[index];
                    if (dbtable.Length != 0)
                    {
                        var dataRows = dtUpdates[index].Select("", "", DataViewRowState.Deleted);
                        var deleteSqls = dtUpdates[index].GetDeleteSqls();
                        if (dataRows.Length != 0 || deleteSqls.Length != 0)
                        {
                            dataSet = dtUpdates[index].DataSet;
                            if (dataRows.Length > 0)
                            {
                                _updatingCommand = commandBuilderArray[index].GetDeleteCommand();
                                if (_updatingCommand != null)
                                {
                                    _updatingCommand.CommandTimeout = _commandTimeout;
                                    DataSetHelper.AttachSqlLog(dataSet, _updatingCommand.CommandText, dataRows.Length.ToString());
                                }
                                num1 = dbDataAdapterArray[index].Update(dataRows);
                                _updatingCommand = null;
                            }
                            if (deleteSqls.Length > 0)
                            {
                                _updatingCommand = DatabaseHelper.GetNewCommand(string.Empty, Connection, Transaction);
                                _updatingCommand.CommandTimeout = _commandTimeout;
                                foreach (var str in deleteSqls)
                                {
                                    _updatingCommand.CommandText = str;
                                    num1 = _updatingCommand.ExecuteNonQuery();
                                    DataSetHelper.AttachSqlLog(dataSet, _updatingCommand.CommandText, num1.ToString());
                                }
                                _updatingCommand = null;
                            }
                            if (!dts.ContainsKey(dtUpdates[index]))
                                dts.Add(dtUpdates[index], updatetable[index]);
                        }
                    }
                }
                for (var index = 0; index < dtUpdates.Length; ++index)
                {
                    var dbtable = updatetable[index];
                    if (dbtable.Length != 0)
                    {
                        var dataRowArray = dtUpdates[index].Select("", "", DataViewRowState.Added);
                        if (dataRowArray.Length != 0)
                        {
                            dataSet = dtUpdates[index].DataSet;
                            if (ApplicationConfig.UseBulkCopy && _connectInfo.DbSyntax == DatabaseType.MSSQL)
                            {
                                BulkCopy.WriteToServer(Transaction, dataRowArray);
                            }
                            else
                            {
                                _updatingCommand = commandBuilderArray[index].GetInsertCommand();
                                if (_updatingCommand != null)
                                {
                                    _updatingCommand.CommandTimeout = _commandTimeout;
                                    DataSetHelper.AttachSqlLog(dataSet, _updatingCommand.CommandText, dataRowArray.Length.ToString());
                                }
                                num1 = dbDataAdapterArray[index].Update(dataRowArray);
                                foreach (var dataRow in dataRowArray)
                                    dataRow.SetAdded();
                                _updatingCommand = null;
                            }
                            if (!dts.ContainsKey(dtUpdates[index]))
                                dts.Add(dtUpdates[index], updatetable[index]);
                        }
                    }
                }
                for (var index = 0; index < dtUpdates.Length; ++index)
                {
                    var dbtable = updatetable[index];
                    if (dbtable.Length != 0)
                    {
                        var dataRows = dtUpdates[index].Select("", "", DataViewRowState.ModifiedCurrent);
                        if (dataRows.Length != 0)
                        {
                            dataSet = dtUpdates[index].DataSet;
                            commandBuilderArray[index].ConcurrentCheckColumns = list = dtUpdates[index].GetConcurrentCheckColumns();
                            _updatingCommand = commandBuilderArray[index].GetUpdateCommand();
                            if (_updatingCommand != null)
                            {
                                _updatingCommand.CommandTimeout = _commandTimeout;
                                DataSetHelper.AttachSqlLog(dataSet, _updatingCommand.CommandText, dataRows.Length.ToString());
                            }
                            num1 = dbDataAdapterArray[index].Update(dataRows);
                            list = null;
                            foreach (var dbCommand in commandBuilderArray[index].TextCommands)
                            {
                                _updatingCommand = dbCommand;
                                dbDataAdapterArray[index].UpdateCommand = dbCommand;
                                num1 = dbDataAdapterArray[index].Update(dataRows);
                            }
                            foreach (var dbCommand in commandBuilderArray[index].ImageCommands)
                            {
                                _updatingCommand = dbCommand;
                                dbDataAdapterArray[index].UpdateCommand = dbCommand;
                                num1 = dbDataAdapterArray[index].Update(dataRows);
                            }
                            _updatingCommand = null;
                            if (!dts.ContainsKey(dtUpdates[index]))
                                dts.Add(dtUpdates[index], updatetable[index]);
                        }
                    }
                }
               if (_autoCommit)
                {
                    Commit();
                    foreach (var dt in dtUpdates)
                        dt.ClearDeleteSqls();
                }

            }
            catch (DbException ex)
            {
                var commandText = DatabaseHelper.GetCommandText(_updatingCommand);
                if ((ex.Message.Contains("ORA-02292") || ex.Message.ToLower().Contains("constraint")) && commandText.ToLower().Trim().StartsWith("delete"))
                {
                    var strArray = commandText.Split(new[]
          {
            ' '
          });
                    var str1 = commandText.Split(new[]
          {
            ' '
          })[1].Trim();
                    var str2 = string.Empty;
                    var str3 = string.Empty;
                    if (commandText.ToLower().Contains("where"))
                    {
                        for (var index = 0; index < strArray.Length; ++index)
                        {
                            if (strArray[index].ToLower().Trim().Equals("where"))
                            {
                                str2 = strArray[index + 1];
                                break;
                            }
                        }
                        var num2 = commandText.LastIndexOf("=");
                        if (num2 != -1)
                            str3 = commandText.Substring(num2 + 1);
                    }
                    var dbErrorMessage = string.Format("Foreign key violation : Rows with {0}={1} cannot be deleted from table {2} as they are referenced in other tables.", str2, str3, str1);
                    SetGenericDbUpdateException(ex, dataSet, dbErrorMessage);
                }
                else
                    SetGenericDbUpdateException(ex, dataSet, ex.Message);
                if (_updatingCommand == null)
                {
                    if (!_catchDbError)
                        throw;
                }
            }
            catch (DBConcurrencyException ex)
            {
                var dbErrorMessage = ex.Message;
                if (list != null)
                    dbErrorMessage = string.Format("Record has been modified in database.  Please refresh the data first.", new object[0]);
                SetGenericDbUpdateException(ex, dataSet, dbErrorMessage);
                if (_updatingCommand == null)
                {
                    if (!_catchDbError)
                        throw;
                }
            }
            catch (Exception ex)
            {
                SetGenericDbUpdateException(ex, dataSet, ex.Message);
                if (_updatingCommand == null)
                {
                    if (!_catchDbError)
                        throw;
                }
            }
            finally
            {
                for (var index = 0; index < dbDataAdapterArray.Length; ++index)
                {
                    if (dbDataAdapterArray[index] != null)
                    {
                        dbDataAdapterArray[index].Dispose();
                        dbDataAdapterArray[index] = null;
                    }
                }
            }
            return num1;
        }

        private void SetGenericDbUpdateException(Exception e, DataSet updatingDS, string dbErrorMessage)
        {
            _vote = false;
            if (_autoCommit)
                Rollback();
            if (_updatingCommand != null)
            {
                var commandText = DatabaseHelper.GetCommandText(_updatingCommand);
                DatabaseHelper.WriteSqlLog(commandText);
                DatabaseHelper.WriteSqlLog(dbErrorMessage);
                DataSetHelper.AttachSqlLog(updatingDS, commandText, dbErrorMessage);
                if (!_catchDbError)
                    throw new DatabaseAdapterException(dbErrorMessage, commandText, e);
                DataSetHelper.AttachDbError(updatingDS, dbErrorMessage, commandText);
            }
            else
            {
                var str = e is DatabaseAdapterException ? "" : e.Message;
                DatabaseHelper.WriteSqlLog(str);
                DatabaseHelper.WriteSqlLog(dbErrorMessage);
                DataSetHelper.AttachSqlLog(updatingDS, str, dbErrorMessage);
                if (!_catchDbError)
                    return;
                DataSetHelper.AttachDbError(updatingDS, dbErrorMessage, str);
            }
        }


        public int Update(DataSet dsUpdate)
        {
            return Update(dsUpdate, true);
        }

        public int Update(DataSet dsUpdate, bool sysGen)
        {
            var selectSqls = new string[dsUpdate.Tables.Count];
            for (var index = 0; index < dsUpdate.Tables.Count; ++index)
                selectSqls[index] = DatabaseHelper.GetSelectSql(dsUpdate.Tables[index], new string[0]);
            return Update(dsUpdate, selectSqls, sysGen);
        }

        public int Update(DataSet dsUpdate, string[] selectSqls, bool sysGen)
        {
            var dtUpdates = dsUpdate.SortTables();
            var strArray = new string[dtUpdates.Length];
            for (var index1 = 0; index1 < dtUpdates.Length; ++index1)
            {
                var index2 = dsUpdate.Tables.IndexOf(dtUpdates[index1]);
                if (index2 >= 0 && index2 < selectSqls.Length)
                    strArray[index1] = selectSqls[index2];
            }
            selectSqls = strArray;
            return Update(dtUpdates, selectSqls, sysGen);
        }

        public object Execute(string commandText, params DbParameter[] commandParameters)
        {
            var flag1 = BeginTransaction();
            var flag2 = false;
            if (commandText.Trim().ToUpper().StartsWith("SELECT "))
            {
                flag2 = true;
                var queryBuilder = new QueryBuilder(commandText);
                queryBuilder.ColumnList.Insert(0, "'blank'");
                commandText = queryBuilder.Query;
            }
            var newCommand = DatabaseHelper.GetNewCommand(commandText, Connection, Transaction);
            newCommand.CommandTimeout = _commandTimeout;
            foreach (var dataParameter in commandParameters)
            {
                var str = DatabaseHelper.SetParameterName(dataParameter);
                var parameterSql = DatabaseHelper.GetParameterSql(dataParameter);
                commandText = commandText.Replace(":" + str + " ", parameterSql + " ");
                newCommand.Parameters.Add(dataParameter);
            }
            DatabaseHelper.WriteSqlLog(commandText);
            var obj = (object)null;
            try
            {
                if (flag2)
                {
                    var dbDataReader = newCommand.ExecuteReader();
                    if (dbDataReader.Read())
                        obj = dbDataReader[1];
                    dbDataReader.Close();
                }
                else
                    obj = newCommand.ExecuteNonQuery();
                if (flag1)
                {
                    if (_autoCommit)
                        Commit();
                }
            }
            catch (Exception ex)
            {
                if (!flag2)
                    _vote = false;
                if (_autoCommit)
                    Rollback();
                DatabaseHelper.WriteSqlLog(ex.Message);
                if (_catchDbError)
                    return ex.Message;
                throw new DatabaseAdapterException(ex.Message, commandText, ex);
            }
            return obj;
        }

        private struct SetParam
        {
            public string SetClause;
            public DbParameter Parameter;
            public object CurrentValue;
            public object OriginalValue;
            public bool AddWhere;

            public SetParam(string setClause, DbParameter dbParam)
            {
                SetClause = setClause;
                Parameter = dbParam;
                CurrentValue = null;
                OriginalValue = null;
                AddWhere = false;
            }

            public SetParam(string setClause, DbParameter dbParam, object currVal, object origVal)
            {
                this = new SetParam(setClause, dbParam);
                CurrentValue = currVal;
                OriginalValue = origVal;
            }
        }
    }
}
