using System;

namespace SimpleAgenda.Exceptions
{
    internal class DateRangeException : DomainException
    {
        public DateRangeException(string message)
            : base(message)
        {
        }

        public DateRangeException()
            : base("The provided date range is invalid.")
        {
        }

        public DateRangeException(DateTime startDate, DateTime endDate)
            : base($"The start date '{startDate}' cannot be later than the end date '{endDate}'.")
        {
        }

        public DateRangeException(DateTime startDate, DateTime endDate, Exception innerException)
            : base($"The start date '{startDate}' cannot be later than the end date '{endDate}'.", innerException)
        {
        }

    }
}
