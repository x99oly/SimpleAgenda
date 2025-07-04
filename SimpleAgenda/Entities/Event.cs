using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Event : IDtoConvertable<EventDto>, IPublicDtoConvertable<EventOutDto>
    {
        internal readonly int id;
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal Location? Location { get; set; }

        internal Event(string title, string? description = null, Location? location = null)
        {
            id = Aid.AidClasses.AidIdentifier.RandomIntId(4);
            Title = title;
            Description = description ?? string.Empty;
            Location = location;
        }

        internal Event(EventDto dto)
        {
            id = dto.Id;
            Title = dto.Title;
            Description = dto.Description ?? string.Empty;
            Location = dto.Location != null ? new Location(dto.Location) : null;
        }

        internal Event(EventOutDto dto)
        {
            id = dto.Id ?? Aid.AidClasses.AidIdentifier.RandomIntId(4);
            Title = dto.Title ?? throw new ArgumentException(nameof(dto.Title),$"The event title must be provided.");
            Description = dto.Description ?? string.Empty;
            Location = dto.Location != null ? new Location(dto.Location) : null;
        }

        public EventOutDto ConvertToPublicDto()
        {
            return new EventOutDto
            {
                Id = id,
                Title = Title,
                Description = Description,
                Location = Location?.ConvertToPublicDto()
            };
        }

        public EventDto ConvertToInternalDto()
        {
            return new EventDto
            {
                Id = id,
                Title = Title,
                Description = Description,
                Location = Location?.ConvertToInternalDto()
            };
        }

        internal Event Update(EventOutDto @event)
        {
            return new Event(
                new EventDto
                {
                    Id = this.id,
                    Title = @event.Title ?? Title,
                    Description = @event.Description ?? Description,
                    Location = @event.Location != null 
                    ? Location?.Update(@event.Location) 
                    : this.Location?.ConvertToInternalDto()
                }
            );
        }
    }
}
