using System.Runtime.InteropServices;
using DickeFinger.Enums;

namespace DickeFinger
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WinBioUnitSchema
    {
        public int UnitId;
        public WinBioPoolType PoolType;
        public WinBioBiometricType BiometricFactor;
        public WinBioSensorSubType SensorSubType;
        public WinbioCapabilitySensor Capabilities;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DeviceInstanceId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Manufacturer;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Model;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string SerialNumber;
        public WinBioVersion FirmwareVersion;
    }
}