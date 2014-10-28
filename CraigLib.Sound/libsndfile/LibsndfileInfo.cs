using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileInfo
    {
        public long Frames;
        public int SampleRate;
        public int Channels;
        public LibsndfileFormat Format;
        public int Sections;
        public int Seekable;

        internal bool IsSet
        {
            get { return Format != 0 && Channels > 0 && SampleRate > 0; }
        }
    }
}
