using Banks.Entities.Accounts;
using Banks.Interfaces;
using Banks.Models.Configs;

namespace Banks.Entities
{
    public class Bank
    {
        private List<IBankAccount> accounts;
        public Bank(string name, CentralBank cb, BankConfiguration bankConfiguration)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Bank name cannot be null or empty", nameof(name));
            Name = name;
            CentralBank = cb;
            accounts = new List<IBankAccount>();
            Id = Guid.NewGuid();

            Configuration = bankConfiguration;
        }

        public Guid Id { get; }
        public string Name { get; }

        public BankConfiguration Configuration { get; private set; }

        public CentralBank CentralBank { get; }
        public IReadOnlyCollection<IBankAccount> Accounts => accounts.AsReadOnly();

        public CreditAccount RegisterCreditAccount(Client client)
        {
            var account = new CreditAccount(client, this);
            accounts.Add(account);
            return account;
        }

        public DebitAccount RegisterDebitAccount(Client client)
        {
            var account = new DebitAccount(client, this, CentralBank.Clock);
            accounts.Add(account);
            return account;
        }

        public DepositAccount RegisterDepositAccount(Client client, decimal initialDeposit)
        {
            var account = new DepositAccount(client, this, CentralBank.Clock, initialDeposit);
            accounts.Add(account);
            return account;
        }

        public void UpdateConfiguration(BankConfiguration configuration)
        {
            Configuration = configuration;
            accounts.ForEach(ac => ac.UpdateState());
        }

        public override string ToString()
        {
            return $"Bank {Name}";
        }
    }
}
