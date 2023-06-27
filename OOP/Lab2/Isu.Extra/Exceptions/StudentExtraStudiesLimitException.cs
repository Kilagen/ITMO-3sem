namespace Isu.Extra.Exceptions
{
    public class StudentExtraStudiesLimitException : IsuExtraException
    {
        public StudentExtraStudiesLimitException(string message)
            : base(message) { }

        public StudentExtraStudiesLimitException()
            : base() { }
    }
}
