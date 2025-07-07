using SimpleAgenda.Enums;

namespace SimpleAgenda.DTOS.Publics
{
    public class LocationOutDto
    {
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Neighborhood { get; set; } // Adicionado caso precise
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Complement { get; set; }
    }
}
