using System;
using DickeFinger.Enums;

namespace DickeFinger.Unused
{
    public class WinBioDatabase
    {
        public static void EnumDatabases()
        {
            var schemaArray = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint);
            Console.WriteLine("Databases found: {0}", schemaArray.Length);
            for (var i = 0; i < schemaArray.Length; i++)
            {
                Console.WriteLine("Database {0}", i);
                var id = schemaArray[i].DatabaseId;
                Console.WriteLine("Id: {0}", id);
            }
        }
    }
}