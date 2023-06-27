using Banks.Console.Exceptions;

namespace Banks.Console.CommandHandlers
{
    public class SkipDay : ICommandHandler
    {
        private ICommandHandler? next;
        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length == 0 || args[0] != "skip" || args[1] != "day")
            {
                if (next is null)
                    throw new NoHandlerException();
                next.Handle(args, space);
                return;
            }

            if (space.CentralBank is null)
                throw new BankConsoleException("Central bank is not created");

            space.CentralBank.Clock.Tick();

            System.Console.WriteLine("Day skipped");
        }

        public void Help()
        {
            System.Console.WriteLine("skip day - skips a day");

            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
