using Isu.Extra.Entities;

namespace Isu.Extra.Models
{
    public class Lecture
    {
        public Lecture(LectureTime lectureTime, string subject, Teacher teacher, string classroom)
        {
            Time = lectureTime;
            Subject = subject;
            Teacher = teacher;
            Classroom = classroom;
        }

        public string Subject { get; init; }
        public Teacher Teacher { get; init; }
        public string Classroom { get; init; }
        public LectureTime Time { get; init; }
    }
}
