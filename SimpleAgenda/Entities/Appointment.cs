using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Appointment : IDtoConvertable<AppointmentDto>, IPublicDtoConvertable<AppointmentOutDto>
    {
        internal readonly int Id;
        internal DateTime Date { get; private set; }
        internal Event Event {  get; private set; }

        internal Appointment(DateTime date, SimpleAgenda.Entities.Event newEvent)
        {
            Id = Aid.AidClasses.AidIdentifier.RandomIntId(4);
            Date = DateValidador(date);
            Event = newEvent; 
        }

        internal Appointment(DateTime date, string title, string? description = null, Location? location = null)
        {
            Id = Aid.AidClasses.AidIdentifier.RandomIntId(4);
            Date = DateValidador(date);
            Event = new SimpleAgenda.Entities.Event(title, description, location);
        }

        internal Appointment(AppointmentDto dto)
        {
            Id = dto.Id;
            Date = dto.Date;
            Event = new Event(dto.Event);
        }

        internal Appointment(AppointmentOutDto dto)
        {
            if (dto.Id is 0)
                throw new ArgumentException("Invalid AppointmentOutDto provided.");
            Id = dto.Id;

            Date = dto.Date ?? throw new InvalidOperationException("The parameter 'Date' must be provided in constructor.");

            if (dto.Event is null)
                throw new ArgumentException("The parameter 'Event' must be provided in constructor.");
            Event = new Event(dto.Event);
        }

        internal void Update(AppointmentOutDto dto)
        {
            Date = dto.Date is not null ? DateValidador(dto.Date) : Date;
            Event = dto.Event is null ? this.Event : Event.Update(dto.Event);
        }

        public AppointmentOutDto ConvertToPublicDto()
        {
            return new AppointmentOutDto
            {
                Id = Id,
                Date = Date,
                Event = Event.ConvertToPublicDto()
            };
        }

        public AppointmentDto ConvertToInternalDto()
        {
            return new AppointmentDto
            {
                Id = Id,
                Date = Date,
                Event = Event.ConvertToInternalDto()
            };
        }


        // Validation method to ensure the appointment is valid
        
        private static DateTime DateValidador(DateTime? date)
        {
            if (date is null)
                throw new ArgumentNullException(nameof(date), "The appointment date cannot be null.");
            if (date < DateTime.Now)
                throw new ArgumentException("The appointment date cannot be in the past.", nameof(date));

            return date.Value;
        }
    }
}
