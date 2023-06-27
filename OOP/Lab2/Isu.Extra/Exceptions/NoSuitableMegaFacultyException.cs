using Isu.Models;

namespace Isu.Extra.Exceptions
{
    public class NoSuitableMegaFacultyException : IsuExtraException
    {
        public NoSuitableMegaFacultyException(GroupName group)
            : base($"No suitable mega faculty for {group}") { }

        public NoSuitableMegaFacultyException(string message)
            : base(message) { }
    }
}
