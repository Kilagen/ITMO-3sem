namespace Banks.Models.Configs
{
    public class BankConfiguration
    {
        public BankConfiguration(
            CreditAccountConfiguration creditAccountConfiguration,
            DebitAccountConfiguration debitAccountConfiguration,
            DepositAccountConfiguration depositAccountConfiguration)
        {
            CreditAccount = creditAccountConfiguration;
            DebitAccount = debitAccountConfiguration;
            DepositAccount = depositAccountConfiguration;
        }

        public CreditAccountConfiguration CreditAccount { get; }
        public DebitAccountConfiguration DebitAccount { get; }
        public DepositAccountConfiguration DepositAccount { get; }
    }
}
