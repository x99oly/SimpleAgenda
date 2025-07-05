using Microsoft.Extensions.Logging;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Enums;
using SimpleAgenda.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleAgenda.Entities
{
    internal class Appointment : IDtoConvertable<AppointmentDto>, IPublicDtoConvertable<AppointmentOutDto>, IQueryFilter<AppointmentDto>
    {
        internal readonly int Id;
        internal StatusEnum Status { get; private set; } = StatusEnum.PENDING;
        internal DateTime Date { get; private set; }
        internal Event Event {  get; private set; }
        

        public Type ElementType => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

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

        public static IQueryable<AppointmentDto> ApplyFilter(IQueryable<AppointmentDto> query, QueryDto param)
        {
            // Appointment filters
            if (param.AppointmentId.HasValue)
                query = query.Where(a => a.Id == param.AppointmentId.Value);

            if (param.Date.HasValue)
                query = query.Where(a => a.Date == param.Date);

            if (param.DateStart.HasValue && param.DateEnd.HasValue)
                query = query.Where(a => a.Date >= param.DateStart.Value && a.Date <= param.DateEnd.Value);
            else if (param.DateStart.HasValue)
                query = query.Where(a => a.Date >= param.DateStart.Value);
            else if (param.DateEnd.HasValue)
                query = query.Where(a => a.Date <= param.DateEnd.Value);

            // Event filters
            if (param.EventId.HasValue)
                query = query.Where(a => a.Event.Id == param.EventId.Value);

            if (!string.IsNullOrWhiteSpace(param.EventTitle))
                query = query.Where(a => a.Event.Title.Contains(param.EventTitle));

            if (!string.IsNullOrWhiteSpace(param.EventDescription))
                query = query.Where(a => a.Event.Description.Contains(param.EventDescription));

            // Location filters (only if Location is not null)
            query = query.Where(a => a.Event.Location != null);

            if (!string.IsNullOrWhiteSpace(param.PostalCode))
                query = query.Where(a => a.Event.Location.PostalCode.Contains(param.PostalCode));

            if (!string.IsNullOrWhiteSpace(param.Street))
                query = query.Where(a => a.Event.Location.Street.Contains(param.Street));

            if (!string.IsNullOrWhiteSpace(param.Number))
                query = query.Where(a => a.Event.Location.Number.Contains(param.Number));

            if (!string.IsNullOrWhiteSpace(param.Neighborhood))
                query = query.Where(a => a.Event.Location.Neighborhood.Contains(param.Neighborhood));

            if (!string.IsNullOrWhiteSpace(param.City))
                query = query.Where(a => a.Event.Location.City.Contains(param.City));

            if (param.State.HasValue)
                query = query.Where(a => a.Event.Location.State == param.State);

            if (!string.IsNullOrWhiteSpace(param.Country))
                query = query.Where(a => a.Event.Location.Country.Contains(param.Country));

            if (!string.IsNullOrWhiteSpace(param.Complement))
                query = query.Where(a => a.Event.Location.Complement.Contains(param.Complement));

            // Status filters
            if (param.Status.HasValue)
                query = query.Where(a => a.Status == param.Status);

            if (param.StatusIn is { Count: > 0 })
                query = query.Where(a => param.StatusIn.Contains(a.Status));

            if (param.IncludeCancelled.HasValue && !param.IncludeCancelled.Value)
                query = query.Where(a => a.Status != StatusEnum.CANCELLED);

            // Search term (in title/description)
            if (!string.IsNullOrWhiteSpace(param.SearchTerm))
            {
                var term = param.SearchTerm.ToLower();
                query = query.Where(a =>
                    a.Event.Title.ToLower().Contains(term) ||
                    a.Event.Description.ToLower().Contains(term));
            }

            // Ordering
            if (!string.IsNullOrWhiteSpace(param.OrderBy))
            {
                var order = param.OrderBy.Trim();
                if (order == "Date")
                    query = query.OrderBy(a => a.Date);
                else if (order == "Date DESC")
                    query = query.OrderByDescending(a => a.Date);
                else if (order == "Event.Title")
                    query = query.OrderBy(a => a.Event.Title);
                else if (order == "Event.Description")
                    query = query.OrderBy(a => a.Event.Description);
                else
                    query = query.OrderBy(a => a.Id); // default
            }
            else
            {
                query = query.OrderBy(a => a.Id); // default if no ordering provided
            }

            // Pagination
            if (param.Skip.HasValue)
                query = query.Skip(param.Skip.Value);

            if (param.Take.HasValue)
                query = query.Take(param.Take.Value);

            return query;
        }

    }
}
