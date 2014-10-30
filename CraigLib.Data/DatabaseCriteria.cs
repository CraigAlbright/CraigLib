using System;
using System.IO;
using System.Xml;

namespace CraigLib.Data
{
    [Serializable]
    public class DatabaseCriteria
    {
        public string LogOperator = string.Empty;
        public string OpenParen = string.Empty;
        public string DbTable = string.Empty;
        public string DbColumn = string.Empty;
        public string RelOperator = string.Empty;
        public string DbValue = string.Empty;
        public string CloseParen = string.Empty;

        public DatabaseCriteria()
        {
        }

        public DatabaseCriteria(string logOperator, string openParen, string dbTable, string dbColumn, string relOperator, string dbValue, string closeParen)
        {
            LogOperator = logOperator;
            OpenParen = openParen;
            DbTable = dbTable;
            DbColumn = dbColumn;
            RelOperator = relOperator;
            DbValue = dbValue;
            CloseParen = closeParen;
        }

        public DatabaseCriteria(XmlNode xmldoc)
        {
            LoadXml(xmldoc);
        }

        public DatabaseCriteria(string xml)
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
                if (xmlNode.Name == "LogOperator")
                    LogOperator = xmlNode.InnerText;
                else if (xmlNode.Name == "OpenParen")
                    OpenParen = xmlNode.InnerText;
                else if (xmlNode.Name == "DbTable")
                    DbTable = xmlNode.InnerText;
                else if (xmlNode.Name == "DbColumn")
                    DbColumn = xmlNode.InnerText;
                else if (xmlNode.Name == "RelOperator")
                    RelOperator = xmlNode.InnerText;
                else if (xmlNode.Name == "DbValue")
                    DbValue = xmlNode.InnerText;
                else if (xmlNode.Name == "CloseParen")
                    CloseParen = xmlNode.InnerText;
            }
        }

        public DatabaseCriteria Copy()
        {
            return new DatabaseCriteria(LogOperator, OpenParen, DbTable, DbColumn, RelOperator, DbValue, CloseParen);
        }

        public static void AdjustParen(DatabaseCriteria databaseCriteria)
        {
            var str1 = databaseCriteria.OpenParen.Replace(" ", "");
            var str2 = databaseCriteria.CloseParen.Replace(" ", "");
            if (str1.Length > str2.Length)
            {
                str1 = str1.Substring(str2.Length);
                str2 = "";
            }
            else if (str1.Length < str2.Length)
            {
                str2 = str2.Substring(str1.Length);
                str1 = "";
            }
            else if (str1.Length == str2.Length)
            {
                str1 = "";
                str2 = "";
            }
            databaseCriteria.OpenParen = str1;
            databaseCriteria.CloseParen = str2;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}{3} {4} '{5}'{6}", (object)LogOperator, (object)OpenParen, (object)(string.IsNullOrEmpty(DbTable) ? "" : DbTable + "."), (object)DbColumn, (object)RelOperator, (object)DbValue, (object)CloseParen);
        }
    }
}
