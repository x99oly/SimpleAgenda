﻿using SimpleAgenda.Enums;
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
        public string Neighborhood { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public char[] State { get; set; } = [];

        [Required]
        public string Country { get; set; } = string.Empty;

        public string? Complement { get; set; }
    }
}
