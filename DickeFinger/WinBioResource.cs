using System;
using System.Diagnostics;

namespace DickeFinger
{
    public abstract class WinBioResource
        : WinBioObject
        , IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }

        ~WinBioResource()
        {
            Trace.WriteLine(ToString() + " leaked!", "Warning");
            Dispose(false);
        }

        protected abstract void Dispose(bool manual);
    }
}