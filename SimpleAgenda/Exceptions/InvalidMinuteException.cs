using System;

namespace SimpleAgenda.Exceptions
{

    internal class InvalidMinuteException : ArgumentException
    {
        public InvalidMinuteException(int minute)
            : base($"The minute must be between 0 and 59. Received: {minute}.")
        {
        }

        public InvalidMinuteException(int minute, Exception innerException)
            : base($"The minute must be between 0 and 59. Received: {minute}.", innerException)
        {
        }
    }
}
