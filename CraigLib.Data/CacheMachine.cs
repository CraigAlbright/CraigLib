using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CraigLib.Data
{
    public static class CacheMachine
    {
        private static readonly Dictionary<string, CacheItem> Cacheitems = new Dictionary<string, CacheItem>();
        private static readonly object Locker = new object();
        [ThreadStatic] public static string InstanceName;
        [ThreadStatic]
        private static string _volsuffix;

        static CacheMachine()
        {
        }

        public static object Get(string name)
        {
            return Get(name, null, null);
        }

        public static object Get(string name, string[] tableNames, SelectCriteria criteria)
        {
           CacheItem cacheItem = null;
            lock (Locker)
            {
                if (_volsuffix != null)
                    Cacheitems.TryGetValue(name + _volsuffix, out cacheItem);
                if (InstanceName != null)
                {
                    var local1 = name + InstanceName;
                    Cacheitems.TryGetValue(local1, out cacheItem);
                    if (cacheItem == null && Cacheitems.TryGetValue("Base:" + name, out cacheItem))
                    {
                        Cacheitems[local1] = cacheItem.Copy(local1);
                        cacheItem = Cacheitems[local1];
                    }
                }
                if (cacheItem == null)
                    Cacheitems.TryGetValue(name, out cacheItem);
                if (cacheItem == null)
                {
                    if (Cacheitems.TryGetValue("Base:" + name, out cacheItem))
                    {
                        Cacheitems[name] = cacheItem.Copy(name);
                        cacheItem = Cacheitems[name];
                    }
                }
            }
            if (cacheItem == null)
                return null;
            if (tableNames == null && criteria == null)
                return cacheItem.Get();
            return cacheItem.Get(tableNames, criteria);
        }

        public static void AddCacheDef(string name, ICacheable cacheable, string[] watchtables, string[] hints)
        {
            AddCacheDef(name, cacheable, watchtables, hints, false, false);
        }

        public static void AddCacheDef(string name, ICacheable cacheable, string[] watchtables, string[] hints, bool replace)
        {
            AddCacheDef(name, cacheable, watchtables, hints, false, false);
        }

        public static void AddCacheDef(string name, ICacheable cacheable, string[] watchtables, string[] hints, bool replace, bool segmented)
        {
            var deferclear = segmented;
            var index = "Base:" + name;
            lock (Locker)
            {
                if (!replace && Cacheitems.ContainsKey(index))
                    return;
                Cacheitems[index] = new CacheItem(index, cacheable, watchtables, hints, segmented, deferclear);
                if (!replace)
                    return;
                foreach (var item0 in Cacheitems)
                {
                    if (item0.Key == name || item0.Key.StartsWith(name + "/"))
                        Cacheitems[item0.Key] = new CacheItem(item0.Key, cacheable, watchtables, hints, segmented, deferclear);
                }
            }
        }

        public static void UpdateNotify(string[] items)
        {
            var list = new List<CacheItem>();
            lock (Locker)
            {
                list.AddRange(from item0 in Cacheitems
                    where !item0.Key.StartsWith("Base:") && item0.Value.IsAffected(items)
                    select item0.Value);
            }
            foreach (var cacheItem in list)
                cacheItem.Clear(items);
        }

        public static void CreateVolatileItem(string name)
        {
            _volsuffix = "/t" + Thread.CurrentThread.ManagedThreadId;
            lock (Locker)
            {
                CacheItem local0;
                if (!Cacheitems.TryGetValue("Base:" + name, out local0))
                    throw new KeyNotFoundException("Cache item '" + name + "' is not defined");
                Cacheitems[name + _volsuffix] = local0.Copy(name + _volsuffix);
                Cacheitems[name + _volsuffix].IsVolatile = true;
            }
        }

        internal static void ClearVolatileItems()
        {
            if (_volsuffix == null)
                return;
            lock (Locker)
            {
                var local0 = (from item0 in Cacheitems where item0.Value.IsVolatile && item0.Key.EndsWith(_volsuffix) select item0.Key).ToList();
                foreach (var item1 in local0)
                    Cacheitems.Remove(item1);
            }
            _volsuffix = null;
        }
    }
}
