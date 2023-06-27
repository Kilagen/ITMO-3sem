namespace Shops.Exceptions
{
    public class CartException : ShopException
    {
        public CartException() { }
        public CartException(string message)
            : base(message) { }
    }
}
