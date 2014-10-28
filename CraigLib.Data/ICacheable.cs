using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Data
{
    public interface ICacheable
    {
        CacheData RetrieveCacheItem(object currentitem, string[] tableNames, SelectCriteria criteria);

        bool CleanUpCacheItem(object item);
    }
}
