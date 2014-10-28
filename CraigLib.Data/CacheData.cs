using System;

namespace CraigLib.Data
{
    public class CacheData
    {
        public DateTime Expires = DateTime.MaxValue;
        public object NewData;
        public bool Append;
        public string[] WatchTables;

        public CacheData(object newdata, bool append)
        {
            NewData = newdata;
            Append = append;
        }

        public CacheData(object newdata, string[] watchtables)
        {
            NewData = newdata;
            WatchTables = watchtables;
            Append = true;
        }
    }
}
