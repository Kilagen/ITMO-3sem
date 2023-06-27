namespace Shops.Exceptions
{
    public class InvalidBalanceException
        : ShopException
    {
        public InvalidBalanceException() { }

        public InvalidBalanceException(decimal balance)
            : base($"{balance} is less than zero") { }

        public InvalidBalanceException(decimal balance, decimal amount)
            : base($"{balance} - {amount} is less than zero") { }
    }
}
