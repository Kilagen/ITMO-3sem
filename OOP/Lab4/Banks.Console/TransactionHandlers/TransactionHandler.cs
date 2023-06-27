using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Console.TransactionHandlers
{
    public class TransactionHandler : ITransactionHandler
    {
        private ITransactionHandler? next;
        public ITransaction Handle(string[] args, DataSpace space)
        {
            if (args.Length < 6 || args[0] != "transaction" || args[1] != "from" || args[3] != "to")
            {
                if (next is null)
                    throw new NoHandlerException();

                return next.Handle(args, space);
            }

            Guid senderId;
            Guid receiverId;
            decimal amount;
            try
            {
                senderId = Guid.Parse(args[2]);
                receiverId = Guid.Parse(args[4]);
                amount = decimal.Parse(args[5]);
            }
            catch (Exception)
            {
                throw new InvalidBankCommandException(
                    $"Invalid senderId `{args[2]}`, receiverId `{args[4]}` or amount `{args[5]}` format. Has to be two account id's and decimal");
            }

            if (amount <= 0)
                throw new InvalidBankCommandException($"Invalid amount `{args[5]}`. Has to be greater that 0");

            if (space.CentralBank is null)
                throw new BankConsoleException("Central bank is not created");

            IBankAccount sender = space.CentralBank.GetBankAccount(senderId);
            IBankAccount receiver = space.CentralBank.GetBankAccount(receiverId);

            return new Transaction(sender, receiver, amount);
        }

        public void Help()
        {
            System.Console.WriteLine("transaction from <senderId> to <receiverId> <amount> - creates a transaction");
            next?.Help();
        }

        public ITransactionHandler SetNext(ITransactionHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
