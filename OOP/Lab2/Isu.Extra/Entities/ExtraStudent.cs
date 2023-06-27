using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities
{
    public class ExtraStudent
    {
        private int _extraStudyStreamsCount;
        private List<ExtraStudyStream> _extraStudyStreams;
        public ExtraStudent(Student student, ExtraGroup group, int extraStudyStreamsCount)
        {
            if (extraStudyStreamsCount < 1)
                throw new IsuLimitException("Student can't pick his faculty extra studies");

            Student = student;
            Group = group;
            _extraStudyStreamsCount = extraStudyStreamsCount;
            _extraStudyStreams = new List<ExtraStudyStream>();
        }

        public Student Student { get; init; }
        public ExtraGroup Group { get; private set; }
        public IReadOnlyList<ExtraStudyStream> ExtraStudyStreams => _extraStudyStreams;

        public Schedule GetSchedule()
        {
            Schedule stSchedule = Group.GroupSchedule;
            foreach (ExtraStudyStream stream in _extraStudyStreams)
            {
                stSchedule = stSchedule.Merge(stream.StreamSchedule);
            }

            return stSchedule;
        }

        public void AddExtraStudyStream(ExtraStudyStream stream)
        {
            if (stream.Discipline.MegaFaculty.Equals(Group.MegaFaculty))
                throw new BadExtraStudiesChoiceException();

            if (_extraStudyStreams.Count == _extraStudyStreamsCount)
                throw new StudentExtraStudiesLimitException();

            if (stream.StreamSchedule.HasIntersection(Group.GroupSchedule))
                throw new ScheduleIntersectionException("Extra studies intersect base schedule");

            if (_extraStudyStreams.Any(x => x.StreamSchedule.HasIntersection(stream.StreamSchedule)))
                throw new ScheduleIntersectionException("Extra studies intersect other extra studies");

            if (!_extraStudyStreams.Contains(stream))
                stream.AddStudent(this);

            _extraStudyStreams.Add(stream);
        }

        public void RemoveExtraStudyStream(ExtraStudyStream stream)
        {
            if (!_extraStudyStreams.Contains(stream))
                throw new StudentExtraStudiesLimitException("Student don't have given extra studies stream");

            stream.RemoveStudent(this);
            _extraStudyStreams.Remove(stream);
        }

        public void ChangeGroup(ExtraGroup newGroup)
        {
            Student.ChangeGroup(newGroup.OldGroup);
            Group = newGroup;
        }

        public override bool Equals(object? obj)
        {
            return (obj is not null)
                && (obj.GetType() == GetType())
                && Student.Equals(((ExtraStudent)obj).Student);
        }

        public override int GetHashCode() => Student.GetHashCode();
    }
}
