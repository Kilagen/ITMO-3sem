using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Console.AccountHandlers
{
    public class DebitAccountHandler : IAccountHandler
    {
        private IAccountHandler? next;
        public IBankAccount Handle(int clientId, int bankId, string[] args, DataSpace space)
        {
            if (args.Length == 0 || args[0] != "deposit")
            {
                if (next is null)
                    throw new InvalidBankCommandException("Invalid account type");

                return next.Handle(clientId, bankId, args, space);
            }

            Bank bank = space.Banks[bankId];
            Client client = space.Clients[clientId];
            return bank.RegisterDebitAccount(client);
        }

        public void Help()
        {
            System.Console.WriteLine("  <client_id> <bank_id> debit - creates a debit account");
            next?.Help();
        }

        public IAccountHandler SetNext(IAccountHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
