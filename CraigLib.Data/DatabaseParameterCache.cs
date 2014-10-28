using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace CraigLib.Data
{
    public static class DbParameterCache
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        static DbParameterCache()
        {
        }

        private static DbParameter[] DiscoverSpParameterSet(string spName, bool includeReturnValueParameter)
        {
            using (var newConnection = DatabaseHelper.GetNewConnection())
            {
                using (var command = newConnection.CreateCommand())
                {
                    DatabaseHelper.OpenConnection(newConnection);
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = newConnection;
                    var dbParameterArray = new DbParameter[command.Parameters.Count];
                    var type = command.GetType();
                    var fullName = type.Assembly.FullName;
                    var type1 = Type.GetType(type.FullName.Replace("Command", "CommandBuilder") + "," + fullName);
                    if (type1 != null)
                        type1.GetMethod("DeriveParameters").Invoke(null, new object[1]
                        {
                            command
                        });
                    if (!includeReturnValueParameter)
                        command.Parameters.RemoveAt(0);
                    command.Parameters.CopyTo(dbParameterArray, 0);
                    newConnection.Close();
                    return dbParameterArray;
                }
            }
        }

        private static DbParameter[] CloneParameters(DbParameter[] originalParameters)
        {
            var dbParameterArray = new DbParameter[originalParameters.Length];
            var index = 0;
            for (var length = originalParameters.Length; index < length; ++index)
                dbParameterArray[index] = (DbParameter)((ICloneable)originalParameters[index]).Clone();
            return dbParameterArray;
        }

        public static void CacheParameterSet(string commandText, params DbParameter[] commandParameters)
        {
            var str = ApplicationConfig.DbConnectInfo.ConnectionString + ":" + commandText;
            paramCache[str] = commandParameters;
        }

        public static DbParameter[] GetCachedParameterSet(string commandText)
        {
            var str = ApplicationConfig.DbConnectInfo.ConnectionString + ":" + commandText;
            var originalParameters = (DbParameter[])paramCache[str];
            if (originalParameters == null)
                return null;
            return CloneParameters(originalParameters);
        }

        public static DbParameter[] GetSpParameterSet(string spName)
        {
            return GetSpParameterSet(spName, false);
        }

        public static DbParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter)
        {
            var str = ApplicationConfig.DbConnectInfo.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : string.Empty);
            return CloneParameters((DbParameter[])paramCache[str] ?? (DbParameter[])(paramCache[str] = DiscoverSpParameterSet(spName, includeReturnValueParameter)));
        }
    }
}
