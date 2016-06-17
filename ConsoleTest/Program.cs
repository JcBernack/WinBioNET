using System;
using WinBioNET;
using WinBioNET.Enums;

namespace ConsoleTest
{
    public static class Program
    {
        private static readonly Guid DatabaseId = Guid.Parse("BC7263C3-A7CE-49F3-8EBF-D47D74863CC6");
        
        public static void Main()
        {
            try
            {
                var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
                Console.WriteLine("Found {0} units", units.Length);
                if (units.Length == 0) return;
                var unit = units[0];
                var unitId = unit.UnitId;
                Console.WriteLine("Using unit id: {0}", unitId);
                Console.WriteLine("Device instance id: {0}", unit.DeviceInstanceId);
                var databases = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint);
                Console.WriteLine("Found {0} databases", databases.Length);
                for (var i = 0; i < databases.Length; i++)
                {
                    Console.WriteLine("DatabaseId {0}: {1}", i, databases[i].DatabaseId);
                }
                if (WinBioConfiguration.DatabaseExists(DatabaseId))
                {
                    Console.WriteLine("Removing database: {0}", DatabaseId);
                    WinBioConfiguration.RemoveDatabase(DatabaseId);
                }
                Console.WriteLine("Creating database: {0}", DatabaseId);
                WinBioConfiguration.AddDatabase(DatabaseId, unitId);
                Console.WriteLine("Adding sensor to the pool: {0}", unitId);
                WinBioConfiguration.AddUnit(DatabaseId, unitId);
                Console.WriteLine("Opening biometric session");
                var session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.Private, WinBioSessionFlag.Basic, new[] { unitId }, DatabaseId);
                try
                {
                    Console.WriteLine("Type sub type and press enter:");
                    WinBioBiometricSubType subType;
                    if (!Enum.TryParse(Console.ReadLine(), true, out subType))
                    {
                        Console.WriteLine("Invalid sub type");
                        return;
                    }
                    var identity = AddEnrollment(session, unitId, subType);
                    Console.WriteLine("Identity: {0}", identity);
                    Console.WriteLine();
                    var enrollments = WinBio.EnumEnrollments(session, unitId, identity);
                    Console.WriteLine("Found {0} enrollments for that identity:", enrollments.Length);
                    foreach (var enrollment in enrollments)
                    {
                        Console.WriteLine(enrollment);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Verify identity with any finger:");
                    WinBioRejectDetail rejectDetail;
                    WinBio.Verify(session, identity, WinBioBiometricSubType.Any, out unitId, out rejectDetail);
                    Console.WriteLine("Success");
                }
                finally
                {
                    Console.WriteLine("Closing biometric session");
                    WinBio.CloseSession(session);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.GetType().Name);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static WinBioIdentity AddEnrollment(WinBioSessionHandle session, int unitId, WinBioBiometricSubType subType)
        {
            Console.WriteLine("Beginning enrollment of {0}:", subType);
            WinBio.EnrollBegin(session, subType, unitId);
            var code = WinBioErrorCode.MoreData;
            for (var swipes = 1; code != WinBioErrorCode.Ok; swipes++)
            {
                WinBioRejectDetail rejectDetail;
                code = WinBio.EnrollCapture(session, out rejectDetail);
                switch (code)
                {
                    case WinBioErrorCode.MoreData:
                        Console.WriteLine("Swipe {0} was good", swipes);
                        break;
                    case WinBioErrorCode.BadCapture:
                        Console.WriteLine("Swipe {0} was bad: {1}", swipes, rejectDetail);
                        break;
                    case WinBioErrorCode.Ok:
                        Console.WriteLine("Enrollment complete with {0} swipes", swipes);
                        break;
                    default:
                        throw new WinBioException(code, "WinBioEnrollCapture failed");
                }
            }
            Console.WriteLine("Committing enrollment..");
            WinBioIdentity identity;
            var isNewTemplate = WinBio.EnrollCommit(session, out identity);
            Console.WriteLine(isNewTemplate ? "New template committed." : "Template already existing.");
            return identity;
        }
    }
}
