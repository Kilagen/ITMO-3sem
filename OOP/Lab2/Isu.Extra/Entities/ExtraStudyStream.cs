using Isu.Exceptions;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities
{
    public class ExtraStudyStream
    {
        private List<ExtraStudent> _students;
        public ExtraStudyStream(string streamName, ExtraStudyDiscipline discipline, int limit = 40)
        {
            if (string.IsNullOrWhiteSpace(streamName))
                throw new ArgumentException("Empty or Null stream name", nameof(streamName));

            if (limit <= 0)
                throw new IsuLimitException("Extra Studies group limit can't be less than 1");

            Name = streamName;
            Discipline = discipline;
            Limit = limit;
            _students = new List<ExtraStudent>();
            StreamSchedule = Schedule.Builder.Build();
        }

        public string Name { get; }
        public int Limit { get; }
        public ExtraStudyDiscipline Discipline { get; }
        public IReadOnlyList<ExtraStudent> Students { get => _students; }
        public Schedule StreamSchedule { get; private set; }

        public bool IsFull() => _students.Count == Limit;

        public void AddStudent(ExtraStudent student)
        {
            if (_students.Contains(student))
                throw new IsuObjectsCollisionException("Student is already in Extra Study Stream");

            if (_students.Count == Limit)
                throw new IsuLimitException("Extra Studies group is full");

            _students.Add(student);
        }

        public void RemoveStudent(ExtraStudent student)
        {
            if (!_students.Contains(student))
                throw new IsuObjectsCollisionException("Student is is not in Extra Study Stream");

            _students.Remove(student);
        }

        public void SetSchedule(Schedule schedule)
        {
            if (schedule == StreamSchedule)
                throw new IsuObjectsCollisionException("This Schedule is already set");

            if (_students.Any(st => st.GetSchedule().HasIntersection(schedule)))
                throw new ScheduleIntersectionException("New schedule intersects with student schedule");

            StreamSchedule = schedule;
        }
    }
}
