using SimpleAgenda.Aid.ExtensionClasses;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Enums;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Entities
{
    internal class Location : IDtoConvertable<LocationDto>, IPublicDtoConvertable<LocationOutDto>
    {
        internal string Street { get; private set; }
        internal string City { get; private set; }
        internal string PostalCode { get; private set; }
        internal string Country { get; private set; }
        internal BrazilStatesEnum State { get; private set; }
        internal string Complement { get; private set; }

        internal Location(string street, string city, string postalCode, string country, string state, string complement = "")
        {
            Street = street.NullOrEmptyValidator();
            City = city.NullOrEmptyValidator();
            PostalCode = PostalCodeValidator(postalCode);
            Country = country.NullOrEmptyValidator();
            State = StateValidator(state);
            Complement = complement.NullOrEmptyValidator();
        }

        public Location(SimpleAgenda.DTOS.Internals.LocationDto dto)
        {
            Street = dto.Street.NullOrEmptyValidator();
            City = dto.City.NullOrEmptyValidator();
            PostalCode = PostalCodeValidator(dto.PostalCode);
            Country = dto.Country.NullOrEmptyValidator();
            State = StateValidator(dto.State.ToString());
            Complement = dto.Complement != null ? dto.Complement : string.Empty;
        }

        public Location(SimpleAgenda.DTOS.Publics.LocationOutDto dto)
        {
            Street = dto.Street.NullOrEmptyValidator();
            City = dto.City.NullOrEmptyValidator();
            PostalCode = PostalCodeValidator(dto.PostalCode);
            Country = dto.Country.NullOrEmptyValidator();
            State = StateValidator(dto.State.ToString());
            Complement = dto.Complement != null ? dto.Complement : string.Empty;
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
                Street = Street,
                City = City,
                PostalCode = PostalCode,
                Country = Country,
                State = State,
                Complement = Complement
            };
        }

        public LocationDto ConvertToInternalDto()
        {
            return new LocationDto
            {
                Street = Street,
                City = City,
                PostalCode = PostalCode,
                Country = Country,
                State = State,
                Complement = Complement
            };

        }

        internal LocationDto Update(LocationOutDto location)
        {
            return new LocationDto
            {
                Street = location.Street ?? Street,
                City = location.City ?? City,
                PostalCode = location.PostalCode ?? PostalCode,
                Country = location.Country ?? Country,
                State = location.State ?? State,
                Complement = location.Complement ?? Complement
            };
        }
    }
}
