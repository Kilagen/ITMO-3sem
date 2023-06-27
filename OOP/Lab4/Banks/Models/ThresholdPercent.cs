namespace Banks.Models
{
    public class ThresholdPercent
    {
        public ThresholdPercent(decimal amount, Percent percent)
        {
            Amount = amount;
            Percent = percent;
        }

        public decimal Amount { get; }
        public Percent Percent { get; }
    }
}
