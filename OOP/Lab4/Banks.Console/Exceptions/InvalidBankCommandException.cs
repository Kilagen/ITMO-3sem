namespace Banks.Console.Exceptions
{
    public class InvalidBankCommandException : BankConsoleException
    {
        public InvalidBankCommandException(string message)
            : base(message) { }
    }
}
