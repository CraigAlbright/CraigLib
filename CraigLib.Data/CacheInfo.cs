using System.Collections.Generic;

namespace CraigLib.Data
{
    public class CacheInfo
    {
        public Dictionary<string, CacheCriteriaStats> Retrieved;
        public string[] ChangeItems;
    }
}
