﻿using SimpleAgenda.Aid.ExtensionClasses;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Enums;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Location : IDtoConvertable<LocationDto>, IPublicDtoConvertable<LocationOutDto>, IQueryFilter<AppointmentDto>
    {
        internal string PostalCode { get; private set; }
        internal string Street { get; private set; }
        internal string Number { get; private set; }
        internal string Neighborhood { get; private set; }
        internal string City { get; private set; }
        internal char[] State { get; private set; }
        internal string Country { get; private set; }
        internal string Complement { get; private set; }

        internal Location(string postalCode, string street, string number, string neighborhood, string city, string country, string state, string complement = "")
        {
            PostalCode = PostalCodeValidator(postalCode);
            Street = street.NullOrEmptyValidator();
            Number = number.NullOrEmptyValidator();
            Neighborhood = neighborhood.NullOrEmptyValidator();
            City = city.NullOrEmptyValidator();
            Country = country.NullOrEmptyValidator();
            State = StateValidator(state);
            Complement = complement.NullOrEmptyValidator();
        }

        public Location(LocationDto dto)
        {
            PostalCode = PostalCodeValidator(dto.PostalCode);
            Street = dto.Street.NullOrEmptyValidator();
            Number = dto.Number.NullOrEmptyValidator();
            Neighborhood = dto.Neighborhood.NullOrEmptyValidator();
            City = dto.City.NullOrEmptyValidator();
            Country = dto.Country.NullOrEmptyValidator();
            State = StateValidator(dto.State.ToString());
            Complement = dto.Complement ?? string.Empty;
        }

        public Location(LocationOutDto dto)
        {
            PostalCode = PostalCodeValidator(dto.PostalCode!);
            Street = dto.Street.NullOrEmptyValidator();
            Number = dto.Number.NullOrEmptyValidator();
            Neighborhood = dto.Neighborhood.NullOrEmptyValidator();
            City = dto.City.NullOrEmptyValidator();
            Country = dto.Country.NullOrEmptyValidator();
            State = StateValidator(dto.State.ToString());
            Complement = dto.Complement ?? string.Empty;
        }

        private static string PostalCodeValidator(string? value)
        {
            value?.NullOrEmptyValidator();

            if (value!.Count(d => char.IsDigit(d)) < 8)
                throw new ArgumentException("The provided value doesn't have the correct number of digits for a postal code.");

            return value!.NullOrEmptyValidator();
        }

        private static char[] StateValidator(string? value)
        {
            value.NullOrEmptyValidator();
            if (value!.Length < 2)
                throw new ArgumentException("The provided value doesn't have the correct length for a state code.");

            return value!.ToCharArray();
        }

        public LocationOutDto ConvertToPublicDto()
        {
            return new LocationOutDto
            {
                PostalCode = PostalCode,
                Street = Street,
                Number = Number,
                Neighborhood = Neighborhood,
                City = City,
                State = "SP",
                Country = Country,
                Complement = Complement
            };
        }

        public LocationDto ConvertToInternalDto()
        {
            return new LocationDto
            {
                PostalCode = PostalCode,
                Street = Street,
                Number = Number,
                Neighborhood = Neighborhood,
                City = City,
                State = State,
                Country = Country,
                Complement = Complement
            };
        }

        internal LocationDto Update(LocationOutDto location)
        {
            return new LocationDto
            {
                PostalCode = location.PostalCode ?? PostalCode,
                Street = location.Street ?? Street,
                Number = location.Number ?? Number,
                Neighborhood = location.Neighborhood ?? Neighborhood,
                City = location.City ?? City,
                State = location.State is not null ? StateValidator(location.State) : State,
                Country = location.Country ?? Country,
                Complement = location.Complement ?? Complement
            };
        }

        public static IQueryable<AppointmentDto> ApplyFilter(IQueryable<AppointmentDto> query, QueryDto param)
        {
            if (!string.IsNullOrWhiteSpace(param.PostalCode))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.PostalCode.Contains(param.PostalCode));

            if (!string.IsNullOrWhiteSpace(param.Street))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.Street.Contains(param.Street));

            if (!string.IsNullOrWhiteSpace(param.Number))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.Number.Contains(param.Number));

            if (!string.IsNullOrWhiteSpace(param.Neighborhood))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.Neighborhood.Contains(param.Neighborhood));

            if (!string.IsNullOrWhiteSpace(param.City))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.City.Contains(param.City));

            if (param.State is not null && param.State.Any())
                query = query.Where(a => a.Event.Location != null && new string(a.Event.Location.State.Take(2).ToArray()) == param.State);

            if (!string.IsNullOrWhiteSpace(param.Country))
                query = query.Where(a => a.Event.Location != null && a.Event.Location.Country.Contains(param.Country));

            if (!string.IsNullOrWhiteSpace(param.Complement))
                query = query.Where(a => a.Event.Location != null 
                && a.Event.Location.Complement != null 
                && a.Event.Location.Complement.Contains(param.Complement));

            return query;
        }

    }
}
