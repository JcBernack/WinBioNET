using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DickeFinger.Enums;
using PInvoker.Marshal;
using WinBio;

namespace DickeFinger
{
    public struct WinBioUnit
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _id;

        public int Id
        {
            get { return _id; }
        }
    }

    public struct WinBioStorageSchema
    {
        public int Factor;
        public _GUID DatabaseId;
        public _GUID DataFormat;
        public int Attributes;
        public short FilePath;
        public short ConnectionString;
    }

    //[SuppressUnmanagedCodeSecurity]
    public class WinBio
    {
        protected const string LibName = "winbio.dll";

        [DllImport(LibName, EntryPoint = "WinBioOpenSession")]
        private extern static WinBioErrorCode OpenSession(
            WinBioBiometricType factor,
            WinBioPoolType poolType,
            WinBioSessionFlag flags,
            IntPtr unitArray,
            int unitCount,
            IntPtr databaseId,
            out WinBioSessionHandle sessionHandle);

        public static WinBioErrorCode OpenSession(
            WinBioBiometricType factor,
            WinBioPoolType poolType,
            WinBioSessionFlag flags,
            ArrayPtr<int> unitArray,
            int unitCount,
            GUID databaseId,
            out WinBioSessionHandle sessionHandle)
        {
            IPin pin1 = null;
            IPin pin2 = null;
            try
            {
                var pointer1 = IntPtr.Zero;
                var pointer2 = IntPtr.Zero;
                if (unitArray != null)
                {
                    pin1 = unitArray.Pin();
                    pointer1 = pin1.Pointer;
                }
                if (databaseId != null)
                {
                    pin2 = databaseId.Pin();
                    pointer2 = pin2.Pointer;
                }
                return OpenSession(factor, poolType, flags, pointer1, unitCount, pointer2, out sessionHandle);
            }
            finally
            {
                if (pin1 != null) pin1.Dispose();
                if (pin2 != null) pin2.Dispose();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioCloseSession")]
        public extern static WinBioErrorCode CloseSession(
            WinBioSessionHandle sessionHandle);


        [DllImport(LibName, EntryPoint = "WinBioEnumDatabases")]
        private static extern WinBioErrorCode EnumDatabases(
            WinBioBiometricType factor,
            IntPtr storageSchemaArray,
            out int storageCount);

        public static WinBioErrorCode EnumDatabases(
            WinBioBiometricType factor,
            ArrayPtr<WINBIO_STORAGE_SCHEMA> storageSchemaArray,
            out int storageCount)
        {
            IPin pin1 = null;
            try
            {
                var pointer1 = IntPtr.Zero;
                if (storageSchemaArray != null)
                {
                    pin1 = storageSchemaArray.Pin();
                    pointer1 = pin1.Pointer;
                }
                return EnumDatabases(factor, pointer1, out storageCount);
            }
            finally
            {
                if (pin1 != null) pin1.Dispose();
            }
        }


        [DllImport(LibName, EntryPoint = "WinBioCaptureSample")]
        private extern static WinBioErrorCode CaptureSample(
            WinBioSessionHandle sessionHandle,
            WinBioBirPurpose purpose,
            WinBioBirDataFlags flags,
            IntPtr unitId,
            IntPtr sample,
            out int sampleSize,
            out WinBioRejectDetail rejectDetail);

        public static WinBioErrorCode CaptureSample(
            WinBioSessionHandle sessionHandle,
            WinBioBirPurpose purpose,
            WinBioBirDataFlags flags,
            ArrayPtr<WINBIO_UNIT_ID> unitId,
            ArrayPtr<PWINBIO_BIR> sample,
            out int sampleSize,
            out WinBioRejectDetail rejectDetail)
        {
            IPin pin1 = null;
            IPin pin2 = null;
            //IPin pin3 = null;
            //IPin pin4 = null;
            try
            {
                var pointer1 = IntPtr.Zero;
                var pointer2 = IntPtr.Zero;
                //var pointer3 = IntPtr.Zero;
                //var pointer4 = IntPtr.Zero;
                if (unitId != null)
                {
                    pin1 = unitId.Pin();
                    pointer1 = pin1.Pointer;
                }
                if (sample != null)
                {
                    pin2 = sample.Pin();
                    pointer2 = pin2.Pointer;
                }
                //if (sampleSize != null)
                //{
                //    pin3 = sampleSize.Pin();
                //    pointer3 = pin3.Pointer;
                //}
                //if (rejectDetail != null)
                //{
                //    pin4 = rejectDetail.Pin();
                //    pointer4 = pin4.Pointer;
                //}
                return CaptureSample(sessionHandle, purpose, flags, pointer1, pointer2, out sampleSize, out rejectDetail);
            }
            finally
            {
                if (pin1 != null) pin1.Dispose();
                if (pin2 != null) pin2.Dispose();
                //if (pin3 != null) pin3.Dispose();
                //if (pin4 != null) pin4.Dispose();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioLocateSensor")]
        public static extern WinBioErrorCode LocateSensor(WinBioSessionHandle sessionHandle, out int unitId);

        [DllImport(LibName, EntryPoint = "WinBioEnumBiometricUnits")]
        private static extern WinBioErrorCode EnumBiometricUnits(
            WinBioBiometricType factor,
            IntPtr unitSchemaArray,
            IntPtr unitCount);

        public static WinBioErrorCode EnumBiometricUnits(
            WinBioBiometricType factor,
            ArrayPtr<WINBIO_UNIT_SCHEMA> unitSchemaArray,
            ArrayPtr<SIZE_T> unitCount)
        {
            IPin pin1 = null;
            IPin pin2 = null;
            try
            {
                var pointer1 = IntPtr.Zero;
                var pointer2 = IntPtr.Zero;
                if (unitSchemaArray != null)
                {
                    pin1 = unitSchemaArray.Pin();
                    pointer1 = pin1.Pointer;
                }
                if (unitCount != null)
                {
                    pin2 = unitCount.Pin();
                    pointer2 = pin2.Pointer;
                }
                return EnumBiometricUnits(factor, pointer1, pointer2);
            }
            finally
            {
                if (pin1 != null) pin1.Dispose();
                if (pin2 != null) pin2.Dispose();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioIdentify")]
        private static extern WinBioErrorCode Identify(
            WinBioSessionHandle sessionHandle,
            IntPtr unitId,
            IntPtr identity,
            IntPtr subFactor,
            out WinBioRejectDetail rejectDetail);

        public static WinBioErrorCode WinBioIdentify(
            WinBioSessionHandle sessionHandle,
            ArrayPtr<WINBIO_UNIT_ID> unitId,
            WINBIO_IDENTITY identity,
            ByteArrayPtr subFactor,
            out WinBioRejectDetail rejectDetail)
        {
            IPin pin1 = null;
            IPin pin2 = null;
            IPin pin3 = null;
            //IPin pin4 = null;
            try
            {
                var pointer1 = IntPtr.Zero;
                var pointer2 = IntPtr.Zero;
                var pointer3 = IntPtr.Zero;
                //var pointer4 = IntPtr.Zero;
                if (unitId != null)
                {
                    pin1 = unitId.Pin();
                    pointer1 = pin1.Pointer;
                }
                if (identity != null)
                {
                    pin2 = identity.Pin();
                    pointer2 = pin2.Pointer;
                }
                if (subFactor != null)
                {
                    pin3 = subFactor.Pin();
                    pointer3 = pin3.Pointer;
                }
                //if (rejectDetail != null)
                //{
                //    pin4 = rejectDetail.Pin();
                //    pointer4 = pin4.Pointer;
                //}
                return Identify(sessionHandle, pointer1, pointer2, pointer3, out rejectDetail);
            }
            finally
            {
                if (pin1 != null) pin1.Dispose();
                if (pin2 != null) pin2.Dispose();
                if (pin3 != null) pin3.Dispose();
                //if (pin4 != null) pin4.Dispose();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioFree")]
        public extern static WinBioErrorCode Free(IntPtr address);
    }
}