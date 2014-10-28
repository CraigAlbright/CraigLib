using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;

namespace CraigLib
{
    public class ApplicationConfig
    {
        private static string _appPath = "";
        public static string AppRelPath = null;
        public static List<string> BinAsmxList = null;
        private static ConnectionInfo _dbconnectinfo;
        public static ConnectionInfo PerfMonConnectInfo = null;
        private static string _appversion = "";
        private const string AppversionRev = "CraigLib V1";
        public static List<IPEndPoint> AzureEndPoints = new List<IPEndPoint>();
        private static bool _useActiveFtp = false;
        //private static bool recordLIMExecutionTime = false;
        private static bool _continueOnFtpError = false;
        private static bool _cacheIsolation = true;
        private static readonly NameValueCollection connectionStrings = new NameValueCollection();
        private static int _keepAliveInterval = 20000;
        public static int PassMaxAge = 90;
        public static int PassMinLen = 8;
        public static int PassSpecChar = 0;
        public static int PassNumChar = 1;
        public static int PassLcChar = 1;
        public static int PassUcChar = 1;
        public static int PassReuseMinAge = 180;
        public static bool ShowPerfInfo = false;
        public static int QueueUpdateSleep = 50;
        public static ICredentials Credentials = CredentialCache.DefaultCredentials;
        public static ICredentials ProxyCredentials = CredentialCache.DefaultCredentials;
        private static bool _isSqlAzure = false;
        private static bool _isAzure = false;
        private static bool _sqlLogEnabled;
        private static int _sqlLogLevel;
        private static int _dbCommandTimeout;
        private static string _smtpServer;
        private static string _smtpFaxServer;
        private static bool _smtpUseDefaultCredentials;
        private static bool _useMapi;
        private static string _messageFromUser;
        private static bool _fixLogFile;
        private static string _defaultExcelExtension;
        private static string _connectionFromUser;
        private static string _exceptionNotify;
        private static bool _unicodeDb;
        private static bool _useBulkCopy;
        private static bool _defaultSecurityPermission;
        private static bool _showExtraTimes;
        private static NameValueCollection _appSettings;
        private static string _helperUrl;
        private static string _baseUrl;

        public static string AppPath
        {
            get
            {
                return _appPath;
            }
            set
            {
                _appPath = value;
            }
        }

        public static string LogPath
        {
            get
            {
                return Path.Combine(_appPath, "logs");
            }
        }

        public static ConnectionInfo DbConnectInfo
        {
            get
            {
                return _dbconnectinfo;
            }
        }

        public static string AppVersion
        {
            get
            {
                if (_appversion.Length == 0)
                {
                    _appversion = GetVersionInfo().FileVersion;
                }
                return _appversion;
            }
            set
            {
                _appversion = value;
            }
        }

        public static string AppVersionRev
        {
            get
            {
                return AppversionRev;
            }
        }

        public static string ProductName
        {
            get
            {
                var str = AppSettings["ProductName"];
                if (string.IsNullOrEmpty(str))
                    str = GetVersionInfo().ProductName;
                return str;
            }
        }

        public static string MachineName { get; set; }

        public static bool RightToLeftLanguage { get; private set; }

        [Config("DataAccess.SqlLogEnabled", "Write SQL to sql.log", "False")]
        public static bool SqlLogEnabled
        {
            get
            {
                return _sqlLogEnabled;
            }
        }

        [Config("DataAccess.SqlLogLevel", "Determines which SQL is written to sql.log.  0=select and exceptions, >=1 logs all SQL", "0")]
        public static int SqlLogLevel
        {
            get
            {
                return _sqlLogLevel;
            }
        }

        [Config("DataAccess.CommandTimeout", "Database query timeout in seconds", "600")]
        public static int DbCommandTimeout
        {
            get
            {
                return _dbCommandTimeout;
            }
        }

        [Config("SmtpServer", "Defines the Simple Mail Transfer Protocol (SMTP) Server used to send email from the application. {IP Address} or {Host Name} of the SMTP Server.", "relay")]
        public static string SmtpServer
        {
            get
            {
                return _smtpServer;
            }
        }

        [Config("SmtpFaxServer", "Server that is used for outgoing SMTP faxes", "")]
        public static string SmtpFaxServer
        {
            get
            {
                return _smtpFaxServer;
            }
        }

        [Config("SmtpUseDefaultCredentials", "Use default credentials when sending via SMTP; Used to control if authentication is done when email is sent via SMTP.", "True")]
        public static bool SmtpUseDefaultCredentials
        {
            get
            {
                return _smtpUseDefaultCredentials;
            }
        }

        [Config("UseMAPI", "Use Client MAPI to send email", "False")]
        public static bool UseMAPI
        {
            get
            {
                return _useMapi;
            }
        }

        [Config("MessageFromUser", "Address used for event based messages", "self")]
        public static string MessageFromUser
        {
            get
            {
                return _messageFromUser;
            }
        }

        [Config("FixLogFile", "Enable/Disable FIX logging", "False")]
        public static bool FixLogFile
        {
            get
            {
                return _fixLogFile;
            }
        }

        [Config("DefaultExcelExtension", "Default Excel file extension (xls or xlsx)", "xls")]
        public static string DefaultExcelExtension
        {
            get
            {
                return _defaultExcelExtension;
            }
        }

        [Config("ConnectionFromUser", "Address used for connection jobs", "self")]
        public static string ConnectionFromUser
        {
            get
            {
                return _connectionFromUser;
            }
        }

        public static string ExceptionNotify
        {
            get
            {
                return _exceptionNotify;
            }
        }

        [Config("UnicodeDB", "Enable/Disable Unicode DB", "False")]
        public static bool UnicodeDB
        {
            get
            {
                return _unicodeDb;
            }
        }

        public static bool UseBulkCopy
        {
            get
            {
                return _useBulkCopy;
            }
        }

        [Config("DefaultSecurityPermission", "Security permission default", "True")]
        public static bool DefaultSecurityPermission
        {
            get
            {
                return _defaultSecurityPermission;
            }
        }

        public static bool ShowExtraTimes
        {
            get
            {
                return _showExtraTimes;
            }
        }

        [Config("FTP.UseActive", "Use active FTP instead of passive", "False")]
        public static bool UseActiveFTP
        {
            get
            {
                return _useActiveFtp;
            }
        }

        public static NameValueCollection AppSettings
        {
            get
            {
                return _appSettings;
            }
        }

        public static NameValueCollection ConnectionStrings
        {
            get
            {
                return connectionStrings;
            }
        }

        public static string HelperUrl
        {
            get
            {
                return _helperUrl;
            }
            set
            {
                _helperUrl = value;
            }
        }

        public static string BaseUrl
        {
            get
            {
                return _baseUrl;
            }
            set
            {
                _baseUrl = value;
            }
        }

        [Config("KeepAliveInterval", "Connection keepalive interval in seconds. Set to 0 for no keepalive.", "20")]
        public static int KeepAliveInterval
        {
            get
            {
                return _keepAliveInterval;
            }
        }

        public static bool DisableNewVarCalculation { get; private set; }

        public static bool IsAzure
        {
            get
            {
                return _isAzure;
            }
            set
            {
                _isAzure = value;
            }
        }

        public static bool IsSqlAzure
        {
            get
            {
                return _isSqlAzure;
            }
        }


        static ApplicationConfig()
        {
            try
            {
                _appPath = HttpRuntime.AppDomainAppId == null ? new FileInfo(GetAssembly().Location).DirectoryName : HttpRuntime.AppDomainAppPath;
                OnApplicationStart(_appPath, ConfigurationManager.AppSettings);
            }
            catch
            {
            }
        }

        private static FileVersionInfo GetVersionInfo()
        {
            return FileVersionInfo.GetVersionInfo(GetAssembly().Location);
        }


        private static void ReadConnectionStrings(ConnectionStringSettingsCollection connstrings)
        {
            connectionStrings.Clear();
            var crypt = new CryptoHelper();
            foreach (ConnectionStringSettings connectionStringSettings in connstrings)
            {
                if (connectionStringSettings.ConnectionString != null)
                {
                    if (connectionStringSettings.ConnectionString.Length > 5)
                    {
                        try
                        {
                            string connectionString = crypt.RC2Decrypt(connectionStringSettings.ConnectionString);
                            if (connectionString.Length > 0)
                                connectionStrings[connectionStringSettings.Name] = connectionString;
                            if (connectionStringSettings.Name.Equals("default", StringComparison.OrdinalIgnoreCase))
                            {
                                _dbconnectinfo = new ConnectionInfo(connectionString);
                                if (_dbconnectinfo.DataSource.Contains("database.windows.net"))
                                    _isSqlAzure = true;
                            }
                            else if (connectionStringSettings.Name.Equals("perfmon"))
                                PerfMonConnectInfo = new ConnectionInfo(connectionString);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public static void OnApplicationStart(string myAppPath, NameValueCollection myAppSettings)
        {
            MachineName = Environment.MachineName.ToLower();
            _appPath = myAppPath;
            _appSettings = myAppSettings;
            if (_appSettings == null)
                _appSettings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            _appSettings["dummy"] = "";
            ReadSettings(_appSettings);
            ReadConnectionStrings(ConfigurationManager.ConnectionStrings);
        }

        private static void ReadSettings(NameValueCollection settings)
        {
            _sqlLogEnabled = ReadSetting(settings, "DataAccess.SqlLogEnabled", false);
            _sqlLogLevel = ReadSetting(settings, "DataAccess.SqlLogLevel", 0);
            _dbCommandTimeout = ReadSetting(settings, "DataAccess.CommandTimeout", 600);
            _smtpServer = ReadSetting(settings, "SMTPServer", "relay");
            _smtpFaxServer = ReadSetting(settings, "SMTPFaxServer", "");
            _smtpUseDefaultCredentials = ReadSetting(settings, "SmtpUseDefaultCredentials", true);
            _useMapi = ReadSetting(settings, "UseMAPI", false);
            if (_smtpFaxServer.Length == 0)
                _smtpFaxServer = _smtpServer;

            _defaultSecurityPermission = ReadSetting(settings, "DefaultSecurityPermission", true);
            _showExtraTimes = ReadSetting(settings, "ShowExtraTimes", false);
            _useBulkCopy = ReadSetting(settings, "UseBulkCopy", false);
            RightToLeftLanguage = ReadSetting(settings, "RightToLeftLanguage", false);
            _messageFromUser = ReadSetting(settings, "MessageFromUser", "self");
            _connectionFromUser = ReadSetting(settings, "ConnectionFromUser", "self");
            _exceptionNotify = ReadSetting(settings, "ExceptionNotify", "");

            _cacheIsolation = ReadSetting(settings, "CacheIsolation", true);
            _useActiveFtp = ReadSetting(settings, "FTP.UseActive", false);
            _continueOnFtpError = ReadSetting(settings, "FTP.ContinueOnError", false);
            _fixLogFile = ReadSetting(settings, "FixLogFile", false);
            _defaultExcelExtension = ReadSetting(settings, "DefaultExcelExtension", "xls");
            _unicodeDb = ReadSetting(settings, "UnicodeDB", false);
            ShowPerfInfo = ReadSetting(settings, "ShowPerfInfo", false);
            QueueUpdateSleep = ReadSetting(settings, "QueueUpdateSleep", 50);
            DisableNewVarCalculation = ReadSetting(settings, "DisableNewVarCalculation", false);
            _keepAliveInterval = ReadSetting(settings, "KeepAliveInterval", 20)*1000;
            PassMaxAge = ReadSetting(settings, "PassMaxAge", 90);
            PassMinLen = ReadSetting(settings, "PassMinLen", 8);
            PassSpecChar = ReadSetting(settings, "PassSpecChar", 0);
            PassNumChar = ReadSetting(settings, "PassNumChar", 1);
            PassLcChar = ReadSetting(settings, "PassLcChar", 1);
            PassUcChar = ReadSetting(settings, "PassUcChar", 1);
            PassReuseMinAge = ReadSetting(settings, "PassReuseMinAge", 180);
            QueueUpdateSleep = ReadSetting(settings, "QueueUpdateSleep", 50);
        }

        private static void SetCredentials(string cred)
        {
            try
            {
                cred = new CryptoHelper().RC2Decrypt(cred);
            }
            catch
            {
            }
            Credentials = ProxyCredentials = new NetworkCredential(cred.Substring(0, cred.IndexOf(":")), cred.Substring(cred.IndexOf(":") + 1));
        }

        public static void ReadSettings(DataTable dt)
        {
            foreach (DataRow dataRow in (InternalDataCollectionBase)dt.Rows)
            {
                if (!dataRow.IsNull(2) && dataRow[2].ToString().Trim().Length > 0)
                {
                    if (dataRow[1].ToString().Trim() == "DefaultCredentials")
                        SetCredentials(dataRow[2].ToString());
                    else
                        AppSettings[dataRow[1].ToString().Trim()] = dataRow[2].ToString();
                }
            }
            ReadSettings(_appSettings);
        }

        private static string ReadSetting(NameValueCollection settings, string key, string defaultValue)
        {
            try
            {
                var str = settings[key];
                return str ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static bool ReadSetting(NameValueCollection settings, string key, bool defaultValue)
        {
            try
            {
                var str1 = settings[key];
                if (string.IsNullOrEmpty(str1))
                    return defaultValue;
                var str2 = str1.Trim();
                if (str2 == "0")
                    return false;
                if (str2 == "1")
                    return true;
                return Convert.ToBoolean(str2);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static int ReadSetting(NameValueCollection settings, string key, int defaultValue)
        {
            try
            {
                var str = settings[key];
                return str == null ? defaultValue : Convert.ToInt32(str);
            }
            catch
            {
                return defaultValue;
            }
        }


        private static Assembly GetAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly;
        }
    }
}
