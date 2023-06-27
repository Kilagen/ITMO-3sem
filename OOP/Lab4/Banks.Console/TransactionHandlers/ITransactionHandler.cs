using Banks.Interfaces;

namespace Banks.Console.TransactionHandlers
{
    public interface ITransactionHandler
    {
        public ITransactionHandler SetNext(ITransactionHandler handler);

        public ITransaction Handle(string[] args, DataSpace space);

        public void Help();
    }
}
