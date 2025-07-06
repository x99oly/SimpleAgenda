using SimpleAgenda.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAgenda.Exceptions
{
    internal class RecurrenceException : ArgumentException
    {
        public RecurrenceException(string message)
            : base(message)
        {
        }
        public RecurrenceException()
            : base("The provided recurrence value is invalid. Expect: greater than 0.")
        {
        }
        public RecurrenceException(int recurrence)
            : base($"The provided recurrency '{recurrence}' must be greater than 0.")
        {
        }

        public RecurrenceException(int recurrence, Exception innerException)
            : base($"The provided recurrency '{recurrence}' must be greater than 0.", innerException)
        {
        }

    }
}
