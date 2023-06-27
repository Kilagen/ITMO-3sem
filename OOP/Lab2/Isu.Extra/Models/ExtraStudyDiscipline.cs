using Isu.Extra.Entities;

namespace Isu.Extra.Models
{
    public class ExtraStudyDiscipline
    {
        private List<ExtraStudyStream> _streams;
        public ExtraStudyDiscipline(string name, MegaFaculty megaFaculty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Discipline's name Can't be blank", nameof(name));

            _streams = new List<ExtraStudyStream>();
            Name = name;
            MegaFaculty = megaFaculty;
        }

        public string Name { get; }
        public MegaFaculty MegaFaculty { get; }
        public IReadOnlyList<ExtraStudyStream> Streams => _streams;

        public ExtraStudyStream AddStream(string streamName, int limit = 40)
        {
            var stream = new ExtraStudyStream(streamName, this, limit);
            _streams.Add(stream);
            return stream;
        }
    }
}
