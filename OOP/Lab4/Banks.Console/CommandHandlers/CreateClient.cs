using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Models;

namespace Banks.Console.CommandHandlers
{
    public class CreateClient : ICommandHandler
    {
        private ICommandHandler? next;

        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 4 || args[0] != "create" || args[1] != "client")
            {
                if (next is null)
                    throw new NoHandlerException();

                next.Handle(args, space);
                return;
            }

            string name = args[2];
            string surname = args[3];
            Passport passport = ParsePassport();
            Address address = ParseAddress();
            Client client = Client.Builder(name, surname).WithPassport(passport).WithAddress(address).Build();
            space.Add(client);
            System.Console.WriteLine($"Client {client} created");
        }

        public void Help()
        {
            System.Console.WriteLine("create client <name> <surname> - creates a client");
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }

        private Passport ParsePassport()
        {
            System.Console.Write("Enter passport series: ");
            string series = System.Console.ReadLine() !.Split()[0];
            System.Console.Write("Enter passport number: ");
            string number = System.Console.ReadLine() !.Split()[0];
            return new Passport(series, number);
        }

        private Address ParseAddress()
        {
            System.Console.Write("Enter city: ");
            string city = System.Console.ReadLine() !.Split()[0];
            System.Console.Write("Enter street: ");
            string street = System.Console.ReadLine() !.Split()[0];
            System.Console.Write("Enter house: ");
            string house = System.Console.ReadLine() !.Split()[0];
            System.Console.Write("Enter apartment: ");
            string apartment = System.Console.ReadLine() !.Split()[0];
            return new Address(city, street, house, apartment);
        }
    }
}
