using System;
using System.Collections.Generic;
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
            foreach (var configuration in IterateConfigurations(2))
            {
                
            }
        }
        
        private static RegistryKey OpenDatabases()
        {
            var key = Registry.LocalMachine.OpenSubKey(DatabaseKeyName, true);
            if (key == null) throw new ApplicationException("Registry error: Biometric databases not found.");
            return key;
        }

        private static IEnumerable<SensorRegistry> IterateConfigurations(int unitId)
        {
            var unitSchema = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint).Single(_ => _.UnitId == unitId);
            var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
            using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName, true))
            {
                if (unitKey == null) throw new WinBioException(string.Format("Unit configurations not found: {0}", unitName));
                foreach (var confName in unitKey.GetSubKeyNames())
                {
                    int number;
                    if (!int.TryParse(confName, out number)) continue;
                    var config = new WinBioSensorKey();
                    using (var confKey = unitKey.OpenSubKey(confName))
                    {
                        config.Read(confKey);
                    }
                    yield return new SensorRegistry(unitKey, config, number);
                }
            }
        }

        private class SensorRegistry
        {
            public RegistryKey UnitKey;
            public WinBioSensorKey Configuration;
            public int ConfigurationName;

            public SensorRegistry(RegistryKey unitKey, WinBioSensorKey configuration, int configurationName)
            {
                UnitKey = unitKey;
                Configuration = configuration;
                ConfigurationName = configurationName;
            }
        }

        public static IEnumerable<WinBioSensorKey> GetSensorConfig(WinBioUnitSchema unitSchema)
        {
            var unitName = string.Format(UnitKeyName, unitSchema.DeviceInstanceId);
            using (var unitKey = Registry.LocalMachine.OpenSubKey(unitName))
            {
                if (unitKey == null) throw new WinBioException(string.Format("Unit configurations not found: {0}", unitName));
                foreach (var confName in unitKey.GetSubKeyNames())
                {
                    int number;
                    if (!int.TryParse(confName, out number)) continue;
                    using (var confKey = unitKey.OpenSubKey(confName))
                    {
                        if (confKey == null) throw new WinBioException(string.Format("Unit configuration not found: {0}\\{1}", unitName, confName));
                        var unit = new WinBioSensorKey();
                        unit.Read(confKey);
                        yield return unit;
                    }
                }
            }
            throw new WinBioException(string.Format("No system sensor configuration found: {0} ({1})", unitSchema.UnitId, unitSchema.DeviceInstanceId));
        }

        private static IEnumerable<WinBioSensorKey> GetSensorConfig(int unitId)
        {
            var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
            return GetSensorConfig(units.Single(_ => _.UnitId == unitId));
        }

        private static string KeyGuid(Guid guid)
        {
            return string.Format("{{{0}}}", guid);
        }

        /// <summary>
        /// Adds a new biometric database compatible to the given unit.
        /// Throws an exception if the database already exists.
        /// </summary>
        public static void AddDatabase(Guid databaseId, int unitId)
        {
            // throw exception if the database already exists
            if (DatabaseExists(databaseId))
            {
                throw new WinBioException(string.Format("Database already exists: {0}", databaseId));
            }
            // get first system system sensor config for the given unitId
            var sensorConfig = GetSensorConfig(unitId).First(_ => _.SystemSensor == 1);
            // get the corresponding database
            var database = GetDatabase(sensorConfig.DatabaseId);
            // create new compatible database
            var newDatabase = new WinBioDatabaseKey(database);
            // write database configuration to registry
            using (var databasesKey = OpenDatabases())
            using (var newKey = databasesKey.CreateSubKey(KeyGuid(databaseId)))
            {
                newDatabase.Write(newKey);
            }
        }

        private static WinBioStorageSchema GetDatabase(Guid databaseId)
        {
            return WinBio.EnumDatabases(WinBioBiometricType.Fingerprint).Single(_ => _.DatabaseId == databaseId);
        }

        /// <summary>
        /// Determines whether the given database exists.
        /// </summary>
        public static bool DatabaseExists(Guid databaseId)
        {
            return WinBio.EnumDatabases(WinBioBiometricType.Fingerprint).Any(_ => _.DatabaseId == databaseId);
        }

        /// <summary>
        /// Removes the given biometric database and all its data.
        /// Also removes all units from the corresponding sensor pool.
        /// </summary>
        public static void RemoveDatabase(Guid databaseId)
        {
            // find database, throws if not found
            var database = GetDatabase(databaseId);
            // delete template file, throws if not authorized
            File.Delete(database.FilePath);
            //TODO: erase sensor configurations for this database

            // erase database configuration from registry
            using (var databasesKey = OpenDatabases())
            {
                databasesKey.DeleteSubKey(KeyGuid(databaseId));
            }
        }

        /// <summary>
        /// Adds a unit to the sensor pool related to the given biometric database.
        /// </summary>
        public static void AddUnit(Guid databaseId, int unitId)
        {
            var configs = GetSensorConfig(unitId).ToList();
            // throw?
            if (configs.Any(_ => _.DatabaseId == databaseId)) return;
            var newConfig = configs.First(_ => _.SystemSensor == 1);
            newConfig.DatabaseId = databaseId;
            newConfig.SystemSensor = 0;
            //TODO: write to registry at the next highest free number
        }

        /// <summary>
        /// Adds a unit to the sensor pool related to the given biometric database.
        /// </summary>
        public static void RemoveUnit(Guid databaseId, int unitId)
        {
            var configs = GetSensorConfig(unitId).ToList();
            // there should be only one, but we will erase all anyway
            foreach (var config in configs.Where(_ => _.DatabaseId == databaseId))
            {
                
            }
        }
    }
}