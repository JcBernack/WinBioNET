using System;

namespace DickeFinger
{
    public class WinBioObject
        : IEquatable<WinBioObject>
    {
        public ulong Handle
        {
            get; protected set;
        }

        public override bool Equals(object other)
        {
            if (!(other is WinBioObject)) return false;
            return Equals((WinBioObject) other);
        }

        public bool Equals(WinBioObject other)
        {
            if (other == null) return false;
            return Handle.Equals(other.Handle);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + "(" + Handle.ToString() + ")";
        }
    }
}