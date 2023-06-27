using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    public CourseNumber(string group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        Course = group.ElementAt(2);
        if (Course > '5' | Course < '1')
        {
            throw new InvalidGroupNameException(group);
        }
    }

    private char Course { get; }

    public override bool Equals(object? obj)
    {
        return (obj != null)
            && (obj.GetType() == GetType())
            && Course.Equals(((CourseNumber)obj).Course);
    }

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => Course.ToString();
}