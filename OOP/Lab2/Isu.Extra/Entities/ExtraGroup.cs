using Isu.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Entities
{
    public class ExtraGroup
    {
        public ExtraGroup(Group group, MegaFaculty megaFaculty, Schedule schedule)
        {
            OldGroup = group;
            MegaFaculty = megaFaculty;
            GroupSchedule = schedule;
        }

        public ExtraGroup(Group group, MegaFaculty megaFaculty)
            : this(group, megaFaculty, Schedule.Builder.Build()) { }

        public Group OldGroup { get; private set; }
        public Schedule GroupSchedule { get; set; }
        public MegaFaculty MegaFaculty { get; private set; }
    }
}
