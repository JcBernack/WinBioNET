using System;

namespace WinBioNET.Enums
{
    [Flags]
    public enum WinBioBirDataFlags
    {
        Integrity = 0x01,
        Privacy = 0x02,
        Signed = 0x04,

        Raw = 0x20,
        Intermediate = 0x40,
        Processed = 0x80,
    }
}