using System;
using System.Runtime.InteropServices;
using WinBioNET.Enums;

namespace WinBioNET
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WinBioBspSchema
    {
        public WinBioBiometricType BiometricFactor;
        public Guid BspId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Vendor;
        public WinBioVersion Version;
    }
}