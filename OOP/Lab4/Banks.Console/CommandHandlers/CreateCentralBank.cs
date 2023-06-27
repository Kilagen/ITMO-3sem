using Banks.Console.ClockHandlers;
using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Models;

namespace Banks.Console.CommandHandlers
{
    public class CreateCentralBank : ICommandHandler
    {
        private ICommandHandler? next;
        private IClockHandler _clockHandler;
        public CreateCentralBank(IClockHandler clockHandler)
        {
            _clockHandler = clockHandler;
        }

        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 3 || args[0] != "create" || args[1] != "central" || args[2] != "bank")
            {
                if (next is null)
                    throw new NoHandlerException();

                next.Handle(args, space);
                return;
            }

            Clock clock = _clockHandler.Handle(args[3..]);
            space.CentralBank = new CentralBank(clock);

            System.Console.WriteLine("Central bank created");
        }

        public void Help()
        {
            System.Console.WriteLine("create central bank <clock_type> <clock_start_time> - creates a central bank");
            System.Console.WriteLine("  clock_type: clock");
            System.Console.WriteLine("  clock_start_time: dd/MM/yyyy");
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
