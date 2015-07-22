using System;
using System.Threading;
using DickeFinger.Enums;

namespace DickeFinger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine();
                WinBioDatabase.EnumDatabases();
                WinBioUnit.EnumUnitIds();
                Console.WriteLine();
                using (var session = new WinBioSession(WinBioPoolType.System, WinBioSessionFlag.Default))
                {
                    //session.LocateSensor();
                    //session.EnumUnitIds();
                    //session.CaptureSample(WinBioBirPurpose.NoPurposeAvailable, WinBioBirDataFlags.Raw);
                    //session.Identify();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.GetType().Name);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Thread.Sleep(2000);
        }
    }
}
