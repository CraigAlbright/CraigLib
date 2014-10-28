using System.Runtime.InteropServices;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileFormatInfo
    {
        public LibsndfileFormat Format;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Extension;
    }
}
