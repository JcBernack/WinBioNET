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

        public static void ThrowOnError(WinBioErrorCode errorCode, string message)
        {
            if (errorCode == WinBioErrorCode.Success) return;
            throw new WinBioException(errorCode, message);
        }

        public static void ThrowOnError(WinBioErrorCode errorCode)
        {
            if (errorCode == WinBioErrorCode.Success) return;
            throw new WinBioException(errorCode);
        }
    }
}