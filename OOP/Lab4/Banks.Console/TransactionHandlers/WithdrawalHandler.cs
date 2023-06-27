using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Console.TransactionHandlers
{
    public class WithdrawalHandler : ITransactionHandler
    {
        private ITransactionHandler? next;
        public ITransaction Handle(string[] args, DataSpace space)
        {
            if (args.Length < 4 || args[0] != "withdrawal" || args[1] != "from")
            {
                if (next is null)
                    throw new NoHandlerException();

                return next.Handle(args, space);
            }

            Guid accountId;
            decimal amount;
            try
            {
                accountId = Guid.Parse(args[2]);
                amount = decimal.Parse(args[3]);
            }
            catch (Exception)
            {
                throw new InvalidBankCommandException(
                    $"Invalid accountId `{args[2]}` or amount `{args[3]}` format. Has to be account id and decimal");
            }

            if (amount <= 0)
                throw new InvalidBankCommandException($"Invalid amount `{args[3]}`. Has to be greater that 0");

            if (space.CentralBank is null)
                throw new BankConsoleException("Central bank is not created");

            IBankAccount account = space.CentralBank.GetBankAccount(accountId);

            return new Withdrawal(account, amount);
        }

        public void Help()
        {
            System.Console.WriteLine("withdrawal from <accountId> <amount> - withdraws money from account");
            next?.Help();
        }

        public ITransactionHandler SetNext(ITransactionHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
