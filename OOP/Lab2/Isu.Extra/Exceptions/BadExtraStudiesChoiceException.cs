namespace Isu.Extra.Exceptions
{
    public class BadExtraStudiesChoiceException : IsuExtraException
    {
        public BadExtraStudiesChoiceException(string message)
            : base(message) { }

        public BadExtraStudiesChoiceException()
            : base("Student can't pick his faculty extra studies") { }
    }
}
