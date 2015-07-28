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

        public static void Test()
        {
        }

        private static string KeyGuid(Guid guid)
        {
            return string.Format("{{{0}}}", guid);
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
            File.Delete(database.FilePath);
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
            var highest = 0;
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
                if (newConfig == null) throw new Exception("dafuq");
                // write to registry at the next highest free number
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

/*
 * 
 * Add Unit:
 *   Check all configs for given UnitId for databaseId.
 *   Find first config with SystemSensor == 1.
 *   Add new config with highest free SubKeyName into the Key of the unit configurations.
 * 
 * Remove Unit:
 *   Remove all SubKeys of the unit configuration key where databaseId matches.
 * 
 * Remove Database:
 *   Iterate over all units.
 *     "Remove Unit" with databaseId.
 * 
 * Add Database:
 *   Find first unit configuration with SystemSensor == 1 for a given UnitId.
 *   Find corresponding database.
 *   Add new compatible database to database key.
 * 
*/