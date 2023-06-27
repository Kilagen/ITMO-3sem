namespace Banks.Exceptions
{
    public class TransactionException : BankException
    {
        public TransactionException(string message)
            : base(message) { }
    }
}
