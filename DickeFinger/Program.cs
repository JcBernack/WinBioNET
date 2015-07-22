using System;
using System.CodeDom;
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
                    Console.WriteLine("Used unit id: {0}", unitId);
                    Console.WriteLine("Identity: {0}", identity);
                    var enrollments = WinBio.EnumEnrollments(session, unitId, identity);
                    Console.WriteLine("Found {0} enrollments", enrollments.Length);
                    foreach (var enrollment in enrollments)
                    {
                        Console.WriteLine("{0}{1}", enrollment, enrollment == subFactor ? " (detected)" : "");
                    }
                    Console.WriteLine();
                    var addEnrollment = WinBioBiometricSubType.RhRingFinger;
                    Console.WriteLine("Beginning enrollment of {0}:", addEnrollment);
                    WinBio.EnrollBegin(session, addEnrollment, unitId);
                    var code = WinBioErrorCode.WinbioIMoreData;
                    for (var swipes = 1; code != WinBioErrorCode.Success; swipes++)
                    {
                        code = WinBio.EnrollCapture(session, out rejectDetail);
                        switch (code)
                        {
                            case WinBioErrorCode.WinbioIMoreData:
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
                    Console.WriteLine("Discarding enrollment for now..");
                    WinBio.EnrollDiscard(session);
                }
                finally
                {
                    Console.WriteLine("Closing beiometric session");
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
    }
}
