namespace Banks.Console.CommandHandlers
{
    public class HelpConsole : ICommandHandler
    {
        private ICommandHandler? next;
        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length == 0 || args[0] != "help")
            {
                if (next is null)
                {
                    throw new ArgumentException("Invalid command");
                }

                next.Handle(args, space);
                return;
            }

            Help();
        }

        public void Help()
        {
            System.Console.WriteLine("Console is case insensitive");
            System.Console.WriteLine("Available commands:");
            System.Console.WriteLine("help - show this message");
            if (next is null)
            {
                throw new ArgumentException("Invalid command");
            }

            next.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
