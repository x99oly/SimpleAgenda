using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class LocationDto
    {
        [Required]
        internal string Street { get; set; } = string.Empty;

        [Required]
        internal string Number { get; set; } = string.Empty;

        [Required]
        internal string City { get; set; } = string.Empty;

        [Required]
        internal string PostalCode { get; set; } = string.Empty;

        [Required]
        internal string Country { get; set; } = string.Empty;

        [Required]
        internal BrazilStatesEnum State { get; set; } = default;

        internal string? Complement { get; set; }
    }
}
