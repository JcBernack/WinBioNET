using System;
using System.Security.Principal;
using DickeFinger.Enums;

namespace DickeFinger
{
    public class WinBioIdentity
    {
        public const int Size = 76;

        public WinBioIdentityType Type;
        public int Null;
        public int Wildcard;
        public Guid TemplateGuid;
        public int AccountSidSize;
        public SecurityIdentifier AccountSid;

        public WinBioIdentity(byte[] bytes)
        {
            Type = (WinBioIdentityType)BitConverter.ToInt32(bytes, 0);
            switch (Type)
            {
                case WinBioIdentityType.Null:
                    Null = BitConverter.ToInt32(bytes, 4);
                    break;
                case WinBioIdentityType.Wildcard:
                    Wildcard = BitConverter.ToInt32(bytes, 4);
                    break;
                case WinBioIdentityType.GUID:
                    var guidBytes = new byte[16];
                    Array.Copy(bytes, 4, guidBytes, 0, 16);
                    TemplateGuid = new Guid(guidBytes);
                    break;
                case WinBioIdentityType.SID:
                    AccountSidSize = BitConverter.ToInt32(bytes, 4);
                    AccountSid = new SecurityIdentifier(bytes, 8);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case WinBioIdentityType.Null: return string.Format("Null ({0})", Null);
                case WinBioIdentityType.Wildcard: return string.Format("Wildcard ({0})", Wildcard);
                case WinBioIdentityType.GUID: return string.Format("GUID ({0})", TemplateGuid);
                case WinBioIdentityType.SID: return string.Format("SID ({0})", AccountSid);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}