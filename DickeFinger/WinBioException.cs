using System;
using DickeFinger.Enums;

namespace DickeFinger
{
    public class WinBioException
        : Exception
    {
        public WinBioErrorCode ErrorCode { get; private set; }

        public WinBioException(WinBioErrorCode errorCode)
            : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        public WinBioException(WinBioErrorCode errorCode, string message)
            : base(string.Format("{0}: {1}", message, errorCode))
        {
            HResult = (int) errorCode;
            ErrorCode = errorCode;
        }
    }
}