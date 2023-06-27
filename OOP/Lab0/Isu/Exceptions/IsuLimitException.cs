namespace Isu.Exceptions
{
    public class IsuLimitException : IsuException
    {
        public IsuLimitException() { }

        public IsuLimitException(string msg)
            : base($"{msg} reached it's limit") { }
    }
}
