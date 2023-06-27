using Banks.Interfaces;

namespace Banks.Models
{
    public class PercentageStrategy : IPercentageStrategy
    {
        private PercentageStrategy(List<ThresholdPercent> thresholds)
        {
            Thresholds = thresholds;
        }

        public List<ThresholdPercent> Thresholds { get; }

        public Percent GetPercent(decimal amount)
        {
            return Thresholds.First(th => th.Amount <= amount).Percent;
        }

        public class Builder
        {
            private List<ThresholdPercent> thresholds;
            public Builder(Percent defaultPercent)
            {
                thresholds = new List<ThresholdPercent>();
                thresholds.Add(new ThresholdPercent(decimal.MinValue, defaultPercent));
            }

            public Builder WithThresholdPercent(decimal amount, Percent percent)
            {
                if (thresholds.Any(th => th.Amount == amount))
                    throw new ArgumentException($"Threshold with amount {amount} already exists");

                thresholds.Add(new ThresholdPercent(amount, percent));
                return this;
            }

            public PercentageStrategy Build()
            {
                return new PercentageStrategy(thresholds.OrderBy(th => th.Amount).ToList());
            }
        }
    }
}
