using System.Text.RegularExpressions;
using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private static Regex validator = new Regex("^[A-Z]((3[1-5])|(4[1-2])|(5[1-6]))[0-9][0-9][1-9]?c?$", RegexOptions.Compiled);

    // Only accepts bachelor's, specialist and master's degree groups
    public GroupName(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (!IsNameValid(name))
        {
            throw new InvalidGroupNameException(name);
        }

        CourseNumber = new CourseNumber(name);
        Faculty = new FacultyName(name);
        Degree = new Degree(name);
        Name = name;
    }

    public CourseNumber CourseNumber { get; }
    public FacultyName Faculty { get; }
    public Degree Degree { get; }
    private string Name { get; }

    public static bool IsNameValid(string name) => validator.IsMatch(name);

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        return (obj != null)
            && (obj.GetType() == GetType())
            && Name.Equals(((GroupName)obj).Name);
    }

    public override int GetHashCode() => Name.GetHashCode();
}