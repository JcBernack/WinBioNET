using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Opening biometric session");
                var session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.System, WinBioSessionFlag.Default);
                try
                {
                    Console.WriteLine("Use any sensor to list enrollments for your identity on that sensor...");
                    WinBioIdentity identity;
                    WinBioBiometricSubType subFactor;
                    WinBioRejectDetail rejectDetail;
                    var unitId = WinBio.Identify(session, out identity, out subFactor, out rejectDetail);
                    Console.WriteLine();
                    Console.WriteLine("Used unit id: {0}", unitId);
                    Console.WriteLine("Identity: {0}", identity);
                    Console.WriteLine();
                    var enrollments = WinBio.EnumEnrollments(session, unitId, identity);
                    Console.WriteLine("Found {0} enrollments", enrollments.Length);
                    foreach (var enrollment in enrollments)
                    {
                        Console.WriteLine("{0}{1}", enrollment, enrollment == subFactor ? " (detected)" : "");
                    }
                    Console.WriteLine();
                    //AddEnrollment(session, unitId, WinBioBiometricSubType.LhIndexFinger);
                    Console.WriteLine("Verify identity with any finger:");
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

        public static void AddEnrollment(WinBioSessionHandle session, int unitId, WinBioBiometricSubType subType)
        {
            var addEnrollment = WinBioBiometricSubType.RhRingFinger;
            Console.WriteLine("Beginning enrollment of {0}:", addEnrollment);
            WinBio.EnrollBegin(session, addEnrollment, unitId);
            var code = WinBioErrorCode.MoreData;
            for (var swipes = 1; code != WinBioErrorCode.Success; swipes++)
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
                    case WinBioErrorCode.Success:
                        Console.WriteLine("Enrollment complete");
                        break;
                    default:
                        throw new WinBioException(code, "WinBioEnrollCapture failed");
                }
            }
            //Console.WriteLine("Discarding enrollment for now..");
            //WinBio.EnrollDiscard(session);
            Console.WriteLine("Committing enrollment..");
            WinBioIdentity identity;
            var isNewTemplate = WinBio.EnrollCommit(session, out identity);
            Console.WriteLine(isNewTemplate ? "New template committed." : "Template already existing.");
            Console.WriteLine("Identity: {0}", identity);
        }
    }
}
