using System;

namespace CraigLib.Data
{
    public class CacheStats
    {
        public string Info = "";
        public string DataType = "";
        public int Hit = 1;
        public TimeSpan IoTime = new TimeSpan();
        public TimeSpan MergeTime = new TimeSpan();
        public string Name;
        public int Miss;
        public int Clears;
        public int Items;

        public int HitRate
        {
            get
            {
                return Hit * 100 / (Hit + Miss);
            }
        }

        public CacheStats(string name)
        {
            Name = name;
        }
    }
}
