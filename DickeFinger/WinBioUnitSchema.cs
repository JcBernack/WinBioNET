namespace DickeFinger
{
    public struct WinBioUnitSchema
    {
        public int UnitId;
        public int PoolType;
        public int BiometricFactor;
        public int SensorSubType;
        public int Capabilities;
        public short DeviceInstanceId;
        public short Description;
        public short Manufacturer;
        public short Model;
        public short SerialNumber;
        public WinBioVersion FirmwareVersion;
    }
}