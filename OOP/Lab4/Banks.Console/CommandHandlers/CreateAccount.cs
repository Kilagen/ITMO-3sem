using Banks.Console.AccountHandlers;
using Banks.Console.Exceptions;
using Banks.Interfaces;

namespace Banks.Console.CommandHandlers
{
    public class CreateAccount : ICommandHandler
    {
        private ICommandHandler? next;
        private IAccountHandler _accountHandler;

        public CreateAccount(IAccountHandler accountHandler)
        {
            _accountHandler = accountHandler;
        }

        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 5 || args[0] != "create" || args[1] != "account")
            {
                if (next is null)
                    throw new NoHandlerException();

                next.Handle(args, space);
                return;
            }

            int bankId;
            int clientId;
            try
            {
                clientId = int.Parse(args[2]);
                bankId = int.Parse(args[3]);
            }
            catch (Exception)
            {
                throw new InvalidBankCommandException($"Invalid client_id `{args[0]}` or bank_id `{args[1]}` format. Has to be an integer");
            }

            if (clientId >= space.Clients.Count || clientId < 0)
                throw new InvalidBankCommandException($"Invalid client_id `{args[0]}`. Has to be in range [0, {space.Clients.Count})");

            if (bankId >= space.Banks.Count || bankId < 0)
                throw new InvalidBankCommandException($"Invalid bank_id `{args[1]}`. Has to be in range [0, {space.Banks.Count})");

            IBankAccount account = _accountHandler.Handle(clientId, bankId, args[4..], space);
            System.Console.WriteLine($"account created with id {account.Id}");
        }

        public void Help()
        {
            System.Console.WriteLine("create account <client_id> <bank_id> <account_type> <account_args> - creates an account");
            _accountHandler.Help();
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
