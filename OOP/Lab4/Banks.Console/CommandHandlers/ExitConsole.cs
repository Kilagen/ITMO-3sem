namespace Banks.Console.CommandHandlers
{
    public class ExitConsole : ICommandHandler
    {
        private ICommandHandler? next;
        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length == 0 || args[0] != "exit")
            {
                if (next is null)
                {
                    throw new ArgumentException("Invalid command");
                }

                next.Handle(args, space);
                return;
            }

            space.Exit = true;
            System.Console.WriteLine("Exiting...");
        }

        public void Help()
        {
            System.Console.WriteLine("exit - exits the console");
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
