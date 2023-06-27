using Banks.Exceptions;

namespace Banks.Models.Configs
{
    public class DebitAccountConfiguration
    {
        public DebitAccountConfiguration(Percent percent, decimal unsafeLimit)
        {
            if (unsafeLimit <= 0)
                throw new AccountConfigException("Limit for unsafe accounts should be positive");
            Percent = percent;
            UnsafeLimit = unsafeLimit;
        }

        public Percent Percent { get; }
        public decimal UnsafeLimit { get; }
    }
}
