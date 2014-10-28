using System;
using System.Collections.Generic;

namespace CraigLib.Data
{
    public class CacheCriteriaStats
    {
        public Dictionary<string, string> WatchItems = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public int Hit;
        public int Items;
        public string Info;
        public string Key;
        public TimeSpan IoTime;
        public object Item;

        public CacheCriteriaStats(string key)
        {
            Key = key;
        }

        public CacheCriteriaStats(string key, Dictionary<string, string> watchitems)
        {
            Key = key;
            MergeWatchItems(watchitems.Keys);
        }

        public void MergeWatchItems(ICollection<string> watchitems)
        {
            if (watchitems == null)
                return;
            foreach (var index in watchitems)
                WatchItems[index] = "";
        }
    }
}
