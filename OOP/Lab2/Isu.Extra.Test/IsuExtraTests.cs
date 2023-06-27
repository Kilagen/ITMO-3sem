using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test
{
    public class IsuExtraTests
    {
        private IsuServiceExtra _isu;
        private Group _myGroup;
        private Group _notMyGroup;
        private ExtraStudyDiscipline _discipline;
        private ExtraStudyStream _stream;
        private Dictionary<Weekday, List<LectureTime>> _baseTimes;
        public IsuExtraTests()
        {
            var builder = new IsuExtraBuilder();
            builder.WithExtraStudiesPerStudent(2);
            _isu = builder.WithMegaFaculty("BiBiP", "MK")
                .WithMegaFaculty("PiPiP", "CD")
                .WithMegaFaculty("Lil Peep", "RIP")
                .Build();

            _baseTimes = LectureTime.GenerateWeekLectureTimes(1);

            _discipline = _isu.AddExtraStudyDiscipline("BiBiBOOP", _isu.GetMegaFaculty("BiBiP"));
            _isu.AddExtraStudyDiscipline("PiPiPOOP", _isu.GetMegaFaculty("PiPiP"));
            _myGroup = _isu.AddGroup(new GroupName("M32011"));
            _notMyGroup = _isu.AddGroup(new GroupName("R32021"));
            _stream = _isu.AddExtraStudyStream("PiPiPOOP-SS1", _discipline);

            foreach (string name in new[] { "Pudge", "Amogus", "Gigachad" })
            {
                _isu.AddStudent(_myGroup, name);
            }

            foreach (string name in new[] { "Gena Bukin", "Kuzya", "Lobanov" })
            {
                _isu.AddStudent(_notMyGroup, name);
            }
        }

        [Fact]
        public void ExtraStudyStreamAddStudentRemoveStudent()
        {
            Assert.Equal(_notMyGroup.StudentsView, _isu.GetNoExtraStudyStudents(_notMyGroup));

            List<Student> students = _notMyGroup.StudentsView;
            Student gena = students[0];
            _isu.AddStudent(_stream, gena);
            Assert.Equal(students.Where(st => st.Id != gena.Id), _isu.GetNoExtraStudyStudents(_notMyGroup));

            _isu.RemoveStudent(_stream, gena);
            Assert.Equal(_notMyGroup.StudentsView, _isu.GetNoExtraStudyStudents(_notMyGroup));
        }

        [Fact]
        public void StudentPicksHisExtraStudy_ThrowsException()
        {
            Student pudge = _myGroup.StudentsView[0];
            Assert.Throws<BadExtraStudiesChoiceException>(() => _isu.AddStudent(_stream, pudge));
        }

        [Fact]
        public void StudentPickExtraGroupWithIntersection_ThrowsException()
        {
            Schedule groupSchedule = Schedule.Builder
                .AddLecture(_baseTimes[Weekday.Monday][0], "Math", "Vozianova", "1213")
                .AddLecture(_baseTimes[Weekday.Monday][1], "Math", "Vozianova", "1213")
                .AddLecture(_baseTimes[Weekday.Monday][2], "Physics", "Muzychenko", "2313")
                .Build();
            Schedule extraSchedule = Schedule.Builder
                .AddLecture(_baseTimes[Weekday.Monday][0], "NotMath", "NotVozianova", "1214")
                .Build();
            Assert.True(extraSchedule.HasIntersection(groupSchedule));

            _isu.SetSchedule(_notMyGroup, groupSchedule);
            Assert.Equal(groupSchedule, _isu.GetSchedule(_notMyGroup));
            _isu.SetSchedule(_stream, extraSchedule);
            Assert.Equal(extraSchedule, _stream.StreamSchedule);
            Assert.Throws<ScheduleIntersectionException>(() => _isu.AddStudent(_stream, _notMyGroup.StudentsView[0]));
        }
    }
}
