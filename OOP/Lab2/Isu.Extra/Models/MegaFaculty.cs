using Isu.Models;

namespace Isu.Extra.Models
{
    public class MegaFaculty
    {
        private List<FacultyName> _faculties;
        public MegaFaculty(string name, List<string> beginLetters)
        {
            Name = name;
            _faculties = beginLetters.Select(x => new FacultyName(x)).ToList();
        }

        public string Name { get; }
        public IReadOnlyList<FacultyName> Faculties => _faculties;

        public bool Contains(FacultyName facultyName)
        {
            return _faculties.Contains(facultyName);
        }
    }
}
