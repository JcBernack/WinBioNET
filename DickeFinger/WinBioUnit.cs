using System;
using DickeFinger.Enums;
using PInvoker.Marshal;
using WinBio;

namespace DickeFinger
{
    public class WinBioUnit
    {
        public static void EnumUnitIds()
        {
            var array = new ArrayPtr<WINBIO_UNIT_SCHEMA>();
            var unitCount = new ArrayPtr<SIZE_T>();
            var code = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint, array, unitCount);
            if (code != WinBioErrorCode.Success) throw new WinBioException(code, "WinBioEnumBiometricUnits failed");
            for (var i = 0; i < unitCount[0]; i++)
            {
                Console.WriteLine(array[i].UnitId);
                Console.WriteLine(array[i].Manufacturer);
                Console.WriteLine(array[i].Model);
                Console.WriteLine(array[i].SerialNumber);
                Console.WriteLine(array[i].Description);
            }
        }
    }
}