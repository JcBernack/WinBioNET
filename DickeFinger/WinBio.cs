using System;
using System.Runtime.InteropServices;
using DickeFinger.Enums;

namespace DickeFinger
{
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

        public static WinBioSessionHandle OpenSession(WinBioBiometricType factor, WinBioPoolType poolType, WinBioSessionFlag flags)
        {
            WinBioSessionHandle sessionHandle;
            var code = OpenSession(factor, poolType, flags, IntPtr.Zero, 0, IntPtr.Zero, out sessionHandle);
            WinBioException.ThrowOnError(code, "WinBioOpenSession failed");
            return sessionHandle;
        }

        [DllImport(LibName, EntryPoint = "WinBioCloseSession")]
        private extern static WinBioErrorCode WinBioCloseSession(WinBioSessionHandle sessionHandle);

        public static void CloseSession(WinBioSessionHandle sessionHandle)
        {
            if (!sessionHandle.IsValid) return;
            var code = WinBioCloseSession(sessionHandle);
            WinBioException.ThrowOnError(code, "WinBioOpenSession failed");
            sessionHandle.Invalidate();
        }
        
        [DllImport(LibName, EntryPoint = "WinBioEnumDatabases")]
        private extern static WinBioErrorCode EnumDatabases(WinBioBiometricType factor, out IntPtr storageSchemaArray, out int storageCount);

        public static WinBioStorageSchema[] EnumDatabases(WinBioBiometricType factor)
        {
            IntPtr pointer;
            int count;
            var code = EnumDatabases(factor, out pointer, out count);
            WinBioException.ThrowOnError(code, "WinBioEnumDatabases failed");
            return MarshalArray<WinBioStorageSchema>(pointer, count);
        }

        [DllImport(LibName, EntryPoint = "WinBioCaptureSample")]
        private extern static WinBioErrorCode CaptureSample(
            WinBioSessionHandle sessionHandle,
            WinBioBirPurpose purpose,
            WinBioBirDataFlags flags,
            out int unitId,
            out IntPtr sample,
            out int sampleSize,
            out WinBioRejectDetail rejectDetail);

        public static int CaptureSample(
            WinBioSessionHandle sessionHandle,
            WinBioBirPurpose purpose,
            WinBioBirDataFlags flags,
            out int sampleSize,
            out WinBioRejectDetail rejectDetail)
        {
            int unitId;
            IntPtr pointer;
            var code = CaptureSample(sessionHandle, purpose, flags, out unitId, out pointer, out sampleSize, out rejectDetail);
            WinBioException.ThrowOnError(code, "WinBioCaptureSample failed");
            //TODO: parse WINBIO_BIR structure at pointer
            Free(pointer);
            return unitId;
        }

        [DllImport(LibName, EntryPoint = "WinBioLocateSensor")]
        private extern static WinBioErrorCode LocateSensor(WinBioSessionHandle sessionHandle, out int unitId);

        public static int LocateSensor(WinBioSessionHandle sessionHandle)
        {
            int unitId;
            var code = LocateSensor(sessionHandle, out unitId);
            WinBioException.ThrowOnError(code, "WinBioLocateSensor failed");
            return unitId;
        }

        [DllImport(LibName, EntryPoint = "WinBioEnumBiometricUnits")]
        private extern static WinBioErrorCode EnumBiometricUnits(WinBioBiometricType factor, out IntPtr unitSchemaArray, out int unitCount);

        public static WinBioUnitSchema[] EnumBiometricUnits(WinBioBiometricType factor)
        {
            IntPtr pointer;
            int count;
            var code = EnumBiometricUnits(factor, out pointer, out count);
            WinBioException.ThrowOnError(code, "WinBioEnumBiometricUnits failed");
            return MarshalArray<WinBioUnitSchema>(pointer, count);
        }

        [DllImport(LibName, EntryPoint = "WinBioIdentify")]
        private extern static WinBioErrorCode Identify(
            WinBioSessionHandle sessionHandle,
            out int unitId,
            IntPtr identity,
            out WinBioBiometricSubType subFactor,
            out WinBioRejectDetail rejectDetail);

        public static int Identify(
            WinBioSessionHandle sessionHandle,
            out WinBioIdentity identity,
            out WinBioBiometricSubType subFactor,
            out WinBioRejectDetail rejectDetail)
        {
            int unitId;
            var bytes = new byte[WinBioIdentity.Size];
            var handle = Marshal.AllocHGlobal(76);
            try
            {
                var code = Identify(sessionHandle, out unitId, handle, out subFactor, out rejectDetail);
                WinBioException.ThrowOnError(code, "WinBioIdentify failed");
                Marshal.Copy(handle, bytes, 0, bytes.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(handle);
            }
            identity = new WinBioIdentity(bytes);
            return unitId;
        }

        [DllImport(LibName, EntryPoint = "WinBioFree")]
        private extern static WinBioErrorCode Free(IntPtr address);

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

        private static T[] MarshalArray<T>(IntPtr pointer, int count)
        {
            if (pointer == IntPtr.Zero) return null;
            try
            {
                return MarshalArrayOfStruct<T>(pointer, count);
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