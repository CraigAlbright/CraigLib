using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace CraigLib.Data
{
    public class CacheItem
    {
        private readonly object _locker = new object();
        private DateTime _expires = DateTime.MaxValue;
        private readonly Dictionary<string, string> _watchtables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _hints = new Dictionary<string, string>();
        private readonly Dictionary<string, CacheCriteriaStats> _retrieved = new Dictionary<string, CacheCriteriaStats>();
        private readonly string _name;
        private readonly ICacheable _cacheable;
        private object _cacheitem;
        private bool _stale;
        public CacheStats Cstats;
        public bool IsVolatile;
        public bool IsSegmented;
        public volatile bool DeferClear;

        public CacheItem(string name, ICacheable cacheable, Dictionary<string, string> watchtables, Dictionary<string, string> hints, bool segmented, bool deferclear)
        {
            _name = name;
            _cacheable = cacheable;
            _watchtables = watchtables;
            _hints = hints;
            IsSegmented = segmented;
            DeferClear = deferclear;
            Cstats = new CacheStats(name);
        }

        public CacheItem(string name, ICacheable cacheable, string[] watchtables, string[] hints)
            : this(name, cacheable, watchtables, hints, false, false)
        {
        }

        public CacheItem(string name, ICacheable cacheable, IEnumerable<string> watchtables, IEnumerable<string> hints, bool segmented, bool deferclear)
        {
            _name = name;
            _cacheable = cacheable;
            IsSegmented = segmented;
            DeferClear = deferclear;
            if (watchtables != null)
            {
                foreach (var index in watchtables)
                    _watchtables[index] = string.Empty;
            }
            if (hints == null)
                return;
            foreach (var index in hints)
                _hints[index] = string.Empty;
        }

        public bool IsAffected(string[] items)
        {
            if (_cacheitem == null && !IsSegmented || (items == null || items.Length == 0))
                return false;
            if (!string.IsNullOrEmpty(items[0]) && _hints.ContainsKey(items[0]))
                return true;
            for (var index = 1; index < items.Length; ++index)
            {
                if (!string.IsNullOrEmpty(items[index]) && _watchtables.ContainsKey(items[index]))
                    return true;
            }
            return false;
        }

        private void AquireLock(string method)
        {
            if (Monitor.TryEnter(_locker, 60000))
                return;
            if (Monitor.TryEnter(_locker, 60000))
                return;
            if (!Monitor.TryEnter(_locker, 60000))
                throw new CacheTimeoutException("Timeout during " + method + " on " + _name);
        }

        public void Clear(string[] items)
        {
            if (IsSegmented || _name.EndsWith("DbContent") || _name.EndsWith("DatabaseModel"))
                _cacheable.CleanUpCacheItem(new CacheInfo
                {
                    Retrieved = _retrieved,
                    ChangeItems = items
                });
            if (DeferClear)
            {
                _stale = true;
            }
            else
            {
                AquireLock("clear");
                try
                {
                    SetAsCleared(true);
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
                
            }
        }

        private void SetAsCleared(bool setinfo)
        {
            _retrieved.Clear();
            _cacheitem = null;
            ++Cstats.Clears;
            Cstats.Items = 0;
            if (!setinfo)
                return;
            Cstats.Info = "Cleared";
        }

        public object Get()
        {
            return Get(new[]
      {
        "All"
      }, null);
        }

        public object Get(string[] tableNames, SelectCriteria criteria)
        {
            object currentitem;
            var key = "";
            if (tableNames != null)
                key = string.Join(",", tableNames) + ",";
            if (criteria != null)
                key = key + DataSetHelper.SelectCriteriaToString(criteria);
            AquireLock("get");
            try
            {
                if (_stale || DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified) > _expires)
                    SetAsCleared(false);
                CacheCriteriaStats ccstat;
                if ((_cacheitem != null || IsSegmented) && _retrieved.TryGetValue(key, out ccstat))
                {
                    ++ccstat.Hit;
                    ++Cstats.Hit;
                }
                else
                {
                    var dateTime1 = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    CacheData cacheData = _cacheable.RetrieveCacheItem(_cacheitem, tableNames, criteria);
                    if (cacheData.WatchTables != null)
                    {
                        foreach (string index in cacheData.WatchTables)
                            _watchtables[index] = "";
                    }
                    var timeSpan = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified) - dateTime1;
                    ccstat = _retrieved[key] = new CacheCriteriaStats(key, _watchtables);
                    ccstat.MergeWatchItems(cacheData.WatchTables);
                    Cstats.IoTime += timeSpan;
                    ccstat.IoTime = timeSpan;
                    GetItemInfo(cacheData.NewData, ccstat);
                    _expires = cacheData.Expires;
                    if (_cacheitem != null && cacheData.Append)
                    {
                        var dateTime2 = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                        if (cacheData.NewData is DataTable)
                            DTImport((DataTable)_cacheitem, (DataTable)cacheData.NewData);
                        else if (cacheData.NewData is DataSet)
                            DsImport((DataSet)_cacheitem, (DataSet)cacheData.NewData);
                        Cstats.MergeTime += DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified) - dateTime2;
                    }
                    if (_stale || !cacheData.Append || IsSegmented && Cstats.Items > 500000)
                    {
                        SetAsCleared(false);
                        if (!_stale)
                            _retrieved[key] = ccstat;
                    }
                    ++Cstats.Miss;
                    if (IsSegmented)
                    {
                        ccstat.Item = cacheData.NewData;
                        _cacheitem = null;
                    }
                    else
                        _cacheitem = cacheData.NewData;
                    GetItemInfo(cacheData.NewData, null);
                }
                currentitem = !IsSegmented ? _cacheitem : ccstat.Item;
            }
            finally
            {
                Monitor.Exit(_locker);
            }
            return currentitem;
        }

        private void GetItemInfo(object item, CacheCriteriaStats ccstat)
        {
            var str = "";
            var num1 = 0;
            Cstats.DataType = item.GetType().Name;

            var set = item as DataSet;
            if (set != null)
            {
                foreach (DataTable dataTable in set.Tables)
                {
                    str = str + dataTable.TableName + ": " + dataTable.Rows.Count + " ";
                    num1 += dataTable.Rows.Count;
                }
            }
            else
            {
                var table = item as DataTable;
                if (table != null)
                {
                    var dataTable = table;
                    str =
                        (string)
                            (string.IsNullOrEmpty(dataTable.TableName) ? (object) "Items" : (object) dataTable.TableName) +
                        (object) ": " + (string) (object) dataTable.Rows.Count;
                    num1 = dataTable.Rows.Count;
                }
                else
                {
                    var property = item.GetType().GetProperty("Count") ?? item.GetType().GetProperty("Length");
                    if (property != null)
                    {
                        num1 = Convert.ToInt32(property.GetValue(item, null));
                        str = "Items: " + num1;
                    }
                    else
                        str = item.ToString();
                }
            }

            if (ccstat != null)
            {
                ccstat.Items = num1;
                ccstat.Info = str;
            }
            else if (!IsSegmented)
            {
                Cstats.Items = num1;
                Cstats.Info = str;
            }
            else
            {
                var num2 = _retrieved.Values.Where(cacheCriteriaStats => cacheCriteriaStats.Info != "Cleared").Sum(cacheCriteriaStats => cacheCriteriaStats.Items);
                Cstats.Info = "Segmented";
                Cstats.Items = num2;
            }
        }

        private void DsImport(DataSet dsfrom, DataSet dsto)
        {
            dsto.EnforceConstraints = false;
            foreach (var key in dsfrom.ExtendedProperties.Keys)
            {
                if (!dsto.ExtendedProperties.ContainsKey(key))
                    dsto.ExtendedProperties[key] = dsfrom.ExtendedProperties[key];
            }
            foreach (DataTable dtfrom in dsfrom.Tables)
            {
                if (dsto.Tables.Contains(dtfrom.TableName))
                    DTImport(dtfrom, dsto.Tables[dtfrom.TableName]);
            }
            dsto.AcceptChanges();
        }

        private void DTImport(DataTable dtfrom, DataTable dtto)
        {
            foreach (var key in dtfrom.ExtendedProperties.Keys)
            {
                if (!dtto.ExtendedProperties.ContainsKey(key))
                    dtto.ExtendedProperties[key] = dtfrom.ExtendedProperties[key];
            }
            var primaryKey = dtfrom.PrimaryKey;
            var keys = new object[primaryKey.Length];
            dtto.BeginLoadData();
            foreach (DataRow dataRow in (InternalDataCollectionBase)dtfrom.Rows)
            {
                for (var index = 0; index < primaryKey.Length; ++index)
                    keys[index] = dataRow[primaryKey[index]];
                if (dtto.Rows.Find(keys) == null)
                    dtto.LoadDataRow(dataRow.ItemArray, true);
            }
            dtto.EndLoadData();
            dtto.AcceptChanges();
        }

        public CacheItem Copy(string name)
        {
            return new CacheItem(name, _cacheable, _watchtables, _hints, IsSegmented, DeferClear);
        }
    }
}
