using System;
using System.IO;
using System.Xml;

namespace CraigLib.Data
{
    [Serializable]
    public class DatabaseJoin
    {
        public string DbTable = string.Empty;
        public string DbColumn = string.Empty;
        public string JoinRelation = "=";
        public string JoinTable = string.Empty;
        public string JoinColumn = string.Empty;

        public DatabaseJoin()
        {
        }

        public DatabaseJoin(string dbTable, string dbColumn, string joinTable, string joinColumn)
            : this(dbTable, dbColumn, "=", joinTable, joinColumn)
        {
        }

        public DatabaseJoin(string dbTable, string dbColumn, string joinRelation, string joinTable, string joinColumn)
        {
            DbTable = dbTable;
            DbColumn = dbColumn;
            JoinRelation = joinRelation;
            JoinTable = joinTable;
            JoinColumn = joinColumn;
        }

        public DatabaseJoin(XmlNode xmlnode)
        {
            LoadXml(xmlnode);
        }

        public DatabaseJoin(string xml)
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
            catch
            {
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
                if (xmlNode.Name == "DbTable")
                    DbTable = xmlNode.InnerText;
                else if (xmlNode.Name == "DbColumn")
                    DbColumn = xmlNode.InnerText;
                else if (xmlNode.Name == "JoinRelation")
                    JoinRelation = xmlNode.InnerText;
                else if (xmlNode.Name == "JoinTable")
                    JoinTable = xmlNode.InnerText;
                else if (xmlNode.Name == "JoinColumn")
                    JoinColumn = xmlNode.InnerText;
            }
        }
    }
}
