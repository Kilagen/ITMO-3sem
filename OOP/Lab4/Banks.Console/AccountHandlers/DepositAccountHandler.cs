using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Console.AccountHandlers
{
    public class DepositAccountHandler : IAccountHandler
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

            decimal initialDeposit = 0;
            if (args.Length > 1)
            {
                try
                {
                    initialDeposit = decimal.Parse(args[1]);
                }
                catch (Exception)
                {
                    throw new InvalidBankCommandException($"Invalid initial deposit `{args[3]}` format. Has to be a decimal");
                }
            }

            Bank bank = space.Banks[bankId];
            Client client = space.Clients[clientId];
            return bank.RegisterDepositAccount(client, initialDeposit);
        }

        public void Help()
        {
            System.Console.WriteLine("  <client_id> <bank_id> deposit <initial_deposit> - creates a deposit account");
            next?.Help();
        }

        public IAccountHandler SetNext(IAccountHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
