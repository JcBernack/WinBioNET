using System;

namespace DickeFinger
{
    [Flags]
    public enum WinBioDatabaseFlag
        : uint
    {
        /// <summary>
        /// Represents a mask for the flag bits.
        /// </summary>
        FlagMask = 0xFFFF0000,
        
        /// <summary>
        /// The database resides on a removable drive.
        /// </summary>
        FlagRemovable = 0x00010000,
        
        /// <summary>
        /// The database resides on a remote computer.
        /// </summary>
        FlagRemote = 0x00020000,

        /// <summary>
        /// Represents a mask for the type bits.
        /// </summary>
        TypeMask = 0x0000FFFF,

        /// <summary>
        /// The database is contained in a file.
        /// </summary>
        TypeFile = 0x00000001,

        /// <summary>
        /// The database is managed by a database management system.
        /// </summary>
        TypeDbms = 0x00000002,

        /// <summary>
        /// The database resides on the biometric sensor.
        /// </summary>
        TypeOnchip = 0x00000003,

        /// <summary>
        /// The database resides on a smart card.
        /// </summary>
        TypeSmartcard = 0x00000004,
    }
}