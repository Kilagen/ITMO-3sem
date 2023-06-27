namespace Banks.Console.Exceptions
{
    public class NoHandlerException : BankConsoleException
    {
        public NoHandlerException(string message)
            : base(message) { }

        public NoHandlerException()
            : base("Unsupported command") { }
    }
}
