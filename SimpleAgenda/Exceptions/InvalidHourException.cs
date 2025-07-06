using System;

namespace SimpleAgenda.Exceptions
{
    // Exceção específica para hora inválida
    internal class InvalidHourException : ArgumentException
    {
        public InvalidHourException(int hour)
            : base($"The hour must be between 0 and 23. Received: {hour}.")
        {
        }

        public InvalidHourException(int hour, Exception innerException)
            : base($"The hour must be between 0 and 23. Received: {hour}.", innerException)
        {
        }
    }

}
