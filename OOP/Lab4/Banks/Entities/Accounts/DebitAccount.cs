using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Configs;

namespace Banks.Entities.Accounts
{
    public class DebitAccount : IBankAccount
    {
        private decimal _dayResidue;
        private decimal _monthResidue;
        private decimal _monthDay;
        public DebitAccount(Client client, Bank bank, Clock clock)
        {
            Client = client;
            Bank = bank;
            Balance = new Balance(0, 0, decimal.MaxValue);
            Id = Guid.NewGuid();
            _dayResidue = 0;
            _monthDay = 0;
            Percent = Bank.Configuration.DebitAccount.Percent;
            UnsafeLimit = Bank.Configuration.DebitAccount.UnsafeLimit;

            clock.Skip += AccrueResidue;
        }

        public Balance Balance { get; }

        public Guid Id { get; }

        public Bank Bank { get; }

        public Client Client { get; }

        public Percent Percent { get; private set; }
        public decimal UnsafeLimit { get; private set; }

        public INotifier? Notifier { get; set; }

        public bool CanDecrease(decimal amount)
        {
            return Balance.CanDecrease(amount) && (Client.IsReliable || amount < UnsafeLimit);
        }

        public bool CanIncrease(decimal amount)
        {
            return Balance.CanIncrease(amount);
        }

        public void Decrease(decimal amount)
        {
            if (!Client.IsReliable && amount < UnsafeLimit)
                throw new BankException("Cannot decrease balance");
            Balance.Decrease(amount);
        }

        public void Increase(decimal amount)
        {
            _dayResidue += amount;
            Balance.Increase(amount);
        }

        public void AccrueResidue()
        {
            _monthResidue += _dayResidue * Percent.AsFraction;
            _dayResidue = 0;
            _monthDay++;

            if (_monthDay == 30)
            {
                Balance.Increase(_monthResidue / 12);
                _monthResidue = 0;
                _monthDay = 0;
            }
        }

        public void UpdateState()
        {
            DebitAccountConfiguration config = Bank.Configuration.DebitAccount;
            if (config.UnsafeLimit != UnsafeLimit)
            {
                Notifier?.Notify($"Debit account {Id} UnsafeLimit changed from {UnsafeLimit} to {config.UnsafeLimit}");
                UnsafeLimit = config.UnsafeLimit;
            }

            if (config.Percent != Percent)
            {
                Notifier?.Notify($"Debit account {Id} Percent changed from {Percent} to {config.Percent}");
                Percent = config.Percent;
            }
        }

        public override string ToString()
        {
            return $"Debit account {Id} of {Client} in {Bank} with {Balance}";
        }
    }
}
