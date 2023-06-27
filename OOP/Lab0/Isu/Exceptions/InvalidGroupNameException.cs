namespace Isu.Exceptions
{
    public class InvalidGroupNameException : IsuException
    {
        public InvalidGroupNameException() { }

        public InvalidGroupNameException(string name)
            : base($"{name} is invalid group name") { }
    }
}
