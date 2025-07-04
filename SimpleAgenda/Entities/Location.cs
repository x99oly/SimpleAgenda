using SimpleAgenda.Aid.ExtensionClasses;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Enums;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Location : IDtoConvertable<LocationDto>, IPublicDtoConvertable<LocationOutDto>
    {
        internal string PostalCode { get; private set; }
        internal string Street { get; private set; }
        internal string Number { get; private set; }
        internal string Neighborhood { get; private set; }
        internal string City { get; private set; }
        internal BrazilStatesEnum State { get; private set; }
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
            Neighborhood = dto.Neiborhood.NullOrEmptyValidator();
            City = dto.City.NullOrEmptyValidator();
            Country = dto.Country.NullOrEmptyValidator();
            State = StateValidator(dto.State.ToString());
            Complement = dto.Complement ?? string.Empty;
        }

        public Location(LocationOutDto dto)
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

        private static string PostalCodeValidator(string value)
        {
            if (value.Count(d => char.IsDigit(d)) < 8)
                throw new ArgumentException("The provided value doesn't have the correct number of digits for a postal code.");

            return value.NullOrEmptyValidator();
        }

        private static BrazilStatesEnum StateValidator(string value)
        {
            value = value.NullOrEmptyValidator();
            if (!Enum.TryParse(value, out BrazilStatesEnum estado))
                throw new ArgumentException("The provided value is not a valid state of Brazil.");

            return estado;
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
                State = State,
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
                Neiborhood = Neighborhood,
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
                Neiborhood = location.Neighborhood ?? Neighborhood,
                City = location.City ?? City,
                State = location.State ?? State,
                Country = location.Country ?? Country,
                Complement = location.Complement ?? Complement
            };
        }
    }
}
