using System;
using System.Collections.Generic;
using System.Data;

namespace CraigLib.Data
{
    public static class DatabaseModel
    {
        private static bool _cacheChanged;
        private static DatabaseSchema _dbSchema;

        public static DatabaseSchema GetCurrentDbModel()
        {
            if (_dbSchema == null || _cacheChanged)
            {
                _dbSchema = (DatabaseSchema)CacheMachine.Get("DatabaseModel", new string[4]
        {
          "dbtable",
          "dbview",
          "dbobject",
          "dbconstraint"
        }, null);
                _cacheChanged = false;
            }
            return _dbSchema;
        }

        public static DateTime GetLatestDmDate()
        {
            return (DateTime)GetCurrentDbModel().ExtendedProperties["latestDmDate"];
        }

        public static DatabaseSchema.DbTableRow GetDbTable(string tablename)
        {
            return GetCurrentDbModel().DbTable.FindByDbTable(tablename);
        }

        public static DatabaseSchema.DbColumnRow[] GetDbColumns(string tablename, string filter, string sort)
        {
            DatabaseSchema.DbColumnRow[] dbColumnRowArray;
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
                    dbColumnRow = dbSchema.DbColumn.FindByDbTableDbColumn(tablename, columnname);
            }
            return dbColumnRow;
        }

        public static DatabaseSchema.DbConstraintRow GetDbConstraint(string constraintname)
        {
            return GetCurrentDbModel().DbConstraint.FindByDbConstraint(constraintname);
        }

        public static DatabaseSchema.DbColumnRow[] GetPrimaryKeys(string tablename)
        {
            return GetDbColumns(tablename, "DbPrimaryKey=1", "DbSeq");
        }

        public static DatabaseSchema.DbTableDataTable GetDbTables(string columnname)
        {
            var dbTableDataTable = new DatabaseSchema.DbTableDataTable();
            using (var dbAdapter = new DatabaseAdapter())
            {
                var selectSql = "select distinct dbtable.* from dbtable,dbobject where dbtable.dbtable=dbobject.dbtable and dbobject.dbcolumn=" + DatabaseHelper.SqlValue(columnname);
                dbAdapter.Fill(dbTableDataTable, selectSql);
            }
            return dbTableDataTable;
        }

        public static DatabaseSchema.DbColumnRow[] GetPrimaryKeys(DataTable dtSource, ref string dbtable)
        {
            var dbColumnRowArray = GetPrimaryKeys(dbtable);
            if (dbColumnRowArray.Length == 0 && dtSource.PrimaryKey.Length > 0)
            {
                var list = new List<DatabaseSchema.DbColumnRow>();
                foreach (var dc in dtSource.PrimaryKey)
                {
                    var dbTable = dc.GetDbTable();
                    var dbColumn1 = dc.GetDbColumn();
                    var dbColumn2 = GetDbColumn(dbTable, dbColumn1);
                    if (dbColumn2 != null)
                    {
                        dbtable = dbTable;
                        list.Add(dbColumn2);
                    }
                }
                dbColumnRowArray = list.ToArray();
            }
            return dbColumnRowArray;
        }

        public static DataTable AddDataTable(DataSet dsSource, string dbTable)
        {
            var dataTable = dsSource.Tables[dbTable];
            if (GetDbTable(dbTable) == null)
                return dataTable;
            if (dataTable == null)
                dataTable = dsSource.Tables.Add(dbTable);
            var dbColumns = GetDbColumns(dbTable, "", "");
            var list = new List<DataColumn>();
            foreach (var dbColumnRow in dbColumns)
            {
                var dc = dataTable.Columns[dbColumnRow.DbColumn] ?? dataTable.Columns.Add(dbColumnRow.DbColumn);
                dc.SetDataType(dbColumnRow);
                if (!string.IsNullOrEmpty(dbColumnRow.DbDefault))
                    dc.DefaultValue = DataSetHelper.GetDefaultValue(dbColumnRow.DbDefault, dc.DataType);
                if (dbColumnRow.DbPrimaryKey == 1)
                    list.Add(dc);
            }
            dataTable.PrimaryKey = list.ToArray();
            return dataTable;
        }
    }
}
