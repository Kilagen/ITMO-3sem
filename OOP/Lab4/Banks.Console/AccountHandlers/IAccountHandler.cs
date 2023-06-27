using Banks.Interfaces;

namespace Banks.Console.AccountHandlers
{
    public interface IAccountHandler
    {
        public IAccountHandler SetNext(IAccountHandler handler);
        public IBankAccount Handle(int clientId, int bankId, string[] args, DataSpace space);
        public void Help();
    }
}
