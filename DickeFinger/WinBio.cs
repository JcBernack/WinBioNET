using System;
using System.Runtime.InteropServices;
using DickeFinger.Enums;
using PInvoker.Marshal;
using WinBio;

namespace DickeFinger
{
    //[SuppressUnmanagedCodeSecurity]
    public class WinBio
    {
        protected const string LibName = "winbio.dll";

        [DllImport(LibName, EntryPoint = "WinBioOpenSession")]
        private static extern WinBioErrorCode OpenSession(
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
        public static extern WinBioErrorCode CloseSession(WinBioSessionHandle sessionHandle);


        [DllImport(LibName, EntryPoint = "WinBioEnumDatabases")]
        private static extern WinBioErrorCode EnumDatabases(WinBioBiometricType factor, out IntPtr storageSchemaArray, out int storageCount);

        public static WinBioErrorCode EnumDatabases(WinBioBiometricType factor, out WinBioStorageSchema[] storageSchemaArray)
        {
            IntPtr pointer;
            int count;
            var code = EnumDatabases(factor, out pointer, out count);
            MarshalArray(pointer, count, out storageSchemaArray);
            return code;
        }

        [DllImport(LibName, EntryPoint = "WinBioCaptureSample")]
        private static extern WinBioErrorCode CaptureSample(
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
            out IntPtr unitSchemaArray,
            out int unitCount);

        public static WinBioErrorCode EnumBiometricUnits(
            WinBioBiometricType factor,
            out WinBioUnitSchema[] unitSchemaArray)
        {
            IntPtr pointer;
            int count;
            var code = EnumBiometricUnits(factor, out pointer, out count);
            MarshalArray(pointer, count, out unitSchemaArray);
            return code;
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
        private static extern WinBioErrorCode Free(IntPtr address);

        private static void MarshalArray<T>(IntPtr pointer, int count, out T[] array)
        {
            if (pointer == IntPtr.Zero) array = null;
            try
            {
                array = MarshalArrayOfStruct<T>(pointer, count);
            }
            finally
            {
                Free(pointer);
            }
        }

        private static T[] MarshalArrayOfStruct<T>(IntPtr pointer, int count)
        {
            var data = new T[count];
            for (var i = 0; i < count; i++)
            {
                data[i] = (T)Marshal.PtrToStructure(pointer, typeof(T));
                pointer += Marshal.SizeOf(typeof(T));
            }
            return data;
        }
    }
}