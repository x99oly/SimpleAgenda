/*
 * LocationDto represents an address with an auto-incremented primary key 'Id'.
 * The combination of PostalCode, Street, Number, 
 * is enforced as unique via Fluent API (Visit Context's folder to see implementation).
 * This ensures no two LocationDto entries have the exact same full address.
 * If a new Location with these same values is inserted, the unique constraint
 * prevents duplication, so the existing entry should be reused.
 */

using SimpleAgenda.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class LocationDto
    {
        [Key]
        internal int Id { get; set; }

        [Required]
        internal string PostalCode { get; set; } = string.Empty;

        [Required]
        internal string Street { get; set; } = string.Empty;

        [Required]
        internal string Number { get; set; } = string.Empty;

        [Required]
        internal string Neiborhood { get; set; } = string.Empty;

        [Required]
        internal string City { get; set; } = string.Empty;

        [Required]
        internal BrazilStatesEnum State { get; set; } = default;

        [Required]
        internal string Country { get; set; } = string.Empty;

        internal string? Complement { get; set; }
    }
}
