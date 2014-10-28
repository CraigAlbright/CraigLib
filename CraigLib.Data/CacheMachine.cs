using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CraigLib.Data
{
    public static class CacheMachine
    {
        private static Dictionary<string, CacheItem> _cacheitems = new Dictionary<string, CacheItem>();
        private static object _locker = new object();
        [ThreadStatic]
        public static string InstanceName = null;
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
            lock (_locker)
            {
                if (_volsuffix != null)
                    _cacheitems.TryGetValue(name + _volsuffix, out cacheItem);
                if (InstanceName != null)
                {
                    var local_1 = name + InstanceName;
                    _cacheitems.TryGetValue(local_1, out cacheItem);
                    if (cacheItem == null && _cacheitems.TryGetValue("Base:" + name, out cacheItem))
                    {
                        _cacheitems[local_1] = cacheItem.Copy(local_1);
                        cacheItem = _cacheitems[local_1];
                    }
                }
                if (cacheItem == null)
                    _cacheitems.TryGetValue(name, out cacheItem);
                if (cacheItem == null)
                {
                    if (_cacheitems.TryGetValue("Base:" + name, out cacheItem))
                    {
                        _cacheitems[name] = cacheItem.Copy(name);
                        cacheItem = _cacheitems[name];
                    }
                }
            }
            if (cacheItem == null)
                return null;
            if (tableNames == null && criteria == null)
                return cacheItem.Get();
            else
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
            lock (_locker)
            {
                if (!replace && _cacheitems.ContainsKey(index))
                    return;
                _cacheitems[index] = new CacheItem(index, cacheable, watchtables, hints, segmented, deferclear);
                if (!replace)
                    return;
                foreach (var item_0 in _cacheitems)
                {
                    if (item_0.Key == name || item_0.Key.StartsWith(name + "/"))
                        _cacheitems[item_0.Key] = new CacheItem(item_0.Key, cacheable, watchtables, hints, segmented, deferclear);
                }
            }
        }

        public static void UpdateNotify(string[] items)
        {
            var list = new List<CacheItem>();
            lock (_locker)
            {
                list.AddRange(from item_0 in _cacheitems where !item_0.Key.StartsWith("Base:") && item_0.Value.IsAffected(items) select item_0.Value);
            }
            foreach (var cacheItem in list)
                cacheItem.Clear(items);
        }

        public static void CreateVolatileItem(string name)
        {
            _volsuffix = "/t" + Thread.CurrentThread.ManagedThreadId;
            lock (_locker)
            {
                CacheItem local0;
                if (!_cacheitems.TryGetValue("Base:" + name, out local0))
                    throw new KeyNotFoundException("Cache item '" + name + "' is not defined");
                _cacheitems[name + _volsuffix] = local0.Copy(name + _volsuffix);
                _cacheitems[name + _volsuffix].IsVolatile = true;
            }
        }

        internal static void ClearVolatileItems()
        {
            if (_volsuffix == null)
                return;
            lock (_locker)
            {
                var local_0 = new List<string>();
                foreach (var item_0 in _cacheitems)
                {
                    if (item_0.Value.IsVolatile && item_0.Key.EndsWith(_volsuffix))
                        local_0.Add(item_0.Key);
                }
                foreach (var item_1 in local_0)
                    _cacheitems.Remove(item_1);
            }
            _volsuffix = null;
        }
    }
}
