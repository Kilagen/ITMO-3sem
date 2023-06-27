namespace Isu.Extra.Exceptions
{
    public class InvalidLectureTimeException : IsuExtraException
    {
        public InvalidLectureTimeException()
            : base() { }

        public InvalidLectureTimeException(string message)
            : base(message) { }
    }
}
