namespace CraigLib.Data
{
    public interface ICacheable
    {
        CacheData RetrieveCacheItem(object currentitem, string[] tableNames, SelectCriteria criteria);

        bool CleanUpCacheItem(object item);
    }
}
