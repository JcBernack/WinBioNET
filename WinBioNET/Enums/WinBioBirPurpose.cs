using System;

namespace WinBioNET.Enums
{
    [Flags]
    public enum WinBioBirPurpose
    {
        NoPurposeAvailable = 0,
        Verify = 1,
        Identify = 2,
        Enroll = 4,
        EnrollForVerification = 8,
        EnrollForIdentification = 16
    }
}