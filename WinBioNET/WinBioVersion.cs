namespace WinBioNET
{
    public struct WinBioVersion
    {
        public int MajorVersion;
        public int MinorVersion;

        public override string ToString()
        {
            return string.Format("{0}.{1}", MajorVersion, MinorVersion);
        }
    }
}