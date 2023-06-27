using Isu.Extra.Exceptions;

namespace Isu.Extra.Models
{
    public class LectureTime
    {
        public LectureTime(Weekday dayOfWeek, int weekNumber, int beginTime, int endTime)
        {
            if (beginTime >= endTime)
            {
                throw new InvalidLectureTimeException("Lecture begin time must be less than end time");
            }

            if (beginTime < 0 || beginTime > 1440)
            {
                throw new InvalidLectureTimeException(nameof(beginTime));
            }

            if (endTime < 0 || endTime > 1440)
            {
                throw new InvalidLectureTimeException(nameof(endTime));
            }

            if (weekNumber < 1)
            {
                throw new ArgumentException("Week number must be positive");
            }

            DayOfWeek = dayOfWeek;
            WeekNumber = weekNumber;
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public Weekday DayOfWeek { get; }
        public int WeekNumber { get; }
        public int BeginTime { get; }
        public int EndTime { get; }

        public static Dictionary<Weekday, List<LectureTime>> GenerateWeekLectureTimes(int weekNumber)
        {
            var result = new Dictionary<Weekday, List<LectureTime>>();
            foreach (Weekday weekday in new[]
            {
                Weekday.Monday,
                Weekday.Tuesday,
                Weekday.Wednesday,
                Weekday.Thursday,
                Weekday.Friday,
                Weekday.Saturday,
                Weekday.Sunday,
            })
            {
                result[weekday] = new List<LectureTime>();
                result[weekday].Add(new LectureTime(weekday, weekNumber, (8 * 60) + 20, (9 * 60) + 50));
                result[weekday].Add(new LectureTime(weekday, weekNumber, (10 * 60) + 10, (11 * 60) + 40));
                result[weekday].Add(new LectureTime(weekday, weekNumber, (11 * 60) + 50, (13 * 60) + 10));
                result[weekday].Add(new LectureTime(weekday, weekNumber, (13 * 60) + 30, (15 * 60) + 0));
                result[weekday].Add(new LectureTime(weekday, weekNumber, (15 * 60) + 20, (16 * 60) + 50));
                result[weekday].Add(new LectureTime(weekday, weekNumber, (17 * 60) + 0, (18 * 60) + 30));
            }

            return result;
        }

        public bool HasIntersection(LectureTime other)
        {
            if (DayOfWeek != other.DayOfWeek || WeekNumber != other.WeekNumber)
            {
                return false;
            }

            return (BeginTime <= other.BeginTime && other.BeginTime <= EndTime) ||
                (BeginTime <= other.EndTime && other.EndTime <= EndTime);
        }
    }
}
