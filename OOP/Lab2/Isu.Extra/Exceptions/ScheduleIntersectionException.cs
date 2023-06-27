namespace Isu.Extra.Exceptions
{
    public class ScheduleIntersectionException : IsuExtraException
    {
        public ScheduleIntersectionException(string message)
            : base(message) { }

        public ScheduleIntersectionException()
            : base() { }
    }
}
