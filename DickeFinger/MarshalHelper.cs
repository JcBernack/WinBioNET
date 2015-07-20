using System;
using System.Runtime.InteropServices;

namespace DickeFinger
{
    public static class MarshalHelper
    {
        public static T[] MarshalArrayOfStruct<T>(IntPtr pointer, int count)
        {
            var data = new T[count];
            for (var i = 0; i < count; i++)
            {
                data[i] = (T)Marshal.PtrToStructure(pointer, typeof(T));
                pointer += Marshal.SizeOf(typeof(WinBioStorageSchema));
            }
            return data;
        }
    }
}