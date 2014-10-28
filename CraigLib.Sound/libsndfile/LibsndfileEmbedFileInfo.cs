using System.Runtime.InteropServices;

namespace CraigLib.Sound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LibsndfileEmbedFileInfo
    {
        public long Offset;
        public long Length;
    }
}
