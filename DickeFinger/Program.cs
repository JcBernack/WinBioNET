using System;
using System.Threading;

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
                WinBioDatabase.EnumDatabases();
                //using (var session = new WinBioSession())
                {
                    //session.LocateSensor();
                    //session.EnumUnitIds();
                    //session.CaptureSample();
                    //session.Identify();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: {0}", ex.Message);
            }
            Thread.Sleep(1000);
        }
    }
}
