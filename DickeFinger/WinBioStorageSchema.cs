using System;
using System.Runtime.InteropServices;
using DickeFinger.Enums;

namespace DickeFinger
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WinBioStorageSchema
    {
        public WinBioBiometricType Factor;
        public Guid DatabaseId;
        public Guid DataFormat;
        public WinBioDatabaseFlag Attributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FilePath;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ConnectionString;
    }
}