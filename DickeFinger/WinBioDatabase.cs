using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public class WinBioDatabase
    {
        public static void EnumDatabases()
        {
            WinBioStorageSchema[] schemaArray;
            var code = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint, out schemaArray);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioEnumDatabases failed");
            Console.WriteLine("Databases found: {0}", schemaArray.Length);
            for (int i = 0; i < schemaArray.Length; i++)
            {
                Console.WriteLine("Database {0}", i);
                var id = schemaArray[i].DatabaseId;
                Console.WriteLine("Id: {0}", id);
            }
        }
    }
}