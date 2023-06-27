namespace Isu.Exceptions
{
    public class IdLimitException : IsuLimitException
    {
        public IdLimitException() { }

        public IdLimitException(string limit)
            : base($"{limit} reached it's limit") { }
    }
}
