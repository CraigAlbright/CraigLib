using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileInstrumentInfo
    {
        public int Gain;
        public short BaseNote;
        public short Detune;
        public short VelocityLow;
        public short VelocityHigh;
        public short KeyLow;
        public short KeyHigh;
        public int LoopCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public LibsndfileLoop[] Loops;

        [StructLayout(LayoutKind.Sequential)]
        public struct LibsndfileLoop
        {
            public int Mode;
            public uint Start;
            public uint End;
            public uint Count;
        }
    }
}
