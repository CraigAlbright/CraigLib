using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CraigLib
{
    public class ConnectionInfo
    {
        public static string OracleDriver = "";
        public static string OraDriverVer = "";
        public string UserId = string.Empty;
        public string Password = string.Empty;
        public string DataSource = string.Empty;
        public string Database = string.Empty;
        public string AttachDbFilename = string.Empty;
        public int ConnectionTimeout = 15;
        public bool Pooling = true;
        public int MinPoolSize = 5;
        public int MaxPoolSize = 100;
        public bool Enlist = true;
        private string _dataSourceName = string.Empty;
        private string _dbms = string.Empty;
        private string _dbProvider = string.Empty;
        private DatabaseType _dbSyntax = DatabaseType.ORACLE;
        private string _tablePrefix = string.Empty;
        private Dictionary<string, string> _otherParams = new Dictionary<string, string>();
        private string _connectstring = "";
        public bool IntegratedSecurity;
        public bool PersistSecurityInfo;
        public bool UserEqualsSchema;
        public bool AllowSnapshot;
        public int ConnectionLifetime;

        public string ConnectionString
        {
            get
            {
                if (_connectstring.Length == 0)
                    _connectstring = JoinConnectionString();
                return _connectstring;
            }
            set
            {
                Reset();
                ParseConnectionString(value);
            }
        }

        public string ConnStrNoPw
        {
            get
            {
                return ConnectionString.Replace("word=" + Password, "word=***").Replace("word=\"" + Password, "word=\"***");
            }
        }

        public string DataSourceName
        {
            get
            {
                return _dataSourceName;
            }
        }

        public string DbProvider
        {
            get
            {
                return _dbProvider;
            }
        }

        public DatabaseType DbSyntax
        {
            get
            {
                return _dbSyntax;
            }
        }

        public string TablePrefix
        {
            get
            {
                return _tablePrefix;
            }
        }

        static ConnectionInfo()
        {
        }

        public ConnectionInfo()
        {
            ParseConnectionString(ApplicationConfig.ConnectionStrings["Default"]);
        }

        //public ConnectionInfo(string datasourcename, string emptyparam)
        //{
        //    ParseConnectionString(ApplicationConfig.ConnectionStrings[datasourcename]);
        //}

        public ConnectionInfo(string userId, string password, string dbms, string server, string database)
        {
            ParseConnectionString("userid=" + userId + ";password=" + password + ";dbms=" + dbms + ";server=" + server + ";database=" + database);
        }

        public ConnectionInfo(string connectionString)
        {
            ParseConnectionString(connectionString);
        }

        private void Reset()
        {
            UserId = string.Empty;
            Password = string.Empty;
            DataSource = string.Empty;
            IntegratedSecurity = false;
            PersistSecurityInfo = false;
            Database = string.Empty;
            AttachDbFilename = string.Empty;
            ConnectionTimeout = 15;
            Pooling = true;
            MinPoolSize = 0;
            MaxPoolSize = 100;
            ConnectionLifetime = 0;
            Enlist = true;
            _connectstring = "";
            _dataSourceName = string.Empty;
            _dbms = string.Empty;
            _dbProvider = string.Empty;
            _dbSyntax = DatabaseType.ORACLE;
            _tablePrefix = string.Empty;
            _otherParams.Clear();
        }

        private void ParseConnectionString(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder {ConnectionString = connectionString};
            if (connectionStringBuilder.Keys != null)
                foreach (string key in connectionStringBuilder.Keys)
                {
                    var s = connectionStringBuilder[key].ToString();
                    switch (key.Replace(" ", "").ToLower())
                    {
                        case "userid":
                            UserId = s;
                            continue;
                        case "password":
                        case "pwd":
                            Password = s;
                            continue;
                        case "dbms":
                            _dbms = s;
                            continue;
                        case "server":
                        case "datasource":
                        case "address":
                        case "addr":
                        case "networkaddress":
                            DataSource = s;
                            continue;
                        case "database":
                        case "initialcatalog":
                            Database = _dbms == "ORACLE" ? s.ToUpper() : s;
                            continue;
                        case "attachdbfilename":
                            AttachDbFilename = s;
                            continue;
                        case "integratedsecurity":
                        case "trustedconnection":
                            if (!bool.TryParse(s, out IntegratedSecurity))
                                break;
                            continue;
                        case "persistsecurityinfo":
                            if (!bool.TryParse(s, out PersistSecurityInfo))
                                break;
                            continue;
                        case "connectiontimeout":
                        case "connecttimeout":
                            if (!int.TryParse(s, out ConnectionTimeout))
                                break;
                            continue;
                        case "pooling":
                            if (!bool.TryParse(s, out Pooling))
                                break;
                            continue;
                        case "minpoolsize":
                            if (!int.TryParse(s, out MinPoolSize))
                                break;
                            continue;
                        case "maxpoolsize":
                            if (!int.TryParse(s, out MaxPoolSize))
                                break;
                            continue;
                        case "connectionlifetime":
                            if (!int.TryParse(s, out ConnectionLifetime))
                                break;
                            continue;
                        case "enlist":
                            if (bool.TryParse(s, out Enlist))
                                continue;
                            break;
                        case "snapshot":
                            continue;
                    }
                    _otherParams.Add(key, s);
                }
            _dbProvider = GetDbProvider();
            _dbSyntax = GetDbSyntax();
            _tablePrefix = string.Empty;
            if (_dbSyntax == DatabaseType.ORACLE && string.IsNullOrEmpty(Database))
                Database = UserId;
            if (_dbSyntax == DatabaseType.ORACLE && UserId.Equals(Database, StringComparison.OrdinalIgnoreCase))
                UserEqualsSchema = true;
            switch (_dbSyntax)
            {
                case DatabaseType.MSSQL:
                    _tablePrefix = "dbo.";
                    break;
            }
        }

        private void GetOraProvider()
        {
            var dataRow = DbProviderFactories.GetFactoryClasses().Rows.Find("Oracle.DataAccess.Client");
            if (dataRow == null)
            {
                OracleDriver = "System.Data.OracleClient";
            }
            else
            {
                OracleDriver = "Oracle.DataAccess.Client";
                if (!dataRow.Table.Columns.Contains("AssemblyQualifiedName") || dataRow.IsNull("AssemblyQualifiedName"))
                    return;
                var str = (string)dataRow["AssemblyQualifiedName"];
                var startIndex = str.IndexOf("Version=", StringComparison.Ordinal) + 8;
                var num = -1;
                if (startIndex > 7)
                    num = str.IndexOf(",", startIndex, StringComparison.Ordinal);
                if (startIndex < 8 || num < 0)
                    return;
                OraDriverVer = str.Substring(startIndex, num - startIndex);
            }
        }

        private string GetDbProvider()
        {
            if (!_dbms.StartsWith("O"))
                return "System.Data.SqlClient";
            if (OracleDriver.Length == 0)
                GetOraProvider();
            return OracleDriver;
        }

        private DatabaseType GetDbSyntax()
        {
            if (_dbms.Length > 0)
            {
                var str = _dbms.ToUpper();
                if (str.StartsWith("MSS"))
                    return DatabaseType.MSSQL;
                if (str.StartsWith("O"))
                    return DatabaseType.ORACLE;
                return DatabaseType.MSSQL;
            }
            return Database.Length > 0 || AttachDbFilename.Length > 0 ? DatabaseType.MSSQL : DatabaseType.ORACLE;
        }

        private string JoinConnectionString()
        {
            var connectionStringBuilder = new DbConnectionStringBuilder();
            if (UserId.Length > 0)
                connectionStringBuilder["User ID"] = UserId;
            if (DataSource.Length > 0)
                connectionStringBuilder["Data Source"] = DataSource;
            if (IntegratedSecurity)
                connectionStringBuilder["Integrated Security"] = IntegratedSecurity;
            if (PersistSecurityInfo)
                connectionStringBuilder["Persist Security Info"] = PersistSecurityInfo;
            if (DbProvider == "System.Data.SqlClient")
            {
                if (Database.Length > 0)
                    connectionStringBuilder["Database"] = Database;
                if (AttachDbFilename.Length > 0)
                    connectionStringBuilder["AttachDBFilename"] = AttachDbFilename;
                if (ConnectionTimeout != 15)
                    connectionStringBuilder["Connection Timeout"] = ConnectionTimeout;
                connectionStringBuilder["Application Name"] = "CraigLib " + ApplicationConfig.AppVersion;
            }
            connectionStringBuilder["Pooling"] = Pooling;
            connectionStringBuilder["Min Pool Size"] = MinPoolSize;
            connectionStringBuilder["Max Pool Size"] = MaxPoolSize;
            if (ConnectionLifetime != 0)
                connectionStringBuilder["Connection Lifetime"] = ConnectionLifetime;
            if (!Enlist)
                connectionStringBuilder["Enlist"] = Enlist;
            foreach (var keyValuePair in _otherParams)
                connectionStringBuilder[keyValuePair.Key] = keyValuePair.Value;
            string str1;
            if (Password.Length > 0)
            {
                if (_dbSyntax == DatabaseType.ORACLE)
                {
                    var str2 = connectionStringBuilder.ToString();
                    if (str2.Length > 0)
                        str2 = str2 + ";";
                    str1 = str2 + "Password=\"" + Password + "\"";
                }
                else
                {
                    connectionStringBuilder.Add("Password", Password);
                    str1 = connectionStringBuilder.ToString();
                }
            }
            else
                str1 = connectionStringBuilder.ToString();
            return str1;
        }
    }
}
