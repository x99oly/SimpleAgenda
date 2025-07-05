
using SimpleAgenda.Enums;

namespace SimpleAgenda.Entities
{
    internal class Schedule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime StartDate { get; private set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; private set; } = DateTime.UtcNow.AddYears(100);
        public RecurrenceTypeEnum RecurrenceType { get; private set; } = RecurrenceTypeEnum.WEEKLY;
        public int RecurrenceInterval { get; private set; } = 1;
        public List<Appointment> CancelledAppointments { get; private set; } = [];
        public List<DateOnly> LockedDates { get; private set; } = [];
        public List<Appointment> PendingAppointments { get; private set; } = [];

        public Schedule() { }


    }
}
