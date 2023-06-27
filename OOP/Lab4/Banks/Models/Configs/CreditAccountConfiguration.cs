using Banks.Exceptions;

namespace Banks.Models.Configs
{
    public class CreditAccountConfiguration
    {
        public CreditAccountConfiguration(decimal comission, decimal unsafeLimit)
        {
            if (comission <= 0)
                throw new AccountConfigException("Credit account commision should be positive");
            if (unsafeLimit <= 0)
                throw new AccountConfigException("Limit for unsafe accounts should be positive");
            Comission = comission;
            UnsafeLimit = unsafeLimit;
        }

        public decimal Comission { get; }
        public decimal UnsafeLimit { get; }
    }
}
