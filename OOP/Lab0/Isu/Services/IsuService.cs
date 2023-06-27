using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly HashSet<Student> _students;
    private readonly HashSet<Group> _groups;
    private IsuNumber _idGenerator;

    public IsuService()
        : this(new HashSet<Group>(), new HashSet<Student>()) { }

    public IsuService(IEnumerable<Group> groups, IEnumerable<Student> students)
    {
        _students = students.ToHashSet();
        _groups = groups.ToHashSet();
        _idGenerator = new IsuNumber(students.Aggregate(-1, (maxId, stud) => maxId > stud.Id ? maxId : stud.Id));
    }

    public Group AddGroup(GroupName name)
    {
        return AddGroup(name, 30);
    }

    public Group AddGroup(GroupName name, int limit)
    {
        if (_groups.Any(gr => gr.Name.Equals(name)))
        {
            throw new GroupAlreadyExistsException(name);
        }

        var group = new Group(name, limit);
        _groups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        if (!_groups.Contains(group))
        {
            _groups.Add(group);
        }

        var student = new Student(_idGenerator.NextId(), name, group);
        _students.Add(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        Student? student = FindStudent(id);
        if (student == null)
        {
            throw new InvalidStudentIdException(id);
        }

        return student;
    }

    public Student? FindStudent(int id) => _students.ToList().Find(student => student.Id.Equals(id));

    public List<Student> FindStudents(GroupName groupName) => _students
        .Where(student => student.Group.Name.Equals(groupName)).ToList();

    public List<Student> FindStudents(CourseNumber courseNumber) => _students
        .Where(student => student.Group.Name.CourseNumber.Equals(courseNumber)).ToList();

    public Group? FindGroup(GroupName groupName) => _groups.ToList().Find(g => g.Name.Equals(groupName));

    public List<Group> FindGroups(CourseNumber courseNumber) => _groups.Where(g => g.Name.CourseNumber.Equals(courseNumber)).ToList();

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        student.ChangeGroup(newGroup);
    }
}