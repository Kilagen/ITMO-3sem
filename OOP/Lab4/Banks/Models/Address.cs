using Banks.Exceptions;

namespace Banks.Models
{
    public class Address
    {
        public Address(string city, string street, string house, string apartment)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ClientException("Address city cannot be null or empty");
            if (string.IsNullOrWhiteSpace(street))
                throw new ClientException("Address street cannot be null or empty");
            if (string.IsNullOrWhiteSpace(house))
                throw new ClientException("Address house cannot be null or empty");
            if (string.IsNullOrWhiteSpace(apartment))
                throw new ClientException("Address apartment cannot be null or empty");
            City = city;
            Street = street;
            House = house;
            Apartment = apartment;
        }

        private Address()
        {
            City = string.Empty;
            Street = string.Empty;
            House = string.Empty;
            Apartment = string.Empty;
        }

        public static Address Empty => new Address();

        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public string Apartment { get; }

        public override string ToString()
        {
            return $"{City} {Street} {House} {Apartment}";
        }
    }
}
