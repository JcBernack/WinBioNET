namespace DickeFinger.Enums
{
    /// <summary>
    /// Error conditions -- values are in the range: 0x8001 - 0xFFFF
    /// Informational messages -- values are in the range: 0x0001 - 0x7FFF
    /// TODO: WinError.h contains more errors which are missing here
    /// </summary>
    public enum WinBioErrorCode
        : uint
    {
        /// <summary>
        /// Ok
        /// </summary>
        Ok = 0,

        /// <summary>
        /// False
        /// </summary>
        False = 1,

        /// <summary>
        /// General access denied error
        /// </summary>
        AccessDenied = 0x80070005,

        /// <summary>
        /// Not implemented
        /// </summary>
        NotImplemented = 0x80004001,

        /// <summary>
        /// Invalid arguments
        /// </summary>
        InvalidArguments = 0x80070057,

        /// <summary>
        /// Invalid pointer
        /// </summary>
        InvalidPointer = 0x80004003,

        /// <summary>
        /// Invalid handle
        /// </summary>
        InvalidHandle = 0x80070006,

        /// <summary>
        /// Windows Biometric Service doesn't support the specified biometric factor.
        /// </summary>
        UnsupportedFactor = 0x80098001,

        /// <summary>
        /// The unit ID number doesn't correspond to a valid biometric device.
        /// </summary>
        InvalidUnit = 0x80098002,

        /// <summary>
        /// The biometric sample doesn't match any known identity.
        /// </summary>
        UnknownID = 0x80098003,

        /// <summary>
        /// The biometric operation was canceled before it could complete.
        /// </summary>
        Canceled = 0x80098004,

        /// <summary>
        /// The biometric sample doesn't match the specified identity or sub-factor.
        /// </summary>
        NoMatch = 0x80098005,

        /// <summary>
        /// A biometric sample could not be captured because the operation was aborted.
        /// </summary>
        CaptureAborted = 0x80098006,

        /// <summary>
        /// An enrollment transaction could not be started because another enrollment is already in progress.
        /// </summary>
        EnrollmentInProgress = 0x80098007,

        /// <summary>
        /// The captured sample cannot be used for any further biometric operations.
        /// </summary>
        BadCapture = 0x80098008,

        /// <summary>
        /// The biometric unit doesn't support the specified unit control code.
        /// </summary>
        InvalidControlCode = 0x80098009,

        /// <summary>
        /// The driver already has a pending data collection operation in progress.
        /// </summary>
        DataCollectionInProgress = 0x8009800B,

        /// <summary>
        /// The biometric sensor driver does not support the requested data format.
        /// </summary>
        UnsupportedDataFormat = 0x8009800C,

        /// <summary>
        /// The biometric sensor driver does not support the requested data type.
        /// </summary>
        UnsupportedDataType = 0x8009800D,

        /// <summary>
        /// The biometric sensor driver does not support the requested data purpose.
        /// </summary>
        UnsupportedPurpose = 0x8009800E,

        /// <summary>
        /// The biometric unit is not in the proper state to perform the specified operation.
        /// </summary>
        InvalidDeviceState = 0x8009800F,

        /// <summary>
        /// The operation could not be performed because the sensor device was busy.
        /// </summary>
        DeviceBusy = 0x80098010,

        /// <summary>
        /// The biometric unit's storage adapter was unable to create a new database.
        /// </summary>
        DatabaseCantCreate = 0x80098011,

        /// <summary>
        /// The biometric unit's storage adapter was unable to open an existing database.
        /// </summary>
        DatabaseCantOpen = 0x80098012,

        /// <summary>
        /// The biometric unit's storage adapter was unable to close a database.
        /// </summary>
        DatabaseCantClose = 0x80098013,

        /// <summary>
        /// The biometric unit's storage adapter was unable to erase a database.
        /// </summary>
        DatabaseCantErase = 0x80098014,

        /// <summary>
        /// The biometric unit's storage adapter was unable to find a database.
        /// </summary>
        DatabaseCantFind = 0x80098015,

        /// <summary>
        /// The biometric unit's storage adapter was unable to create a database because that database already exists.
        /// </summary>
        DatabaseAlreadyExists = 0x80098016,

        /// <summary>
        /// The biometric unit's storage adapter was unable to add a record to the database because the database is full.
        /// </summary>
        DatabaseFull = 0x80098018,

        /// <summary>
        /// The database is locked and its contents are inaccessible.
        /// </summary>
        DatabaseLocked = 0x80098019,

        /// <summary>
        /// The contents of the database have become corrupted and are inaccessible.
        /// </summary>
        DatabaseCorrupted = 0x8009801A,

        /// <summary>
        /// No records were deleted because the specified identity and sub-factor are not present in the database.
        /// </summary>
        DatabaseNoSuchRecord = 0x8009801B,

        /// <summary>
        /// The specified identity and sub-factor are already enrolled in the database.
        /// </summary>
        DuplicateEnrollment = 0x8009801C,

        /// <summary>
        /// An error occurred while trying to read from the database.
        /// </summary>
        DatabaseReadError = 0x8009801D,

        /// <summary>
        /// An error occurred while trying to write to the database.
        /// </summary>
        DatabaseWriteError = 0x8009801E,

        /// <summary>
        /// No records in the database matched the query.
        /// </summary>
        DatabaseNoResults = 0x8009801F,

        /// <summary>
        /// All records from the most recent database query have been viewed.
        /// </summary>
        DatabaseNoMoreRecords = 0x80098020,

        /// <summary>
        /// A database operation unexpectedly encountered the end of the file.
        /// </summary>
        DatabaseEof = 0x80098021,

        /// <summary>
        /// A database operation failed due to a malformed index vector.
        /// </summary>
        DatabaseBadIndexVector = 0x80098022,

        /// <summary>
        /// The biometric unit doesn't belong to the specified service provider.
        /// </summary>
        IncorrectBsp = 0x80098024,

        /// <summary>
        /// The biometric unit doesn't belong to the specified sensor pool.
        /// </summary>
        IncorrectSensorPool = 0x80098025,

        /// <summary>
        /// The sensor adapter's capture buffer is empty.
        /// </summary>
        NoCaptureData = 0x80098026,

        /// <summary>
        /// The sensor adapter doesn't support the sensor mode specified in the configuration.
        /// </summary>
        InvalidSensorMode = 0x80098027,

        /// <summary>
        /// The requested operation cannot be performed due to a locking conflict.
        /// </summary>
        LockViolation = 0x8009802A,

        /// <summary>
        /// The data in a biometric template matches another template already in the database.
        /// </summary>
        DuplicateTemplate = 0x8009802B,

        /// <summary>
        /// The requested operation is not valid for the current state of the session or biometric unit.
        /// </summary>
        InvalidOperation = 0x8009802C,

        /// <summary>
        /// The session cannot begin a new operation because another operation is already in progress.
        /// </summary>
        SessionBusy = 0x8009802D,

        /// <summary>
        /// System policy settings have disabled the Windows biometric credential provider.
        /// </summary>
        CredProvDisabled = 0x80098030,

        /// <summary>
        /// The requested credential was not found.
        /// </summary>
        CredProvNoCredential = 0x80098031,

        /// <summary>
        /// System policy settings have disabled the Windows biometric service.
        /// </summary>
        Disabled = 0x80098032,

        /// <summary>
        /// The biometric unit could not be configured.
        /// </summary>
        ConfigurationFailure = 0x80098033,

        /// <summary>
        /// A private pool cannot be created because one or more biometric units are not available.
        /// </summary>
        SensorUnavailable = 0x80098034,

        /// <summary>
        /// A secure attention sequence (CTRL-ALT-DEL) is required for logon.
        /// </summary>
        SasEnabled = 0x80098035,

        /// <summary>
        /// A biometric sensor has failed.
        /// </summary>
        DeviceFailure = 0x80098036,

        /// <summary>
        /// Fast user switching is disabled.
        /// </summary>
        FastUserSwitchDisabled = 0x80098037,

        /// <summary>
        /// The System sensor pool cannot be opened from Terminal Server client sessions.
        /// </summary>
        NotActiveConsole = 0x80098038,

        /// <summary>
        /// There is already an active event monitor associated with the specified session.
        /// </summary>
        EventMonitorActive = 0x80098039,

        /// <summary>
        /// The value specified is not a valid property type.
        /// </summary>
        InvalidPropertyType = 0x8009803A,

        /// <summary>
        /// The value specified is not a valid property ID.
        /// </summary>
        InvalidPropertyID = 0x8009803B,

        /// <summary>
        /// The biometric unit doesn't support the specified property.
        /// </summary>
        UnsupportedProperty = 0x8009803C,

        /// <summary>
        /// The adapter binary did not pass its integrity check.
        /// </summary>
        AdapterIntegrityFailure = 0x8009803D,

        /// <summary>
        /// Informational messages:
        /// Another sample is needed for the current enrollment template.
        /// </summary>
        MoreData = 0x00090001,
    }
}