using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Sound
{
    /// <summary>
    /// Interface to provide support for marshalling arrays.
    /// </summary>
    internal interface ILibsndfileArrayMarshal
    {
        T[] ToArray<T>(UnmanagedMemoryHandle memory) where T : struct;
    }
}
