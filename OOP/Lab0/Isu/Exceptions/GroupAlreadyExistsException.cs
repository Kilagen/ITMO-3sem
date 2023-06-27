using Isu.Models;

namespace Isu.Exceptions
{
    public class GroupAlreadyExistsException : IsuException
    {
        public GroupAlreadyExistsException() { }

        public GroupAlreadyExistsException(GroupName group)
        : base($"{group} is already presented in IsuService") { }
    }
}
