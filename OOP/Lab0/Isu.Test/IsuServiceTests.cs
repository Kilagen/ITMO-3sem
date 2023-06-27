using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var isu = new IsuService();
        Group myGroup = isu.AddGroup(new GroupName("M32011"));
        Student maks1 = isu.AddStudent(myGroup, "Maksim Semyonov");
        Student maks2 = isu.AddStudent(myGroup, "Maksim Chebotarev");
        Student me = isu.AddStudent(myGroup, "Kirill Zakharov");

        Group otherGroup = isu.AddGroup(new GroupName("M3101"));
        Student otherMe = isu.AddStudent(otherGroup, "Kirill Zakharov");

        Assert.NotEqual(me, otherMe);

        foreach (Student student in new Student[] { me, maks1, maks2 })
        {
            Assert.Equal(student.Group, myGroup);
            Assert.True(myGroup.HasStudent(student));
            Assert.False(otherGroup.HasStudent(student));
            Assert.Equal(student, isu.GetStudent(student.Id));
        }
    }

    [Theory]
    [InlineData(26)]
    [InlineData(2)]
    [InlineData(1)]
    public void ReachMaxStudentPerGroup_ThrowException(int groupSize)
    {
        var isu = new IsuService();
        Group group = isu.AddGroup(new GroupName("M32011"), groupSize);
        for (char student = 'B'; student < 'B' + groupSize; student++)
        {
            isu.AddStudent(group, student.ToString());
        }

        Assert.Throws<GroupLimitException>(() => isu.AddStudent(group, "A"));
    }

    [Theory]
    [InlineData("12")]
    [InlineData("!@@HI")]
    [InlineData("M310")]
    [InlineData("ZXC")]
    public void CreateGroupWithInvalidName_ThrowException(string invalidGroupName)
    {
        var isu = new IsuService();
        Assert.Throws<InvalidGroupNameException>(() => isu.AddGroup(new GroupName(invalidGroupName)));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var isu = new IsuService();
        foreach (string name in new[] { "M32001", "M32011", "M32021", "M32031" })
        {
            isu.AddGroup(new GroupName(name));
        }

        List<Group> groups = isu.FindGroups(new CourseNumber("M32011"));
        Student albert = isu.AddStudent(groups[0], "Miheev Albert");
        foreach (Group group in groups)
        {
            Assert.Equal(TransferStudentAndReturnStudent(isu, albert, group).Group, group);
        }
    }

    private Student TransferStudentAndReturnStudent(IsuService isu, Student student, Group group)
    {
        isu.ChangeStudentGroup(student, group);
        return student;
    }
}