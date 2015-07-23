using System;

namespace WinBioNET.Enums
{
    /// <summary>
    /// Configuration flags specify the general configuration of units in the session.
    /// Access flags specify how the application will use the biometric units.
    /// You must specify one configuration flag but you can combine that flag with any access flag.
    /// </summary>
    [Flags]
    public enum WinBioSessionFlag
    {
        /// <summary>
        /// Group: configuration
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// Group: configuration
        /// </summary>
        Basic = 1 << 16,
        
        /// <summary>
        /// Group: configuration
        /// </summary>
        Advanced = 2 << 16,
        
        /// <summary>
        /// Group: access
        /// </summary>
        Raw = 1,
        
        /// <summary>
        /// Group: access
        /// </summary>
        Maintenance = 2
    }
}