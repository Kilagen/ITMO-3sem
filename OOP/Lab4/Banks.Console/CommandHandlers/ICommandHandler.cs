namespace Banks.Console.CommandHandlers
{
    public interface ICommandHandler
    {
        public ICommandHandler SetNext(ICommandHandler handler);
        public void Handle(string[] args, DataSpace space);
        public void Help();
    }
}
