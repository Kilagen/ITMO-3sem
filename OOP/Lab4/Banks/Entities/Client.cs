using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities
{
    public class Client
    {
        private Client(string name, string surname, Address address, Passport passport)
        {
            Name = name;
            Surname = surname;
            Address = address;
            Passport = passport;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public Address Address { get; set; }
        public Passport Passport { get; set; }
        public bool IsReliable => Passport != Passport.Empty && Address != Address.Empty;
        public string FullName => $"{Name} {Surname}";

        public static ClientBuilder Builder(string name, string surname) => new ClientBuilder(name, surname);

        public override string ToString()
        {
            return $"Client {FullName} {Address} {Passport}";
        }

        public class ClientBuilder
        {
            private string _name;
            private string _surname;
            private Address _address;
            private Passport _passport;

            public ClientBuilder(string name, string surname)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ClientException("Name cannot be null or empty");
                if (string.IsNullOrWhiteSpace(surname))
                    throw new ClientException("Surname cannot be null or empty");
                _name = name;
                _surname = surname;
                _address = Address.Empty;
                _passport = Passport.Empty;
            }

            public ClientBuilder WithAddress(Address address)
            {
                _address = address;
                return this;
            }

            public ClientBuilder WithPassport(Passport passport)
            {
                _passport = passport;
                return this;
            }

            public Client Build()
            {
                return new Client(_name, _surname, _address, _passport);
            }
        }
    }
}
