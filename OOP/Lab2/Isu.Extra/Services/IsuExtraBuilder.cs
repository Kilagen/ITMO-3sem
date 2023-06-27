namespace Isu.Extra.Services
{
    public class IsuExtraBuilder
    {
        private Dictionary<string, List<string>> _megaFaculties;
        private int? _extraStudyPerStudent;
        public IsuExtraBuilder()
        {
            _megaFaculties = new Dictionary<string, List<string>>();
        }

        public IsuExtraBuilder WithMegaFaculty(string name, string beginLetters)
        {
            _megaFaculties.Add(name, beginLetters.Select(x => x.ToString()).ToList());
            return this;
        }

        public IsuExtraBuilder WithExtraStudiesPerStudent(int extraStudiesPerStudent)
        {
            _extraStudyPerStudent = extraStudiesPerStudent;
            return this;
        }

        public IsuServiceExtra Build()
        {
            if (_extraStudyPerStudent is null)
                throw new Exception("Extra studies per student is not set");

            if (_megaFaculties.Count == 0)
                throw new Exception("Mega faculties are not set");

            if (_megaFaculties.SelectMany(x => x.Value).Distinct().Count() != _megaFaculties.SelectMany(x => x.Value).Count())
                throw new Exception("Mega faculties have same faculties");

            var service = new IsuServiceExtra((int)_extraStudyPerStudent);
            foreach (KeyValuePair<string, List<string>> megaFaculty in _megaFaculties)
            {
                service.AddMegaFaculty(megaFaculty.Key, megaFaculty.Value);
            }

            return service;
        }
    }
}
