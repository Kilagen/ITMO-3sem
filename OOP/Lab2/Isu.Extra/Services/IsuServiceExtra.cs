using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services
{
    public class IsuServiceExtra : IIsuServiceExtra, IIsuService
    {
        private Dictionary<Student, ExtraStudent> _extraStudents;
        private List<ExtraStudyDiscipline> _extraStudies;
        private Dictionary<Group, ExtraGroup> _groups;
        private List<MegaFaculty> _megaFaculties;
        private Isu.Services.IIsuService _oldIsuService;
        private int _extraStudiesPerStream;

        public IsuServiceExtra(int extraStudiesPerStream = 2)
        {
            _extraStudents = new Dictionary<Student, ExtraStudent>();
            _extraStudies = new List<ExtraStudyDiscipline>();
            _groups = new Dictionary<Group, ExtraGroup>();
            _megaFaculties = new List<MegaFaculty>();
            _oldIsuService = new Isu.Services.IsuService();
            _extraStudiesPerStream = extraStudiesPerStream;
        }

        public ExtraStudyDiscipline AddExtraStudyDiscipline(string name, MegaFaculty faculty)
        {
            if (_extraStudies.Any(x => x.Name == name))
                throw new IsuObjectsCollisionException("Extra study with this name already exists");

            var extraStudy = new ExtraStudyDiscipline(name, faculty);
            _extraStudies.Add(extraStudy);
            return extraStudy;
        }

        public ExtraStudyStream AddExtraStudyStream(string name, ExtraStudyDiscipline discipline, int limit = 40)
        {
            return discipline.AddStream(name, limit);
        }

        public MegaFaculty AddMegaFaculty(string name, List<string> beginLetters)
        {
            if (_megaFaculties.Any(x => x.Name == name))
                throw new IsuObjectsCollisionException("Mega faculty with this name already exists");

            var megaFaculty = new MegaFaculty(name, beginLetters);
            if (_megaFaculties.Any(x => x.Faculties.Any(y => megaFaculty.Faculties.Contains(y))))
                throw new IsuObjectsCollisionException("Mega faculty with this faculties already exists");

            _megaFaculties.Add(megaFaculty);
            return megaFaculty;
        }

        public Group AddGroup(GroupName name)
        {
            MegaFaculty? megaFaculty = _megaFaculties.FirstOrDefault(x => x.Contains(name.Faculty));
            if (megaFaculty is null)
                throw new NoSuitableMegaFacultyException(name);

            Group group = _oldIsuService.AddGroup(name);
            var extraGroup = new ExtraGroup(group, megaFaculty);
            _groups.Add(group, extraGroup);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            ExtraGroup extraGroup = _groups[group];
            Student student = _oldIsuService.AddStudent(group, name);
            var extraStudent = new ExtraStudent(student, extraGroup, _extraStudiesPerStream);
            _extraStudents.Add(student, extraStudent);
            return student;
        }

        public void AddStudent(ExtraStudyStream extraStudyStream, Student student)
        {
            _extraStudents[student].AddExtraStudyStream(extraStudyStream);
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            ExtraGroup extraGroup = _groups[newGroup];
            ExtraStudent extraStudent = _extraStudents[student];
            if (extraStudent.ExtraStudyStreams.Any(x => x.StreamSchedule.HasIntersection(extraGroup.GroupSchedule)))
                throw new ScheduleIntersectionException("New group schedule intersects with Extra Studies");

            _oldIsuService.ChangeStudentGroup(student, newGroup);
            _extraStudents[student] = new ExtraStudent(student, extraGroup, _extraStudiesPerStream);
        }

        public Group? FindGroup(GroupName groupName) => _oldIsuService.FindGroup(groupName);

        public List<Group> FindGroups(CourseNumber courseNumber) => _oldIsuService.FindGroups(courseNumber);

        public Student? FindStudent(int id) => _oldIsuService.FindStudent(id);

        public List<Student> FindStudents(GroupName groupName) => _oldIsuService.FindStudents(groupName);

        public List<Student> FindStudents(CourseNumber courseNumber) => _oldIsuService.FindStudents(courseNumber);

        public IReadOnlyList<Student> GetExtraStudyGroupStudents(ExtraStudyStream extraStudy)
        {
            return extraStudy.Students.Select(x => x.Student).ToList();
        }

        public IReadOnlyList<ExtraStudyStream> GetExtraStudyStreams(ExtraStudyDiscipline extraStudy)
        {
            return extraStudy.Streams;
        }

        public MegaFaculty GetMegaFaculty(string name)
        {
            MegaFaculty? mf = _megaFaculties.FirstOrDefault(x => x.Name == name);
            if (mf is null)
                throw new IsuObjectsCollisionException("Mega faculty with this name doesn't exist");

            return mf;
        }

        public IReadOnlyList<Student> GetNoExtraStudyStudents(Group group)
        {
            return group.StudentsView.Select(x => _extraStudents[x]).
                Where(x => x.ExtraStudyStreams.Count == 0).Select(x => x.Student).ToList();
        }

        public Student GetStudent(int id) => _oldIsuService.GetStudent(id);

        public void RemoveStudent(ExtraStudyStream extraStudyStream, Student student)
        {
            ExtraStudent extraStudent = _extraStudents[student];
            extraStudent.RemoveExtraStudyStream(extraStudyStream);
        }

        public Schedule GetSchedule(Group group)
        {
            return _groups[group].GroupSchedule;
        }

        public Schedule GetSchedule(Student student) => _extraStudents[student].GetSchedule();

        public Schedule GetSchedule(ExtraStudyStream extraStudyStream) => extraStudyStream.StreamSchedule;

        public void SetSchedule(Group group, Schedule schedule)
        {
            ExtraGroup extraGroup = _groups[group];
            foreach (Student st in extraGroup.OldGroup.StudentsView)
            {
                if (_extraStudents[st].ExtraStudyStreams.Any(x => x.StreamSchedule.HasIntersection(schedule)))
                    throw new ScheduleIntersectionException("New Group Schedule has intersections");
            }

            extraGroup.GroupSchedule = schedule;
        }

        public void SetSchedule(ExtraStudyStream extraStudyStream, Schedule schedule)
        {
            if (extraStudyStream.Students.Any(x => x.GetSchedule().HasIntersection(schedule)))
                throw new ScheduleIntersectionException("New Stream Schedule has intersections");

            extraStudyStream.SetSchedule(schedule);
        }
    }
}