namespace WinBioNET.Enums
{
    public enum WinBioIdentityType
    {
        /// <summary>
        /// The Identity structure is empty.
        /// </summary>
        Null = 0,
        
        /// <summary>
        /// The Identity matches "all identities"
        /// </summary>
        Wildcard = 1,
        
        /// <summary>
        /// A GUID identifies the template.
        /// </summary>
        GUID = 2,
        
        /// <summary>
        /// An account SID identifies the template.
        /// </summary>
        SID = 3
    }
}