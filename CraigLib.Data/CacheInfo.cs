using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Data
{
    public class CacheInfo
    {
        public Dictionary<string, CacheCriteriaStats> Retrieved;
        public string[] ChangeItems;
    }
}
