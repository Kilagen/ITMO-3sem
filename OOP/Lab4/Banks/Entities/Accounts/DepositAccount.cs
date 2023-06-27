using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Configs;

namespace Banks.Entities.Accounts
{
    public class DepositAccount : IBankAccount
    {
        private decimal monthResidue = 0;
        public DepositAccount(Client client, Bank bank, Clock clock, decimal initialDeposit)
        {
            Client = client;
            Bank = bank;
            Balance = new Balance(initialDeposit, 0, decimal.MaxValue);
            Id = Guid.NewGuid();
            DaysPassed = TimeSpan.FromDays(0);
            clock.Skip += AccrueResidue;

            Period = Bank.Configuration.DepositAccount.Period;
            Percent = Bank.Configuration.DepositAccount.PercentageModel.GetPercent(initialDeposit);
            UnsafeLimit = Bank.Configuration.DepositAccount.UnsafeLimit;
        }

        public Balance Balance { get; }

        public Bank Bank { get; }

        public Client Client { get; }

        public Guid Id { get; }
        public TimeSpan DaysPassed { get; private set; }
        public Percent Percent { get; private set; }
        public TimeSpan Period { get; private set; }
        public decimal UnsafeLimit { get; private set; }

        public INotifier? Notifier { get; set; }

        public bool CanDecrease(decimal amount)
        {
            return DaysPassed > Period && (Client.IsReliable || amount < UnsafeLimit) && Balance.CanDecrease(amount);
        }

        public bool CanIncrease(decimal amount)
        {
            return Balance.CanIncrease(amount);
        }

        public void Decrease(decimal amount)
        {
            if (DaysPassed < Period || (!Client.IsReliable && amount > UnsafeLimit))
                throw new BankException("Cannot decrease balance");

            Balance.Decrease(amount);
        }

        public void Increase(decimal amount)
        {
            Balance.Increase(amount);
            monthResidue += amount;
        }

        public void AccrueResidue()
        {
            DaysPassed += TimeSpan.FromDays(1);
            if (DaysPassed.TotalDays % 30 == 0)
            {
                Balance.Increase(monthResidue * Percent.Value / 12);
            }

            monthResidue = 0;
        }

        public void UpdateState()
        {
            DepositAccountConfiguration config = Bank.Configuration.DepositAccount;
            if (config.Period != Period)
            {
                Notifier?.Notify($"Deposit account {Id} period changed from {Period} to {config.Period}");
                Period = config.Period;
            }

            if (config.UnsafeLimit != UnsafeLimit)
            {
                Notifier?.Notify($"Deposit account {Id} unsafe limit changed from {UnsafeLimit} to {config.UnsafeLimit}");
                UnsafeLimit = config.UnsafeLimit;
            }
        }

        public override string ToString()
        {
            return $"Deposit account {Id} of {Client} at {Bank} with {Balance}";
        }
    }
}
