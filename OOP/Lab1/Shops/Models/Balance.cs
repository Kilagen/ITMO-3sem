using Shops.Exceptions;

namespace Shops.Models
{
    public class Balance
    {
        public Balance(decimal amount = 0)
        {
            ValidateBalance(amount);
            Amount = amount;
        }

        public decimal Amount { get; private set; }

        public decimal Increase(decimal amount)
        {
            ValidateBalance(amount);
            Amount += amount;
            return Amount;
        }

        public decimal Decrease(decimal amount)
        {
            ValidateBalance(amount);
            if (Amount - amount < 0)
            {
                throw new InvalidBalanceException(Amount, amount);
            }

            Amount -= amount;
            return Amount;
        }

        private void ValidateBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new InvalidBalanceException(amount);
            }
        }
    }
}
