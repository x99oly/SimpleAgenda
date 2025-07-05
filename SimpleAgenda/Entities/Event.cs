using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Event : IDtoConvertable<EventDto>, IPublicDtoConvertable<EventOutDto>, IQueryFilter<AppointmentDto>
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

        public static IQueryable<AppointmentDto> ApplyFilter(IQueryable<AppointmentDto> query, QueryDto param)
        {
            if (param.EventId.HasValue)
                query = query.Where(a => a.Event.Id == param.EventId.Value);

            if (!string.IsNullOrWhiteSpace(param.EventTitle))
                query = query.Where(a => a.Event.Title.Contains(param.EventTitle));

            if (!string.IsNullOrWhiteSpace(param.EventDescription))
                query = query.Where(a => a.Event.Description.Contains(param.EventDescription));

            // Search term (in title/description)
            if (!string.IsNullOrWhiteSpace(param.SearchTerm))
            {
                var term = param.SearchTerm.ToLower();
                query = query.Where(a =>
                    a.Event.Title.ToLower().Contains(term) ||
                    a.Event.Description.ToLower().Contains(term));
            }

            // Delegar para Location
            return Location.ApplyFilter(query, param);
        }
    }
}
