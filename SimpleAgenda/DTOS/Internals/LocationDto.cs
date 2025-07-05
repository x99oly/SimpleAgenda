using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class LocationDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public string Neiborhood { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public BrazilStatesEnum State { get; set; } = default;

        [Required]
        public string Country { get; set; } = string.Empty;

        public string? Complement { get; set; }
    }
}
