namespace WinBioNET
{
    public struct WinBioBdbAnsi381Header
    {
        public ulong RecordLength;
        public uint FormatIdentifier;
        public uint VersionNumber;
        public WinBioRegisteredFormat ProductId;
        public ushort CaptureDeviceId;
        public ushort ImageAcquisitionLevel;
        public ushort HorizontalScanResolution;
        public ushort VerticalScanResolution;
        public ushort HorizontalImageResolution;
        public ushort VerticalImageResolution;
        public byte ElementCount;
        public byte ScaleUnits;
        public byte PixelDepth;
        public byte ImageCompressionAlg;
        public ushort Reserved;
    }
}
