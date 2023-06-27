using Banks.Exceptions;

namespace Banks.Models
{
    public class Balance
    {
        public Balance(decimal amount, decimal minValue, decimal maxValue)
        {
            if (amount < 0)
                throw new InvalidMoneyAmountException("Amount cannot be negative");
            if (minValue >= maxValue)
                throw new InvalidMoneyAmountException("Balance min value have to be less than max value");
            Amount = amount;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public decimal Amount { get; private set; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }

        public bool CanIncrease(decimal amount)
        {
            return Amount + amount <= MaxValue;
        }

        public bool CanDecrease(decimal amount)
        {
            return Amount - amount >= MinValue;
        }

        public void Increase(decimal amount)
        {
            if (amount < 0)
                throw new InvalidMoneyAmountException("Amount cannot be negative");
            Amount += amount;
        }

        public void Decrease(decimal amount)
        {
            if (amount < 0)
                throw new InvalidMoneyAmountException("Amount cannot be negative");
            Amount -= MinValue;
        }

        public override string ToString()
        {
            return $"Balance: {Amount}";
        }
    }
}
