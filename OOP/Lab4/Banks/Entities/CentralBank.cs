using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Configs;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly List<Bank> banks;

        public CentralBank(Clock clock)
        {
            banks = new List<Bank>();
            Clock = clock;
        }

        public Clock Clock { get; }

        public Bank RegisterBank(string name, BankConfiguration bankConfiguration)
        {
            var bank = new Bank(name, this, bankConfiguration);
            banks.Add(bank);
            return bank;
        }

        public IBankAccount GetBankAccount(Guid accountId)
        {
            Bank? holder = banks.Find(bank => bank.Accounts.Any(account => account.Id == accountId));
            if (holder is null)
                throw new BankException("Account doesn't exist");
            return holder.Accounts.First(account => account.Id == accountId);
        }
    }
}
