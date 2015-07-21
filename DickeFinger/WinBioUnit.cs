using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public class WinBioUnit
    {
        public static void EnumUnitIds()
        {
            WinBioUnitSchema[] units;
            var code = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint, out units);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioEnumBiometricUnits failed");
            Console.WriteLine("Biometric units found: {0}", units.Length);
            for (var i = 0; i < units.Length; i++)
            {
                Console.WriteLine(units[i].UnitId);
                Console.WriteLine(units[i].PoolType);
                Console.WriteLine(units[i].BiometricFactor);
                Console.WriteLine(units[i].SensorSubType);
                Console.WriteLine(units[i].Capabilities);
                Console.WriteLine(units[i].DeviceInstanceId);
                Console.WriteLine(units[i].Description);
                Console.WriteLine(units[i].Manufacturer);
                Console.WriteLine(units[i].Model);
                Console.WriteLine(units[i].SerialNumber);
                Console.WriteLine(units[i].FirmwareVersion);
            }
        }
    }
}