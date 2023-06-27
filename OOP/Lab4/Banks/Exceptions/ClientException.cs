namespace Banks.Exceptions
{
    public class ClientException : BankException
    {
        public ClientException(string message)
            : base(message) { }
    }
}
