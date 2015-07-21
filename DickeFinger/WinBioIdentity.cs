using System.Runtime.InteropServices;
using DickeFinger.Enums;

namespace DickeFinger
{
    [StructLayout(LayoutKind.Explicit, Size = 76)]
    public struct WinBioIdentity
    {
        [FieldOffset(0)]
        public WinBioIdentityType Type;

        [FieldOffset(4)]
        public int Null;

        //[FieldOffset(4)]
        //public int Wildcard;

        //[FieldOffset(4)]
        //public Guid TemplateGuid;

        //[FieldOffset(4)]
        //public WinBioAccountSid AccountSidSize;
    }

    public struct WinBioAccountSid
    {
        public int AccountSidSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
        public char[] AccountSid;
    }
}