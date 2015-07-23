using System;
using System.Diagnostics;

namespace WinBioNET
{
    public struct WinBioSessionHandle
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IntPtr _value;

        /// <summary>
        /// Gets a logic value indicating whether the handle is valid.
        /// </summary>
        public bool IsValid
        {
            get { return _value != IntPtr.Zero; }
        }

        /// <summary>
        /// Gets the value of the handle.
        /// </summary>
        public IntPtr Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Invalidates the handle.
        /// </summary>
        public void Invalidate()
        {
            _value = IntPtr.Zero;
        }
    }
}