using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileBroadcastInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Description;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Originator;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string OriginatorReference;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string OriginationDate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string OriginationTime;

        public uint TimeReferenceLow;
        public uint TimeReferenceHigh;
        public short Version;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Umid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 190)]
        public string Reserved;

        public uint CodingHistorySize;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CodingHistory;
    }
}
