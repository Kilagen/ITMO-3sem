namespace Shops.Exceptions
{
    public class StorageException : ShopException
    {
        public StorageException() { }

        public StorageException(string message)
            : base(message) { }
    }
}
