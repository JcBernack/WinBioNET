namespace WinBioNET
{
    public struct WinBioBdbAnsi381Record
    {
        public uint BlockLength;
        public ushort HorizontalLineLength;
        public ushort VerticalLineLength;
        public byte Position;
        public byte CountOfViews;
        public byte ViewNumber;
        public byte ImageQuality;
        public byte ImpressionType;
        public byte Reserved;
    }
}
