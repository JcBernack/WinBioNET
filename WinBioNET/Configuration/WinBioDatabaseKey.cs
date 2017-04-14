using System;
using WinBioNET.Enums;

namespace WinBioNET.Configuration
{
    public class WinBioDatabaseKey
        : WinBioRegistryKeyBase
    {
        public WinBioBiometricType BiometricType;
        public WinBioDatabaseFlag Attributes;
        public int AutoCreate; //bool?
        public int AutoName; //bool?
        public int InitialSize; //bool?
        public Guid Format;
        public string FilePath;
        public string ConnectionString;

        public WinBioDatabaseKey()
        {
        }

        public WinBioDatabaseKey(WinBioStorageSchema database)
        {
            Attributes = database.Attributes;
            Format = database.DataFormat;
            BiometricType = WinBioBiometricType.Fingerprint;
            AutoCreate = 1;
            AutoName = 1;
            InitialSize = 32;
            FilePath = Environment.SystemDirectory + string.Format(@"\WINBIODATABASE\{0}.DAT", database.DatabaseId.ToString().ToUpper());
            ConnectionString = "";
        }
    }
}