using Isu.Exceptions;

namespace Isu.Extra.Exceptions
{
    public class IsuExtraException : IsuException
    {
        public IsuExtraException(string message)
            : base(message) { }

        public IsuExtraException()
            : base() { }
    }
}
