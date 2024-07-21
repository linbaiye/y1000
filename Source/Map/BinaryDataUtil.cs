using System;
using System.Runtime.InteropServices;

namespace y1000.Source.Map;

public static class BinaryDataUtil
{   
    public static T BytesToStructure<T>(byte[] bytes)
    {
        int size = Marshal.SizeOf(typeof(T));
        if (bytes.Length < size)
            size = bytes.Length;
        IntPtr ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(bytes, 0, ptr, size);
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}