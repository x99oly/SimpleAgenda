using SimpleAgenda.Enums;
using SimpleAgenda.Exceptions;

namespace SimpleAgenda.Entities
{
    internal readonly struct Recurrence(RecurrenceTypeEnum recurrenceType, int recurrenceInterval, int recurrenceLimit, HourMinute recurrenceTime,
    DaysOfWeekCollection daysOfWeek)
    {
        public readonly RecurrenceTypeEnum RecurrenceType { get; init; } = recurrenceType;
        public readonly int RecurrenceInterval { get; init; } = ValidateRecurrence(recurrenceInterval);
        public readonly int RecurrenceLimit { get; init; } = ValidateRecurrence(recurrenceLimit);
        public readonly HourMinute RecurrenceTime { get; init; } = recurrenceTime;
        public readonly DaysOfWeekCollection DaysOfWeek { get; init; } = daysOfWeek;

        private static int ValidateRecurrence(int recurrenceInterval)
        {
            if (recurrenceInterval <= 0)
                throw new RecurrenceException(recurrence: recurrenceInterval);

            return recurrenceInterval;
        }

    }

    internal readonly record struct DateRange(DateTime StartDate, DateTime EndDate)
    {
        public readonly DateTime StartDate = StartDate >= DateTime.UtcNow
            ? StartDate
            : throw new DateRangeException(
                $"The provided date '{StartDate}' cannot be early than the current date '{DateTime.UtcNow}-UTC'.");

        public readonly DateTime EndDate = EndDate > StartDate
            ? EndDate
            : throw new DateRangeException(
                $"The provided 'End Date' is null or smaller than 'Start Date'.");
    }

    internal record struct HourMinute(int Hours, int Minutes)
    {
        public readonly int Hour = Hours is < 24 and >= 0
            ? Hours
            : throw new InvalidHourException(hour: Hours);

        public readonly int Minute = Minutes is < 60 and >= 0
            ? Minutes
            : throw new InvalidMinuteException(minute: Hours);

        public readonly TimeSpan AsTimeSpan()
            => new(Hour, Minute, 0);

        public static HourMinute FromTimeSpan(TimeSpan ts)
            => new((int)ts.TotalHours, ts.Minutes);

        public readonly override string ToString() => $"{Hour:D2}:{Minute:D2}";
    }

    internal readonly struct DaysOfWeekCollection
    {
        private readonly HashSet<DayOfWeek> _days;

        public DaysOfWeekCollection(IEnumerable<DayOfWeek>? days)
        {
            if (days is null || !days.Any())
                _days = [DateTime.UtcNow.DayOfWeek];
            else
                _days = [.. days];
        }

        public static DaysOfWeekCollection FromDate(DateTime date)
            => new([date.DayOfWeek]);

        public bool Contains(DayOfWeek day) => _days.Contains(day);

        public IEnumerable<DayOfWeek> AsEnumerable() => _days.OrderBy(d => d);

        public override string ToString()
            => string.Join(", ", AsEnumerable());
    }

}
