using Isu.Exceptions;

namespace Isu.Models
{
    public class Degree
    {
        public Degree(string name)
        {
            char degree = name.ElementAt(1);
            if (degree == '3')
            {
                Name = "Bachelor's";
            }
            else if (degree == '4')
            {
                Name = "Master's";
            }
            else if (degree == '5')
            {
                Name = "Specialist";
            }
            else
            {
                throw new InvalidGroupNameException(name);
            }
        }

        public string Name { get; }

        public override string ToString() => $"{Name} degree";
    }
}
