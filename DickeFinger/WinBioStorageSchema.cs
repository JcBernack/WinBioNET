using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public struct WinBioStorageSchema
    {
        public WinBioBiometricType Factor;
        public Guid DatabaseId;
        public Guid DataFormat;
        public WinBioDatabaseFlag Attributes;
        public IntPtr FilePath;
        public IntPtr ConnectionString;
    }
}