using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Data
{
    public class BulkCopy
    {
        public static void WriteToServer(DbTransaction tran, DataRow[] rows)
        {
            SqlBulkCopy bc = new SqlBulkCopy((SqlConnection)tran.Connection, SqlBulkCopyOptions.CheckConstraints, (SqlTransaction)tran);
            bc.BulkCopyTimeout = ApplicationConfig.DbCommandTimeout;
            if (rows.Length == 0)
                return;
            DataTable table = rows[0].Table;
            bool[] flagArray = BulkCopy.MapColumns(bc, table);
            if (flagArray[0])
            {
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                    table.Columns.Add("creationname", typeof(string), Expr.Value(windowsIdentity.Name));
                bc.ColumnMappings.Add("creationname", "creationname");
            }
            if (flagArray[1])
            {
                table.Columns.Add("creationdate", typeof(DateTime), Expr.Value((object)DatabaseHelper.GetServerDate()));
                bc.ColumnMappings.Add("creationdate", "creationdate");
            }
            bc.DestinationTableName = DataSetHelper.GetDbTable(table);
            bc.WriteToServer(rows);
            bc.Close();
            if (flagArray[0])
                table.Columns.Remove("creationname");
            if (!flagArray[1])
                return;
            table.Columns.Remove("creationdate");
        }

        private static bool[] MapColumns(SqlBulkCopy bc, DataTable dt)
        {
            string dbTable = DataSetHelper.GetDbTable(dt);
            bool[] flagArray = new bool[2]
      {
        false,
        false
      };
            foreach (DatabaseSchema.DbColumnRow dbColumnRow in DatabaseModel.GetDbColumns(dbTable, "", ""))
            {
                if (dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase))
                    flagArray[0] = true;
                else if (dbColumnRow.DbColumn.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                    flagArray[1] = true;
                foreach (DataColumn dc in (InternalDataCollectionBase)dt.Columns)
                {
                    if (dc.ColumnName.Equals("creationname", StringComparison.OrdinalIgnoreCase))
                        flagArray[0] = false;
                    else if (dc.ColumnName.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                        flagArray[1] = false;
                    if (dbColumnRow.DbColumn.Equals(DataSetHelper.GetDbColumn(dc), StringComparison.OrdinalIgnoreCase))
                    {
                        bc.ColumnMappings.Add(dc.ColumnName, DataSetHelper.GetDbColumn(dc));
                        break;
                    }
                }
            }
            return flagArray;
        }

        public static bool BulkWriteToServer(DbTransaction tran, DataRow[] rows)
        {
            if (rows.Length == 0)
                return false;
            DataTable table = rows[0].Table;
            string dbTable = DataSetHelper.GetDbTable(table);
            DataRowState rowState = rows[0].RowState;
            Type type1 = tran.GetType();
            Type type2 = type1.FullName == "Oracle.DataAccess.Client.OracleTransaction" ? type1.Assembly.GetType("Oracle.DataAccess.Client.OracleBulkCopy") : (Type)null;
            if (!(tran is SqlTransaction) && !(type2 != (Type)null))
                return false;
            DbConnection conn = tran is SqlTransaction ? tran.Connection : DatabaseHelper.GetNewConnection(true);
            try
            {
                DatabaseSchema.DbColumnRow[] dbColumns = DatabaseContent.GetDbColumns(dbTable, "", "DbSeq");
                SqlTransaction externalTransaction = tran as SqlTransaction;
                if (externalTransaction != null)
                {
                    SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.CheckConstraints, externalTransaction)
                    {
                        DestinationTableName = dbTable,
                        BulkCopyTimeout = ApplicationConfig.DbCommandTimeout
                    };
                    IEnumerable<DataColumn> addedColumns = BulkCopy.MapColumns((object)sqlBulkCopy, table, (IEnumerable<DatabaseSchema.DbColumnRow>)dbColumns, rowState);
                    sqlBulkCopy.WriteToServer(rows);
                    sqlBulkCopy.Close();
                    BulkCopy.UnmapColumns(table, addedColumns);
                }
                else if (type2 != (Type)null)
                {
                    object instance = Activator.CreateInstance(type2, new object[1]
          {
            (object) conn
          });
                    type2.GetProperty("DestinationTableName").SetValue(instance, (object)dbTable, (object[])null);
                    type2.GetProperty("BulkCopyTimeout").SetValue(instance, (object)ApplicationConfig.DbCommandTimeout, (object[])null);
                    BulkCopy.OracleRowsReader oracleRowsReader = new BulkCopy.OracleRowsReader(conn, dbTable, rows);
                    type2.GetMethod("WriteToServer", new Type[1]
          {
            typeof (IDataReader)
          }).Invoke(instance, new object[1]
          {
            (object) oracleRowsReader
          });
                    type2.GetMethod("Close").Invoke(instance, (object[])null);
                }
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }
            finally
            {
                if (type2 != (Type)null)
                    conn.Close();
            }
            return true;
        }

        private static IEnumerable<DataColumn> MapColumns(object bc, DataTable dt, IEnumerable<DatabaseSchema.DbColumnRow> cols, DataRowState rowState)
        {
            object obj = bc.GetType().GetProperty("ColumnMappings").GetValue(bc, new object[0]);
            MethodInfo method = obj.GetType().GetMethod("Add", new Type[2]
      {
        typeof (string),
        typeof (string)
      });
            List<DataColumn> list = new List<DataColumn>();
            foreach (DatabaseSchema.DbColumnRow dbColumnRow in cols)
            {
                DataColumn dc = dt.Columns[dbColumnRow.DbColumn];
                if (rowState != DataRowState.Deleted || dbColumnRow.DbPrimaryKey == 1)
                {
                    if (dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase) || dbColumnRow.DbColumn.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                    {
                        if (rowState != DataRowState.Modified)
                        {
                            if (rowState == DataRowState.Added && dc == null)
                            {
                                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                                if (windowsIdentity != null)
                                    list.Add(dc = dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase) ? dt.Columns.Add("creationname", typeof(string), Expr.Value(windowsIdentity.Name)) : dt.Columns.Add("creationdate", typeof(DateTime), Expr.Value((object)DatabaseHelper.GetServerDate())));
                            }
                        }
                        else
                            continue;
                    }
                    else if (dbColumnRow.DbColumn.Equals("revisionname", StringComparison.OrdinalIgnoreCase) || dbColumnRow.DbColumn.Equals("revisiondate", StringComparison.OrdinalIgnoreCase))
                    {
                        if (rowState != DataRowState.Added)
                        {
                            if (rowState == DataRowState.Modified && dc == null)
                            {
                                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                                if (identity != null)
                                    list.Add(dc = dbColumnRow.DbColumn.Equals("revisionname", StringComparison.OrdinalIgnoreCase) ? dt.Columns.Add("revisionname", typeof(string), Expr.Value(identity.Name)) : dt.Columns.Add("revisiondate", typeof(DateTime), Expr.Value((object)DatabaseHelper.GetServerDate())));
                            }
                        }
                        else
                            continue;
                    }
                    if (dc != null)
                        method.Invoke(obj, new object[2]
            {
              (object) dc.ColumnName,
              (object) DataSetHelper.GetDbColumn(dc)
            });
                }
            }
            return (IEnumerable<DataColumn>)list;
        }

        private static void UnmapColumns(DataTable dt, IEnumerable<DataColumn> addedColumns)
        {
            foreach (DataColumn column in addedColumns)
                dt.Columns.Remove(column);
        }

        private class OracleRowsReader : IDataReader, IDisposable, IDataRecord
        {
            private readonly Dictionary<string, int> _mapping = new Dictionary<string, int>();
            private bool _open = true;
            private int _rowPos = -1;
            private readonly DataTable _schemaTable;
            private readonly DataRow[] _rows;
            private readonly DataTable _rowTable;

            public object this[string name]
            {
                get
                {
                    return this[this.GetOrdinal(name)];
                }
            }

            public object this[int i]
            {
                get
                {
                    return this.GetRowValue(i);
                }
            }

            public int Depth
            {
                get
                {
                    return 0;
                }
            }

            public bool IsClosed
            {
                get
                {
                    return !this._open;
                }
            }

            public int RecordsAffected
            {
                get
                {
                    return this._rows.Length;
                }
            }

            public int FieldCount
            {
                get
                {
                    return this._schemaTable.Columns.Count;
                }
            }

            public OracleRowsReader(DbConnection conn, string dbTable, DataRow[] rows)
            {
                using (DbDataAdapter newDataAdapter = DatabaseHelper.GetNewDataAdapter("SELECT * FROM " + dbTable + " WHERE 0=1", conn))
                {
                    this._schemaTable = new DataTable(dbTable);
                    newDataAdapter.Fill(this._schemaTable);
                }
                this._rows = rows;
                this._rowTable = this._rows.Length > 0 ? this._rows[0].Table : new DataTable(dbTable);
                foreach (DataColumn dc in (InternalDataCollectionBase)this._rowTable.Columns)
                    this._mapping[DataSetHelper.GetDbColumnOrDefault(dc).ToUpper()] = dc.Ordinal;
            }

            public void Close()
            {
                this._open = false;
            }

            public DataTable GetSchemaTable()
            {
                return this._schemaTable;
            }

            public bool NextResult()
            {
                return this._rowPos < this._rows.Length - 1;
            }

            public bool Read()
            {
                return ++this._rowPos < this._rows.Length;
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize((object)this);
            }

            public bool GetBoolean(int i)
            {
                return (bool)this.GetRowValue(i);
            }

            public byte GetByte(int i)
            {
                return (byte)this.GetRowValue(i);
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotSupportedException("GetBytes not supported.");
            }

            public char GetChar(int i)
            {
                return (char)this.GetRowValue(i);
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                return 0L;
            }

            public IDataReader GetData(int i)
            {
                return (IDataReader)null;
            }

            public string GetDataTypeName(int i)
            {
                return this._schemaTable.Columns[i].DataType.Name;
            }

            public DateTime GetDateTime(int i)
            {
                return (DateTime)this.GetRowValue(i);
            }

            public Decimal GetDecimal(int i)
            {
                return (Decimal)this.GetRowValue(i);
            }

            public double GetDouble(int i)
            {
                return (double)this.GetRowValue(i);
            }

            public Type GetFieldType(int i)
            {
                return this._schemaTable.Columns[i].DataType;
            }

            public float GetFloat(int i)
            {
                return (float)this.GetRowValue(i);
            }

            public Guid GetGuid(int i)
            {
                return (Guid)this.GetRowValue(i);
            }

            public short GetInt16(int i)
            {
                return (short)this.GetRowValue(i);
            }

            public int GetInt32(int i)
            {
                return (int)this.GetRowValue(i);
            }

            public long GetInt64(int i)
            {
                return (long)this.GetRowValue(i);
            }

            public string GetName(int i)
            {
                return this._schemaTable.Columns[i].ColumnName;
            }

            public int GetOrdinal(string name)
            {
                return this._schemaTable.Columns[name].Ordinal;
            }

            public string GetString(int i)
            {
                return (string)this.GetRowValue(i);
            }

            public object GetValue(int i)
            {
                return this.GetRowValue(i);
            }

            public int GetValues(object[] values)
            {
                int index = 0;
                for (int i = 0; index < values.Length && i < this._schemaTable.Columns.Count; ++i)
                {
                    values[index] = this.GetRowValue(i);
                    ++index;
                }
                return index;
            }

            public bool IsDBNull(int i)
            {
                return this.GetRowValue(i) == DBNull.Value;
            }

            private void Dispose(bool disposing)
            {
                if (!disposing)
                    return;
                try
                {
                    this.Close();
                    this._schemaTable.Dispose();
                }
                catch (Exception ex)
                {
                    throw new SystemException("An exception of type " + (object)ex.GetType() + " was encountered while closing the OracleRowsReader.");
                }
            }

            private object GetRowValue(int i)
            {
                string columnName = this._schemaTable.Columns[i].ColumnName;
                if (!this._mapping.ContainsKey(columnName))
                    return (object)DBNull.Value;
                else
                    return this._rows[this._rowPos][this._mapping[columnName]];
            }
        }
    }
}
