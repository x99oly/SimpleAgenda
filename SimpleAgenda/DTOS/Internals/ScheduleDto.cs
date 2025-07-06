using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class ScheduleDto
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RecurrenceTypeEnum RecurrenceType { get; set; }
        public int RecurrenceInterval { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }

        public List<DayOfWeek> RecurrenceWeekDays { get; init; } = [];
        public List<AppointmentDto> CancelledAppointments { get; set; } = [];
        public List<DateOnly> ExcludedDates { get; set; } = [];

        [Required]
        public List<AppointmentDto> PendingAppointments { get; set; } = [];
    }
}
