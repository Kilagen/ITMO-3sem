namespace Banks.Exceptions
{
    public class AccountConfigException : BankException
    {
        public AccountConfigException(string message)
            : base(message) { }
    }
}
