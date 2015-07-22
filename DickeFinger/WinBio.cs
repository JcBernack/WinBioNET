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
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var code = Identify(sessionHandle, out unitId, handle.AddrOfPinnedObject(), out subFactor, out rejectDetail);
                WinBioException.ThrowOnError(code, "WinBioIdentify failed");
            }
            finally
            {
                handle.Free();
            }
            identity = new WinBioIdentity(bytes);
            return unitId;
        }

        [DllImport(LibName, EntryPoint = "WinBioEnumEnrollments")]
        private extern static WinBioErrorCode EnumEnrollments(
            WinBioSessionHandle sessionHandle,
            int unitId,
            IntPtr identity,
            out IntPtr subFactorArray,
            out int subFactorCount);

        public static WinBioBiometricSubType[] EnumEnrollments(WinBioSessionHandle sessionHandle, int unitId, WinBioIdentity identity)
        {
            var handle = GCHandle.Alloc(identity.GetBytes(), GCHandleType.Pinned);
            try
            {
                IntPtr subFactorArray;
                int subFactorCount;
                var code = EnumEnrollments(sessionHandle, unitId, handle.AddrOfPinnedObject(), out subFactorArray, out subFactorCount);
                WinBioException.ThrowOnError(code, "WinBioEnumEnrollments failed");
                return MarshalArray<WinBioBiometricSubType>(subFactorArray, subFactorCount);
            }
            finally
            {
                handle.Free();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioEnrollBegin")]
        private extern static WinBioErrorCode WinBioEnrollBegin(WinBioSessionHandle sessionHandle, WinBioBiometricSubType subType, int unitId);

        public static void EnrollBegin(WinBioSessionHandle sessionHandle, WinBioBiometricSubType subType, int unitId)
        {
            var code = WinBioEnrollBegin(sessionHandle, subType, unitId);
            WinBioException.ThrowOnError(code, "WinBioEnrollBegin failed");
        }

        [DllImport(LibName, EntryPoint = "WinBioEnrollCapture")]
        public extern static WinBioErrorCode EnrollCapture(WinBioSessionHandle sessionHandle, out WinBioRejectDetail rejectDetail);

        [DllImport(LibName, EntryPoint = "WinBioEnrollCommit")]
        private extern static WinBioErrorCode EnrollCommit(WinBioSessionHandle sessionHandle, IntPtr identity, out bool isNewTemplate);

        public static bool EnrollCommit(WinBioSessionHandle sessionHandle, out WinBioIdentity identity)
        {
            bool isNewTemplate;
            var bytes = new byte[WinBioIdentity.Size];
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var code = EnrollCommit(sessionHandle, handle.AddrOfPinnedObject(), out isNewTemplate);
                WinBioException.ThrowOnError(code, "WinBioEnrollCommit failed");
            }
            finally
            {
                handle.Free();
            }
            identity = new WinBioIdentity(bytes);
            return isNewTemplate;
        }

        [DllImport(LibName, EntryPoint = "WinBioEnrollDiscard")]
        private extern static WinBioErrorCode WinBioEnrollDiscard(WinBioSessionHandle sessionHandle);

        public static void EnrollDiscard(WinBioSessionHandle sessionHandle)
        {
            var code = WinBioEnrollDiscard(sessionHandle);
            WinBioException.ThrowOnError(code, "WinBioEnrollDiscard failed");
        }

        [DllImport(LibName, EntryPoint = "WinBioVerify")]
        private static extern WinBioErrorCode Verify(
            WinBioSessionHandle sessionHandle,
            IntPtr identity,
            WinBioBiometricSubType subFactor,
            out int unitId,
            out bool match,
            out WinBioRejectDetail rejectDetail);

        public static bool Verify(
            WinBioSessionHandle sessionHandle,
            WinBioIdentity identity,
            WinBioBiometricSubType subFactor,
            out int unitId,
            out WinBioRejectDetail rejectDetail)
        {
            bool match;
            var handle = GCHandle.Alloc(identity.GetBytes(), GCHandleType.Pinned);
            try
            {
                var code = Verify(sessionHandle, handle.AddrOfPinnedObject(), subFactor, out unitId, out match, out rejectDetail);
                WinBioException.ThrowOnError(code, "WinBioVerify failed");
            }
            finally
            {
                handle.Free();
            }
            return match;
        }

        [DllImport(LibName, EntryPoint = "WinBioDeleteTemplate")]
        private static extern WinBioErrorCode DeleteTemplate(
            WinBioSessionHandle sessionHandle,
            int unitId,
            IntPtr identity,
            WinBioBiometricSubType subFactor);

        public static void DeleteTemplate(
            WinBioSessionHandle sessionHandle,
            int unitId,
            WinBioIdentity identity,
            WinBioBiometricSubType subFactor)
        {
            var handle = GCHandle.Alloc(identity.GetBytes(), GCHandleType.Pinned);
            try
            {
                var code = DeleteTemplate(sessionHandle, unitId, handle.AddrOfPinnedObject(), subFactor);
                WinBioException.ThrowOnError(code, "WinBioDeleteTemplate failed");
            }
            finally
            {
                handle.Free();
            }
        }

        [DllImport(LibName, EntryPoint = "WinBioEnumServiceProviders")]
        private static extern WinBioErrorCode EnumServiceProviders(
            WinBioBiometricType factor,
            out IntPtr bspSchemaArray,
            out int bspCount);

        public static WinBioBspSchema[] EnumServiceProviders(WinBioBiometricType factor)
        {
            IntPtr bspSchemaArray;
            int bspCount;
            var code = EnumServiceProviders(factor, out bspSchemaArray, out bspCount);
            WinBioException.ThrowOnError(code, "WinBioEnumServiceProviders failed");
            return MarshalArray<WinBioBspSchema>(bspSchemaArray, bspCount);
        }

        [DllImport(LibName, EntryPoint = "WinBioFree")]
        private extern static WinBioErrorCode Free(IntPtr address);

        /// <summary>
        /// Marshals an array of type T at the given address and frees the unmanaged memory afterwards.
        /// Supports primitive types, structures and enums.
        /// </summary>
        /// <typeparam name="T">Type of the array elements.</typeparam>
        /// <param name="pointer">Address of the array in unmanaged memory.</param>
        /// <param name="count">Number of elements in the array.</param>
        /// <returns>Managed array of the given type.</returns>
        private static T[] MarshalArray<T>(IntPtr pointer, int count)
        {
            if (pointer == IntPtr.Zero) return null;
            try
            {
                var offset = pointer;
                var data = new T[count];
                var type = typeof (T);
                if (type.IsEnum) type = type.GetEnumUnderlyingType();
                for (var i = 0; i < count; i++)
                {
                    data[i] = (T) Marshal.PtrToStructure(offset, type);
                    offset += Marshal.SizeOf(type);
                }
                return data;
            }
            finally
            {
                Free(pointer);
            }
        }
    }
}