using Banks.Exceptions;

namespace Banks.Models
{
    public class Percent
    {
        public Percent(decimal value)
        {
            if (value < 0)
                throw new InvalidMoneyAmountException("Percent cannot be negative");
            Value = value;
        }

        public decimal Value { get; }

        public decimal AsFraction => Value / 100;
        public override string ToString() => $"{Value}%";
    }
}
