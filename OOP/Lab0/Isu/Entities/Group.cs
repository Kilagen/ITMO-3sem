using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private readonly HashSet<Student> _students;

    public Group(GroupName name, int limit = 30)
        : this(new HashSet<Student>(), name, limit) { }

    public Group(IEnumerable<Student> students, GroupName name, int limit = 30)
    {
        if (limit < 0)
        {
            throw new GroupLimitException(limit, 0);
        }

        if (limit < students.Count())
        {
            throw new GroupLimitException(limit, students.Count());
        }

        _students = students.ToHashSet();
        Name = name;
        Limit = limit;
    }

    public GroupName Name { get; }

    public int Limit { get; }

    public List<Student> StudentsView
    {
        get => _students.ToList();
    }

    public override bool Equals(object? obj)
    {
        return (obj != null)
            && (obj.GetType() == GetType())
            && Name.Equals(((Group)obj).Name);
    }

    public override int GetHashCode() => base.GetHashCode();

    public bool HasStudent(Student student) => _students.Contains(student);

    public bool IsFull() => Limit == _students.Count;

    public bool Remove(Student student)
    {
        return _students.Remove(student);
    }

    public bool Add(Student student)
    {
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (IsFull())
        {
            throw new GroupLimitException(this);
        }

        return _students.Add(student);
    }
}