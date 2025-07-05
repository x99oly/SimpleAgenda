using SimpleAgenda.Enums;
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

//public class EventOutDto
//{
//    public int? Id { get; set; } = null;
//    public string? Title { get; set; } = null;
//    public string? Description { get; set; } = null;
//    public LocationOutDto? Location { get; set; } = null;

//}
//public class LocationOutDto
//{
//    public string? PostalCode { get; set; }
//    public string? Street { get; set; }
//    public string? Number { get; set; }
//    public string? Neighborhood { get; set; } // Adicionado caso precise
//    public string? City { get; set; }
//    public BrazilStatesEnum? State { get; set; }
//    public string? Country { get; set; }
//    public string? Complement { get; set; }
//}
