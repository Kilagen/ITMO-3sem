using Banks.Exceptions;

namespace Banks.Models.Configs
{
    public class DepositAccountConfiguration
    {
        public DepositAccountConfiguration(TimeSpan minPeriod, PercentageStrategy percentageModel, decimal unsafeLimit)
        {
            if (minPeriod <= TimeSpan.FromDays(0))
                throw new AccountConfigException("Deposit period cannot be negative or zero");
            if (unsafeLimit <= 0)
                throw new AccountConfigException("Limit for unsafe accounts should be positive");
            Period = minPeriod;
            UnsafeLimit = unsafeLimit;
            PercentageModel = percentageModel;
        }

        public TimeSpan Period { get; }
        public decimal UnsafeLimit { get; }
        public PercentageStrategy PercentageModel { get; }
    }
}
