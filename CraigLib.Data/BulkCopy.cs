using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Principal;

namespace CraigLib.Data
{
    public class BulkCopy
    {
        public static void WriteToServer(DbTransaction tran, DataRow[] rows)
        {
            var bc = new SqlBulkCopy((SqlConnection)tran.Connection, SqlBulkCopyOptions.CheckConstraints, (SqlTransaction)tran)
            {
                BulkCopyTimeout = ApplicationConfig.DbCommandTimeout
            };
            if (rows.Length == 0)
                return;
            var table = rows[0].Table;
            var flagArray = MapColumns(bc, table);
            if (flagArray[0])
            {
                var windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                    table.Columns.Add("creationname", typeof(string), Expr.Value(windowsIdentity.Name));
                bc.ColumnMappings.Add("creationname", "creationname");
            }
            if (flagArray[1])
            {
                table.Columns.Add("creationdate", typeof(DateTime), Expr.Value(DatabaseHelper.GetServerDate()));
                bc.ColumnMappings.Add("creationdate", "creationdate");
            }
            bc.DestinationTableName = table.GetDbTable();
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
            var dbTable = dt.GetDbTable();
            var flagArray = new[]
      {
        false,
        false
      };
            foreach (var dbColumnRow in DatabaseModel.GetDbColumns(dbTable, "", ""))
            {
                if (dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase))
                    flagArray[0] = true;
                else if (dbColumnRow.DbColumn.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                    flagArray[1] = true;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Equals("creationname", StringComparison.OrdinalIgnoreCase))
                        flagArray[0] = false;
                    else if (dc.ColumnName.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                        flagArray[1] = false;
                    if (!dbColumnRow.DbColumn.Equals(dc.GetDbColumn(), StringComparison.OrdinalIgnoreCase)) continue;
                    bc.ColumnMappings.Add(dc.ColumnName, dc.GetDbColumn());
                    break;
                }
            }
            return flagArray;
        }

        public static bool BulkWriteToServer(DbTransaction tran, DataRow[] rows)
        {
            if (rows.Length == 0)
                return false;
            var table = rows[0].Table;
            var dbTable = table.GetDbTable();
            var rowState = rows[0].RowState;
            var type1 = tran.GetType();
            var type2 = type1.FullName == "Oracle.DataAccess.Client.OracleTransaction" ? type1.Assembly.GetType("Oracle.DataAccess.Client.OracleBulkCopy") : null;
            if (!(tran is SqlTransaction) && !(type2 != null))
                return false;
            var conn = tran is SqlTransaction ? tran.Connection : DatabaseHelper.GetNewConnection(true);
            try
            {
                var dbColumns = DatabaseContent.GetDbColumns(dbTable, "", "DbSeq");
                var externalTransaction = tran as SqlTransaction;
                if (externalTransaction != null)
                {
                    var sqlBulkCopy = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.CheckConstraints, externalTransaction)
                    {
                        DestinationTableName = dbTable,
                        BulkCopyTimeout = ApplicationConfig.DbCommandTimeout
                    };
                    var addedColumns = MapColumns(sqlBulkCopy, table, dbColumns, rowState);
                    sqlBulkCopy.WriteToServer(rows);
                    sqlBulkCopy.Close();
                    UnmapColumns(table, addedColumns);
                }
                else
                {
                    var instance = Activator.CreateInstance(type2, new object[]
                    {
                        conn
                    });
                    type2.GetProperty("DestinationTableName").SetValue(instance, dbTable, null);
                    type2.GetProperty("BulkCopyTimeout").SetValue(instance, ApplicationConfig.DbCommandTimeout, null);
                    var oracleRowsReader = new OracleRowsReader(conn, dbTable, rows);
                    type2.GetMethod("WriteToServer", new[]
                    {
                        typeof (IDataReader)
                    }).Invoke(instance, new object[]
                    {
                        oracleRowsReader
                    });
                    type2.GetMethod("Close").Invoke(instance, null);
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
                if (type2 != null)
                    conn.Close();
            }
            return true;
        }

        private static IEnumerable<DataColumn> MapColumns(object bc, DataTable dt, IEnumerable<DatabaseSchema.DbColumnRow> cols, DataRowState rowState)
        {
            var obj = bc.GetType().GetProperty("ColumnMappings").GetValue(bc, new object[0]);
            var method = obj.GetType().GetMethod("Add", new[]
      {
        typeof (string),
        typeof (string)
      });
            var list = new List<DataColumn>();
            foreach (var dbColumnRow in cols)
            {
                var dc = dt.Columns[dbColumnRow.DbColumn];
                if (rowState == DataRowState.Deleted && dbColumnRow.DbPrimaryKey != 1) continue;
                if (dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase) || dbColumnRow.DbColumn.Equals("creationdate", StringComparison.OrdinalIgnoreCase))
                {
                    if (rowState != DataRowState.Modified)
                    {
                        if (rowState == DataRowState.Added && dc == null)
                        {
                            var windowsIdentity = WindowsIdentity.GetCurrent();
                            if (windowsIdentity != null)
                                list.Add(dc = dbColumnRow.DbColumn.Equals("creationname", StringComparison.OrdinalIgnoreCase) ? dt.Columns.Add("creationname", typeof(string), Expr.Value(windowsIdentity.Name)) : dt.Columns.Add("creationdate", typeof(DateTime), Expr.Value(DatabaseHelper.GetServerDate())));
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
                            var identity = WindowsIdentity.GetCurrent();
                            if (identity != null)
                                list.Add(dc = dbColumnRow.DbColumn.Equals("revisionname", StringComparison.OrdinalIgnoreCase) ? dt.Columns.Add("revisionname", typeof(string), Expr.Value(identity.Name)) : dt.Columns.Add("revisiondate", typeof(DateTime), Expr.Value(DatabaseHelper.GetServerDate())));
                        }
                    }
                    else
                        continue;
                }
                if (dc != null)
                    method.Invoke(obj, new object[]
                    {
                        dc.ColumnName,
                        dc.GetDbColumn()
                    });
            }
            return list;
        }

        private static void UnmapColumns(DataTable dt, IEnumerable<DataColumn> addedColumns)
        {
            foreach (var column in addedColumns)
                dt.Columns.Remove(column);
        }

        private class OracleRowsReader : IDataReader
        {
            private readonly Dictionary<string, int> _mapping = new Dictionary<string, int>();
            private bool _open = true;
            private int _rowPos = -1;
            private readonly DataTable _schemaTable;
            private readonly DataRow[] _rows;

            public object this[string name]
            {
                get
                {
                    return this[GetOrdinal(name)];
                }
            }

            public object this[int i]
            {
                get
                {
                    return GetRowValue(i);
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
                    return !_open;
                }
            }

            public int RecordsAffected
            {
                get
                {
                    return _rows.Length;
                }
            }

            public int FieldCount
            {
                get
                {
                    return _schemaTable.Columns.Count;
                }
            }

            public OracleRowsReader(DbConnection conn, string dbTable, DataRow[] rows)
            {
                using (var newDataAdapter = DatabaseHelper.GetNewDataAdapter("SELECT * FROM " + dbTable + " WHERE 0=1", conn))
                {
                    _schemaTable = new DataTable(dbTable);
                    newDataAdapter.Fill(_schemaTable);
                }
                _rows = rows;
                var rowTable = _rows.Length > 0 ? _rows[0].Table : new DataTable(dbTable);
                foreach (DataColumn dc in rowTable.Columns)
                    _mapping[dc.GetDbColumnOrDefault().ToUpper()] = dc.Ordinal;
            }

            public void Close()
            {
                _open = false;
            }

            public DataTable GetSchemaTable()
            {
                return _schemaTable;
            }

            public bool NextResult()
            {
                return _rowPos < _rows.Length - 1;
            }

            public bool Read()
            {
                return ++_rowPos < _rows.Length;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public bool GetBoolean(int i)
            {
                return (bool)GetRowValue(i);
            }

            public byte GetByte(int i)
            {
                return (byte)GetRowValue(i);
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotSupportedException("GetBytes not supported.");
            }

            public char GetChar(int i)
            {
                return (char)GetRowValue(i);
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                return 0L;
            }

            public IDataReader GetData(int i)
            {
                return null;
            }

            public string GetDataTypeName(int i)
            {
                return _schemaTable.Columns[i].DataType.Name;
            }

            public DateTime GetDateTime(int i)
            {
                return (DateTime)GetRowValue(i);
            }

            public Decimal GetDecimal(int i)
            {
                return (Decimal)GetRowValue(i);
            }

            public double GetDouble(int i)
            {
                return (double)GetRowValue(i);
            }

            public Type GetFieldType(int i)
            {
                return _schemaTable.Columns[i].DataType;
            }

            public float GetFloat(int i)
            {
                return (float)GetRowValue(i);
            }

            public Guid GetGuid(int i)
            {
                return (Guid)GetRowValue(i);
            }

            public short GetInt16(int i)
            {
                return (short)GetRowValue(i);
            }

            public int GetInt32(int i)
            {
                return (int)GetRowValue(i);
            }

            public long GetInt64(int i)
            {
                return (long)GetRowValue(i);
            }

            public string GetName(int i)
            {
                return _schemaTable.Columns[i].ColumnName;
            }

            public int GetOrdinal(string name)
            {
                return _schemaTable.Columns[name].Ordinal;
            }

            public string GetString(int i)
            {
                return (string)GetRowValue(i);
            }

            public object GetValue(int i)
            {
                return GetRowValue(i);
            }

            public int GetValues(object[] values)
            {
                var index = 0;
                for (var i = 0; index < values.Length && i < _schemaTable.Columns.Count; ++i)
                {
                    values[index] = GetRowValue(i);
                    ++index;
                }
                return index;
            }

            public bool IsDBNull(int i)
            {
                return GetRowValue(i) == DBNull.Value;
            }

            private void Dispose(bool disposing)
            {
                if (!disposing)
                    return;
                try
                {
                    Close();
                    _schemaTable.Dispose();
                }
                catch (Exception ex)
                {
                    throw new SystemException("An exception of type " + ex.GetType() + " was encountered while closing the OracleRowsReader.");
                }
            }

            private object GetRowValue(int i)
            {
                var columnName = _schemaTable.Columns[i].ColumnName;
                return !_mapping.ContainsKey(columnName) ? DBNull.Value : _rows[_rowPos][_mapping[columnName]];
            }
        }
    }
}
