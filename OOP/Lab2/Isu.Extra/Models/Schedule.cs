using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models
{
    public class Schedule
    {
        private List<Lecture> _lectures;
        private Schedule(List<Lecture> lectures)
        {
            _lectures = lectures;
        }

        public static ScheduleBuilder Builder { get => new ScheduleBuilder(); }

        public IReadOnlyList<Lecture> Lectures => _lectures;

        public bool HasIntersection(Schedule schedule)
        {
            return schedule.Lectures.Any(
                x => _lectures.Any(
                    y => y.Time.HasIntersection(x.Time)));
        }

        public Schedule Merge(Schedule other)
        {
            if (HasIntersection(other))
                throw new ScheduleIntersectionException("Schedules have intersection");

            return new Schedule(_lectures.Concat(other._lectures).ToList());
        }

        public class ScheduleBuilder
        {
            private List<Lecture> _lectures;
            public ScheduleBuilder()
            {
                _lectures = new List<Lecture>();
            }

            public ScheduleBuilder AddLecture(LectureTime lectureTime, string subject, string teacher, string classroom)
            {
                _lectures.Add(new Lecture(lectureTime, subject, new Teacher(teacher), classroom));
                return this;
            }

            public Schedule Build()
            {
                foreach (Lecture l in _lectures)
                {
                    if (_lectures.Any(x => x.Time.HasIntersection(l.Time) && x != l))
                        throw new ScheduleIntersectionException("Schedule has intersection");
                }

                return new Schedule(_lectures);
            }
        }
    }
}
