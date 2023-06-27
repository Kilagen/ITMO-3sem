using Banks.Console.Exceptions;
using Banks.Entities;

namespace Banks.Console.CommandHandlers
{
    public class GetClientAccounts : ICommandHandler
    {
        private ICommandHandler? next;
        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 3 || args[0] != "get" || args[1] != "accounts")
            {
                if (next is null)
                    throw new NoHandlerException();

                next.Handle(args, space);
                return;
            }

            int clientId;
            try
            {
                clientId = int.Parse(args[2]);
            }
            catch (Exception)
            {
                throw new InvalidBankCommandException($"Invalid client_id `{args[2]}`. Has to be an integer");
            }

            if (clientId >= space.Clients.Count || clientId < 0)
                throw new InvalidBankCommandException($"Invalid client_id `{args[2]}`. Has to be in range [0, {space.Clients.Count})");

            Client client = space.Clients[clientId];

            foreach (Bank bank in space.Banks)
            {
                var accounts = bank.Accounts.Where(acc => acc.Client == client).ToList();
                if (accounts.Count == 0)
                    continue;
                System.Console.WriteLine($"Account in {bank.Name}");
                accounts.ForEach(acc => System.Console.WriteLine(acc));
            }
        }

        public void Help()
        {
            System.Console.WriteLine("get accounts <client_id> - get all client accounts");
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
