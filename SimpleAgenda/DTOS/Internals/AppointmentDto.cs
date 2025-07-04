using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class AppointmentDto
    {
        [Key]
        internal int Id { get; set; }
        [Required]
        internal DateTime Date { get; set; }

        [Required]
        internal EventDto Event { get; set; } = null!;
    }
}
