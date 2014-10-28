using System.Runtime.InteropServices;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileLoopInfo
    {
        public short TimeSignatureNumerator;
        public short TimeSignatureDenominator;
        public LibsndfileLoopMode LoopMode;
        public int BeatsCount;
        public float BeatsPerMinute;
        public int RootKey;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] Future;
    }
}
