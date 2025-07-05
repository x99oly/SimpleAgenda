using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class AppointmentDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public EventDto Event { get; set; } = null!;

        public StatusEnum Status { get; set; } = StatusEnum.PENDING;
    }
}
