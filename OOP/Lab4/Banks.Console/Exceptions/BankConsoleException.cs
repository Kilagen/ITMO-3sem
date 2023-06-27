namespace Banks.Console.Exceptions
{
    public class BankConsoleException : Exception
    {
        public BankConsoleException(string message)
            : base(message) { }

        public BankConsoleException()
            : base() { }
    }
}
