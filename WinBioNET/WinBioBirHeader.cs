namespace WinBioNET
{
    public struct WinBioBirHeader
    {
        public ushort ValidFields;
        public byte HeaderVersion;
        public byte PatronHeaderVersion;
        public byte DataFlags;
        public uint Type;
        public byte Subtype;
        public byte Purpose;
        public sbyte DataQuality;
        public LargeInteger CreationDate;
        public Period ValidityPeriod;
        public WinBioRegisteredFormat BiometricDataFormat;
        public WinBioRegisteredFormat ProductId;
    }

    public struct Period
    {
        public LargeInteger BeginDate;
        public LargeInteger EndDate;
    }
}
