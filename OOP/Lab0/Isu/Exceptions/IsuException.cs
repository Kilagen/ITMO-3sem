namespace Isu.Exceptions
{
    public class IsuException : Exception
    {
        public IsuException() { }

        public IsuException(string msg)
            : base(msg) { }
    }
}
