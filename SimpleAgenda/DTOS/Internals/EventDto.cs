using System.ComponentModel.DataAnnotations;

namespace SimpleAgenda.DTOS.Internals
{
    internal class EventDto
    {
        [Key]
        internal int Id { get; set; }
        [Required]
        internal string Title { get; set; } = string.Empty;
        internal string? Description { get; set; }
        internal LocationDto? Location { get; set; }
    }
}
