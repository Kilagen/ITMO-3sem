using Isu.Entities;

namespace Isu.Exceptions
{
    public class GroupLimitException : IsuLimitException
    {
        public GroupLimitException() { }
        public GroupLimitException(Group group)
            : base($"{group.Name} reached it's limit") { }
        public GroupLimitException(int limit, int expectedLimit)
            : base($"{limit} is too small. Expected at least {expectedLimit}") { }
    }
}
