using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    private Group _group;

    public Student(int id, string name, Group group)
    {
        Id = id;
        Name = name;
        _group = group;
        group.Add(this);
    }

    public int Id { get; }

    public Group Group
    {
        get => _group;

        private set { }
    }

    public string Name { get; }

    public override bool Equals(object? obj)
    {
        return (obj is not null)
            && (obj.GetType() == GetType())
            && Id == ((Student)obj).Id;
    }

    public override int GetHashCode() => base.GetHashCode();

    public void ChangeGroup(Group newGroup)
    {
        if (newGroup is null)
        {
            throw new ArgumentNullException(nameof(newGroup));
        }

        if (_group.Equals(newGroup))
        {
            return;
        }

        if (newGroup.IsFull())
        {
            throw new GroupLimitException(Group);
        }

        _group.Remove(this);
        newGroup.Add(this);
        _group = newGroup;
    }
}