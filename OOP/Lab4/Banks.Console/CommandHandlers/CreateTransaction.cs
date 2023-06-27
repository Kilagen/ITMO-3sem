using Banks.Console.Exceptions;
using Banks.Console.TransactionHandlers;
using Banks.Interfaces;

namespace Banks.Console.CommandHandlers
{
    public class CreateTransaction : ICommandHandler
    {
        private ICommandHandler? next;
        private ITransactionHandler _transactionHandler;

        public CreateTransaction(ITransactionHandler transactionHandler)
        {
            _transactionHandler = transactionHandler;
        }

        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 4 || args[0] != "create" || args[1] != "transaction")
            {
                if (next is null)
                    throw new NoHandlerException();

                next.Handle(args, space);
                return;
            }

            ITransaction transaction = _transactionHandler.Handle(args[2..], space);
            System.Console.WriteLine($"{transaction} created");
        }

        public void Help()
        {
            System.Console.WriteLine("create transaction <name> <args> - creates a transaction");
            _transactionHandler.Help();
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
