namespace Shops.Exceptions
{
    public class ShopException : Exception
    {
        public ShopException() { }
        public ShopException(string message)
            : base(message) { }
    }
}
