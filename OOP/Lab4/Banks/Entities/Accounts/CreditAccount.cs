using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Configs;

namespace Banks.Entities.Accounts
{
    public class CreditAccount : IBankAccount
    {
        public CreditAccount(Client client, Bank bank)
        {
            Client = client;
            Bank = bank;
            Balance = new Balance(0, decimal.MinValue, decimal.MaxValue);
            Id = Guid.NewGuid();
            Comission = Bank.Configuration.CreditAccount.Comission;
            UnsafeLimit = Bank.Configuration.CreditAccount.UnsafeLimit;
        }

        public Balance Balance { get; }

        public Bank Bank { get; }

        public Client Client { get; }

        public Guid Id { get; }

        public decimal Comission { get; private set; }
        public decimal UnsafeLimit { get; private set; }

        public INotifier? Notifier { get; set; }

        public bool CanDecrease(decimal amount)
        {
            if (Balance.Amount < amount)
            {
                amount += Comission;
            }

            return Balance.CanDecrease(amount) && (Client.IsReliable || amount < UnsafeLimit);
        }

        public bool CanIncrease(decimal amount)
        {
            return Balance.CanIncrease(amount);
        }

        public void Decrease(decimal amount)
        {
            if (Balance.Amount < amount)
            {
                amount += Comission;
            }

            if (!Client.IsReliable && amount < UnsafeLimit)
                throw new ArgumentException("Cannot decrease balance");
        }

        public void Increase(decimal amount)
        {
            Balance.Increase(amount);
        }

        public void UpdateState()
        {
            CreditAccountConfiguration config = Bank.Configuration.CreditAccount;
            if (config.Comission != Comission)
            {
                Notifier?.Notify($"Credit account {Id} comission changed from {Comission} to {config.Comission}");
                Comission = config.Comission;
            }

            if (config.UnsafeLimit != UnsafeLimit)
            {
                Notifier?.Notify($"Credit account {Id} unsafe transaction limit changed from {UnsafeLimit} to {config.UnsafeLimit}");
                UnsafeLimit = config.UnsafeLimit;
            }
        }

        public override string ToString()
        {
            return $"Credit account {Id} of {Client.Name} in {Bank} with {Balance}";
        }
    }
}
