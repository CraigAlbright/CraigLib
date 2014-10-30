using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace CraigLib.Data
{
    [Serializable]
    public class SelectCriteria : ICloneable
    {
        public static readonly DateTime MinTime = new DateTime(1753, 1, 1);
        public static readonly DateTime MaxTime = new DateTime(9999, 12, 31);
        public string DateColumn = string.Empty;
        public DateTime BegTime = MinTime;
        public DateTime EndTime = MaxTime;
        public bool FilterByRelation = true;
        public string[] DrillColumn = new string[0];
        public DatabaseCriteria[] DatabaseCriteria = new DatabaseCriteria[0];
        public DatabaseJoin[] DatabaseJoin = new DatabaseJoin[0];
        public bool FilterDateByRelation;
        public bool FilterByForeignKey;

        public string DbCriteriaString
        {
            get
            {
                return DatabaseCriteria.Aggregate(string.Empty, (current, dbCriteria) => current + dbCriteria + " ");
            }
        }

        static SelectCriteria()
        {
        }

        public SelectCriteria()
        {
        }

        public SelectCriteria(string dateColumn, DateTime begTime, DateTime endTime, string[] drillColumn, DatabaseCriteria[] databaseCriteria, DatabaseJoin[] databaseJoin)
        {
            DateColumn = dateColumn;
            BegTime = begTime;
            EndTime = endTime;
            DrillColumn = drillColumn;
            DatabaseCriteria = databaseCriteria;
            DatabaseJoin = databaseJoin;
        }

        public SelectCriteria(XmlNode xmlnode)
        {
            LoadXml(xmlnode);
        }

        public SelectCriteria(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return;
            var stringReader = new StringReader(xml);
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(stringReader);
                LoadXml(xmlDocument.DocumentElement);
            }
           finally
            {
                stringReader.Close();
            }
        }

        private void LoadXml(XmlNode xmldoc)
        {
            foreach (XmlNode xmlNode in xmldoc.ChildNodes)
            {
                if (xmlNode.Name == "DateColumn")
                    DateColumn = xmlNode.InnerText;
                else if (xmlNode.Name == "BegTime")
                    BegTime = DateTime.Parse(xmlNode.InnerText);
                else if (xmlNode.Name == "EndTime")
                    EndTime = DateTime.Parse(xmlNode.InnerText);
                else if (xmlNode.Name == "FilterByRelation")
                    FilterByRelation = bool.Parse(xmlNode.InnerText);
                else if (xmlNode.Name == "FilterDateByRelation")
                    FilterDateByRelation = bool.Parse(xmlNode.InnerText);
                else if (xmlNode.Name == "FilterByForeignKey")
                    FilterByForeignKey = bool.Parse(xmlNode.InnerText);
                else if (xmlNode.Name == "DrillColumn")
                {
                    DrillColumn = new string[xmlNode.ChildNodes.Count];
                    for (var index = 0; index < xmlNode.ChildNodes.Count; ++index)
                        DrillColumn[index] = xmlNode.ChildNodes[index].InnerText;
                }
                else if (xmlNode.Name == "DatabaseCriteria")
                {
                    DatabaseCriteria = new DatabaseCriteria[xmlNode.ChildNodes.Count];
                    for (var index = 0; index < xmlNode.ChildNodes.Count; ++index)
                        DatabaseCriteria[index] = new DatabaseCriteria(xmlNode.ChildNodes[index]);
                }
                else if (xmlNode.Name == "DatabaseJoin")
                {
                    DatabaseJoin = new DatabaseJoin[xmlNode.ChildNodes.Count];
                    for (var index = 0; index < xmlNode.ChildNodes.Count; ++index)
                        DatabaseJoin[index] = new DatabaseJoin(xmlNode.ChildNodes[index]);
                }
            }
        }

        public int AddDrillColumn(params string[] newDrillColumn)
        {
            var list = new List<string>(DrillColumn);
            list.AddRange(newDrillColumn);
            DrillColumn = list.ToArray();
            return DrillColumn.Length;
        }

        public int RemoveDrillColumn(params string[] oldDrillColumn)
        {
            var list = new List<string>(DrillColumn);
            foreach (var str in oldDrillColumn)
                list.Remove(str);
            DrillColumn = list.ToArray();
            return DrillColumn.Length;
        }

        public int AddDbCriteria(string logOperator, string openParen, string dbTable, string dbColumn, string relOperator, string dbValue, string closeParen)
        {
            return AddDbCriteria(new[]
      {
        new DatabaseCriteria(logOperator, openParen, dbTable, dbColumn, relOperator, dbValue, closeParen)
      });
        }

        public int AddDbCriteria(params DatabaseCriteria[] newDatabaseCriteria)
        {
            var list = new List<DatabaseCriteria>(DatabaseCriteria);
            list.AddRange(newDatabaseCriteria);
            DatabaseCriteria = list.ToArray();
            return DatabaseCriteria.Length;
        }

        public int AddDbCriteriaWithLimit(int paramLimit, params DatabaseCriteria[] newDatabaseCriteria)
        {
            newDatabaseCriteria[0].Copy();
            var list = new List<DatabaseCriteria>(DatabaseCriteria);
            var index1 = 0;
            var strArray1 = newDatabaseCriteria[0].DbValue.Split(new[]
      {
        ','
      });
            var length = strArray1.Length / paramLimit;
            if (strArray1.Length % paramLimit > 0)
                ++length;
            var strArray2 = new string[length];
            for (var index2 = 0; index2 < strArray1.Length; ++index2)
            {
                if (index2 != 0 && index2 % paramLimit == 0)
                    ++index1;
                strArray2[index1] = index2 % paramLimit == 0 || index2 == 0 ? strArray2[index1] + strArray1[index2] : strArray2[index1] + "," + strArray1[index2];
            }
            for (var index2 = 0; index2 < length; ++index2)
            {
                var databaseCriteria = newDatabaseCriteria[0].Copy();
                databaseCriteria.DbValue = strArray2[index2];
                if (index2 > 0 && length > 1 && index2 < length - 1)
                {
                    databaseCriteria.LogOperator = "or";
                    databaseCriteria.OpenParen = "(";
                    databaseCriteria.CloseParen = ")";
                }
                else if (index2 == 0)
                {
                    databaseCriteria.OpenParen = databaseCriteria.OpenParen + "(";
                    databaseCriteria.CloseParen = index2 != length - 1 ? ")" : databaseCriteria.CloseParen + ")";
                }
                else if (index2 == length - 1)
                {
                    databaseCriteria.LogOperator = "or";
                    databaseCriteria.CloseParen = databaseCriteria.CloseParen + ")";
                    databaseCriteria.OpenParen = "(";
                }
                list.Add(databaseCriteria);
            }
            DatabaseCriteria = list.ToArray();
            return DatabaseCriteria.Length;
        }

        public int RemoveDbCriteria(params DatabaseCriteria[] oldDatabaseCriteria)
        {
            var list = new List<DatabaseCriteria>(DatabaseCriteria);
            foreach (var dbCriteria in oldDatabaseCriteria)
                list.Remove(dbCriteria);
            DatabaseCriteria = list.ToArray();
            return DatabaseCriteria.Length;
        }

        public int RemoveDbCriteriaAt(int index)
        {
            var list = new List<DatabaseCriteria>(DatabaseCriteria);
            list.RemoveAt(index);
            DatabaseCriteria = list.ToArray();
            return DatabaseCriteria.Length;
        }

        public int AddDatabaseJoin(string dbTable, string dbColumn, string joinRelation, string joinTable, string joinColumn)
        {
            return AddDatabaseJoin(new[]
      {
        new DatabaseJoin(dbTable, dbColumn, joinRelation, joinTable, joinColumn)
      });
        }

        public int AddDatabaseJoin(params DatabaseJoin[] newDatabaseJoin)
        {
            var list = new List<DatabaseJoin>(DatabaseJoin);
            list.AddRange(newDatabaseJoin);
            DatabaseJoin = list.ToArray();
            return DatabaseJoin.Length;
        }

        public int RemoveDatabaseJoin(params DatabaseJoin[] oldDatabaseJoin)
        {
            var list = new List<DatabaseJoin>(DatabaseJoin);
            foreach (var dbJoin in oldDatabaseJoin)
                list.Remove(dbJoin);
            DatabaseJoin = list.ToArray();
            return DatabaseJoin.Length;
        }

        public int RemoveDatabaseJoinAt(int index)
        {
            var list = new List<DatabaseJoin>(DatabaseJoin);
            list.RemoveAt(index);
            DatabaseJoin = list.ToArray();
            return DatabaseJoin.Length;
        }

        public object Clone()
        {
            var memoryStream = new MemoryStream(1000);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, this);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            var obj = binaryFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            return obj;
        }
    }
}
