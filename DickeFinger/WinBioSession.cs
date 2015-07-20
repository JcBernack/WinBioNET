using System;
using DickeFinger.Enums;
using PInvoker.Marshal;
using WinBio;

namespace DickeFinger
{
    public class WinBioSession
        : WinBioResource
    {
        private WinBioSessionHandle _handle;

        public WinBioSession()
            : this(WinBioPoolType.System, WinBioSessionFlag.Default)
        {
        }

        public WinBioSession(WinBioPoolType poolType, WinBioSessionFlag sessionFlags)
        {
            ArrayPtr<int> unitArray = new ArrayPtr<int>(new IntPtr(1));
            var code = WinBio.OpenSession(WinBioBiometricType.Fingerprint, poolType, sessionFlags, unitArray, 1, new GUID(), out _handle);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioOpenSession failed");
            Console.WriteLine("WinBioSession opened: " + _handle.Value);
        }

        protected override void Dispose(bool manual)
        {
            if (!_handle.IsValid) return;
            var code = WinBio.CloseSession(_handle);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioCloseSession failed");
            _handle.Invalidate();
            Console.WriteLine("WinBioSession closed");
        }

        public int LocateSensor()
        {
            int unitId;
            var code = WinBio.LocateSensor(_handle, out unitId);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioLocateSensor failed");
            Console.WriteLine("Sensor located: UnitID = {0}", unitId);
            return unitId;
        }

        public void Identify()
        {
            var unitId = new ArrayPtr<WINBIO_UNIT_ID>();
            var identity = new WINBIO_IDENTITY();
            var subFactor = new ByteArrayPtr();
            WinBioRejectDetail rejectDetail;
            var code = WinBio.WinBioIdentify(_handle, unitId, identity, subFactor, out rejectDetail);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioIdentify failed");
            Console.WriteLine("Identity: {0}", identity.Value.AccountSid.Data);
            Console.WriteLine("Sub factor: {0}", subFactor);
            Console.WriteLine("Reject details: {0}", rejectDetail);
        }

        public void CaptureSample()
        {
            var unitId = new ArrayPtr<WINBIO_UNIT_ID>();
            var sample = new ArrayPtr<PWINBIO_BIR>();
            int sampleSize;
            WinBioRejectDetail rejectDetail;
            var code = WinBio.CaptureSample(_handle, WinBioBirPurpose.NoPurposeAvailable, WinBioBirDataFlags.Raw, unitId, sample, out sampleSize, out rejectDetail);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioCaptureSample failed");
            Console.WriteLine("Captured sample size: {0}", sampleSize);
        }
    }
}