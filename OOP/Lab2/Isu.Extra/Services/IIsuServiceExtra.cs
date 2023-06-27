using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Services
{
    public interface IIsuServiceExtra
    {
        public ExtraStudyDiscipline AddExtraStudyDiscipline(string name, MegaFaculty faculty);
        public void AddStudent(ExtraStudyStream extraStudyStream, Student student);
        public void RemoveStudent(ExtraStudyStream extraStudyStream, Student student);
        public IReadOnlyList<ExtraStudyStream> GetExtraStudyStreams(ExtraStudyDiscipline extraStudy);
        public IReadOnlyList<Student> GetExtraStudyGroupStudents(ExtraStudyStream extraStudy);
        public IReadOnlyList<Student> GetNoExtraStudyStudents(Group group);
    }
}
