using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public class WinBioSession
        : WinBioResource
    {
        private readonly WinBioSessionHandle _handle;

        public WinBioSession()
            : this(WinBioPoolType.System, WinBioSessionFlag.Default)
        {
        }

        public WinBioSession(WinBioPoolType poolType, WinBioSessionFlag sessionFlags)
        {
            _handle = WinBio.OpenSession(WinBioBiometricType.Fingerprint, poolType, sessionFlags);
            SetID(_handle.Value);
            Console.WriteLine("WinBioSession opened: " + _handle.Value);
        }

        protected override void Dispose(bool manual)
        {
            WinBio.CloseSession(_handle);
            Console.WriteLine("WinBioSession closed");
        }

        public void LocateSensor()
        {
            var unitId = WinBio.LocateSensor(_handle);
            Console.WriteLine("Sensor located: UnitID = {0}", unitId);
        }

        public void Identify()
        {
            WinBioIdentity identity;
            WinBioBiometricSubType subFactor;
            WinBioRejectDetail rejectDetail;
            var unitId = WinBio.Identify(_handle, out identity, out subFactor, out rejectDetail);
            Console.WriteLine("Unit Id: {0}", unitId);
            Console.WriteLine("Identity: {0}", identity);
            Console.WriteLine("Sub factor: {0}", subFactor);
            Console.WriteLine("Reject details: {0}", rejectDetail);
        }

        public void CaptureSample(WinBioBirPurpose purpose, WinBioBirDataFlags dataFlags)
        {
            int sampleSize;
            WinBioRejectDetail rejectDetail;
            var unitId = WinBio.CaptureSample(_handle, purpose, dataFlags, out sampleSize, out rejectDetail);
            Console.WriteLine("Unit id: {0}", unitId);
            Console.WriteLine("Captured sample size: {0}", sampleSize);
            Console.WriteLine("Reject details: {0}", rejectDetail);
        }
    }
}