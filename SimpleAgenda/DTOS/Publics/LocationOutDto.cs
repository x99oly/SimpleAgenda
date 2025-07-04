using SimpleAgenda.Enums;

namespace SimpleAgenda.DTOS.Publics
{
    public class LocationOutDto
    {
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public BrazilStatesEnum? State { get; set; }
        public string? Complement { get; set; }
    }
}
