using Isu.Exceptions;

namespace Isu.Models
{
    public class FacultyName
    {
        public FacultyName(string group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Fac = group.ElementAt(0);
            if (Fac > 'Z' | Fac < 'A')
            {
                throw new InvalidGroupNameException(nameof(group));
            }
        }

        private char Fac { get; }

        public override bool Equals(object? obj)
        {
            return (obj != null)
                && (obj.GetType() == GetType())
                && Fac.Equals(((FacultyName)obj).Fac);
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => Fac.ToString();
    }
}
