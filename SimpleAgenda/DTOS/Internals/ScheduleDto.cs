using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class ScheduleDto
    {
        [Key]
        public Guid Id { get; set; } = default;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = DateTime.UtcNow.AddYears(100);
        public RecurrenceTypeEnum RecurrenceType { get; set; } = RecurrenceTypeEnum.WEEKLY;
        public int RecurrenceInterval { get; set; } = 1;
        public List<AppointmentDto> CancelledAppointments { get; set; } = [];
        public List<DateOnly> LockedDates { get; set; } = [];
        [Required]
        public List<AppointmentDto> PendingAppointments { get; set; } = [];
    }
}
