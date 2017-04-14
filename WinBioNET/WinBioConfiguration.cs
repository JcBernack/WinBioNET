using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using WinBioNET.Configuration;
using WinBioNET.Enums;

namespace WinBioNET
{
    public class WinBioConfiguration
    {
        private const string DatabaseKeyName = @"SYSTEM\CurrentControlSet\Services\WbioSrvc\Databases";
        private const string UnitKeyName = @"System\CurrentControlSet\Enum\{0}\Device Parameters\WinBio\Configurations";

        private static string KeyGuid(Guid guid)
        {
            return guid.ToString("B").ToUpperInvariant();
        }

        /// <summary>
        /// Determines whether the given database exists.
        /// </summary>
        public static bool DatabaseExists(Guid databaseId)
        {
            return WinBio.EnumDatabases(WinBioBiometricType.Fingerprint).Any(_ => _.DatabaseId == databaseId);
        }

        /// <summary>
        /// Adds a new biometric database compatible to the given unit.
        /// Throws an exception if the database already exists.
        /// </summary>
        public static void AddDatabase(Guid databaseId, int unitId)
        {
            // throw exception if the database already exists
            var databases = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint);
            if (databases.Any(_ => _.DatabaseId == databaseId))
            {
                throw new WinBioException(string.Format("Database already exists: {0}", databaseId));
            }
            // get first system sensor config for the given unitId
            var unitSchema = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint).Single(_ => _.UnitId == unitId);
            var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
            WinBioSensorKey systemSensorConfig = null;
            using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName, true))
            {
                if (unitKey == null) throw new Exception("wtf");
                foreach (var confName in unitKey.GetSubKeyNames())
                {
                    int number;
                    if (!int.TryParse(confName, out number)) continue;
                    var config = new WinBioSensorKey();
                    using (var confKey = unitKey.OpenSubKey(confName))
                    {
                        config.Read(confKey);
                    }
                    if (config.SystemSensor == 1)
                    {
                        systemSensorConfig = config;
                        break;
                    }
                }
            }
            if (systemSensorConfig == null) throw new Exception("dafuq");
            // get the corresponding database
            var database = databases.Single(_ => _.DatabaseId == systemSensorConfig.DatabaseId);
            // create new compatible database
            var newDatabase = new WinBioDatabaseKey(database);
            // write database configuration to registry
            using (var databasesKey = Registry.LocalMachine.OpenSubKey(DatabaseKeyName, true))
            {
                if (databasesKey == null) throw new Exception("wat?");
                using (var newKey = databasesKey.CreateSubKey(KeyGuid(databaseId)))
                {
                    newDatabase.Write(newKey);
                }
            }
        }

        /// <summary>
        /// Removes the given biometric database and all its data.
        /// Also removes all units from the corresponding sensor pool.
        /// </summary>
        public static void RemoveDatabase(Guid databaseId)
        {
            // find database, throws if not found
            var database = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint).Single(_ => _.DatabaseId == databaseId);

            // delete template file, throws if not authorized
            // make sure to grant permissions to folder C:\WINDOWS\SYSTEM32\WINBIODATABASE or similar in your machine
            // get ownership of the folder WINBIODATABASE then grant full control for your user
            if (File.Exists(database.FilePath))
            {
                File.Delete(database.FilePath);
            }
            else
            {
                // For 32-bits process running in 64 bits OS
                string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                string filePath = string.Format(@"{0}\SYSTEM32", windowsPath);
                string filePathSysNative = string.Format(@"{0}\SysNative", windowsPath);

                if (File.Exists(database.FilePath.Replace(filePath, filePathSysNative)))
                {
                    File.Delete(database.FilePath.Replace(filePath, filePathSysNative));
                }
            }
            // erase sensor configurations for this database
            foreach (var unitSchema in WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint))
            {
                var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
                using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName, true))
                {
                    if (unitKey == null) continue;
                    foreach (var confName in unitKey.GetSubKeyNames())
                    {
                        int number;
                        if (!int.TryParse(confName, out number)) continue;
                        var config = new WinBioSensorKey();
                        using (var confKey = unitKey.OpenSubKey(confName))
                        {
                            config.Read(confKey);
                        }
                        if (config.DatabaseId == databaseId)
                        {
                            unitKey.DeleteSubKey(confName);
                        }
                    }
                }
            }
            // erase database configuration from registry
            using (var databasesKey = Registry.LocalMachine.OpenSubKey(DatabaseKeyName, true))
            {
                if (databasesKey == null) throw new Exception("wat?");
                databasesKey.DeleteSubKey(KeyGuid(databaseId));
            }
        }

        /// <summary>
        /// Adds a unit to the sensor pool related to the given biometric database.
        /// </summary>
        public static void AddUnit(Guid databaseId, int unitId)
        {
            var unitSchema = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint).Single(_ => _.UnitId == unitId);
            var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
            var highest = -1;
            WinBioSensorKey newConfig = null;
            using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName, true))
            {
                if (unitKey == null) throw new Exception("wtf");
                foreach (var confName in unitKey.GetSubKeyNames())
                {
                    int number;
                    if (!int.TryParse(confName, out number)) continue;
                    if (number > highest) highest = number;
                    if (newConfig != null) continue;
                    var config = new WinBioSensorKey();
                    using (var confKey = unitKey.OpenSubKey(confName))
                    {
                        config.Read(confKey);
                    }
                    if (config.SystemSensor == 1)
                    {
                        newConfig = config;
                    }
                }
                if (newConfig == null || highest < 0) throw new Exception("dafuq");
                // write to registry at the next highest free number
                highest++;
                newConfig.DatabaseId = databaseId;
                newConfig.SystemSensor = 0;
                using (var confKey = unitKey.CreateSubKey(highest.ToString()))
                {
                    newConfig.Write(confKey);
                }
            }
        }

        /// <summary>
        /// Adds a unit to the sensor pool related to the given biometric database.
        /// </summary>
        public static void RemoveUnit(Guid databaseId, int unitId)
        {
            var unitSchema = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint).Single(_ => _.UnitId == unitId);
            var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
            using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName, true))
            {
                if (unitKey == null) throw new Exception("wtf");
                foreach (var confName in unitKey.GetSubKeyNames())
                {
                    int number;
                    if (!int.TryParse(confName, out number)) continue;
                    var config = new WinBioSensorKey();
                    using (var confKey = unitKey.OpenSubKey(confName))
                    {
                        config.Read(confKey);
                    }
                    // there should be only one but we will remove all anyway
                    if (config.DatabaseId == databaseId)
                    {
                        unitKey.DeleteSubKey(confName);
                    }
                }
            }
        }
    }
}
