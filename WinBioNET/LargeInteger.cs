using System.Runtime.InteropServices;

namespace WinBioNET
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct LargeInteger
    {
        [FieldOffset(0)] public long QuadPart;
        [FieldOffset(0)] public uint LowPart;
        [FieldOffset(4)] public int HighPart;
    }
}
