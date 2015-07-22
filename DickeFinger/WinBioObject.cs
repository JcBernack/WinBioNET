using System;

namespace DickeFinger
{
    public class WinBioObject
        : IEquatable<WinBioObject>
    {
        private IntPtr _handle;

        protected void SetID(IntPtr handle)
        {
            _handle = handle;
        }

        public override bool Equals(object other)
        {
            if (!(other is WinBioObject)) return false;
            return Equals((WinBioObject) other);
        }

        public bool Equals(WinBioObject other)
        {
            if (other == null) return false;
            return _handle.Equals(other._handle);
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", GetType().Name, _handle);
        }
    }
}