using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Publics
{
    public class AppointmentOutDto
    {
        public int Id { get; set; } = 0;
        public DateTime? Date { get; set; } = null;
        public EventOutDto Event { get; set; } = null!;
    }
}
