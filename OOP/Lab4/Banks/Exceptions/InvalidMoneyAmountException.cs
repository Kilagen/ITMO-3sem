namespace Banks.Exceptions
{
    public class InvalidMoneyAmountException : BankException
    {
        public InvalidMoneyAmountException(string message)
            : base(message) { }
    }
}
