

namespace SimpleAgenda.DTOS.Publics
{
    public class EventOutDto
    {
        public int? Id { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public LocationOutDto? Location { get; set; } = null;

    }
}
