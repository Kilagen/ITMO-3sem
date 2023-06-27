namespace Isu.Extra.Entities
{
    public class Teacher
    {
        public Teacher(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Teacher's name Can't be blank", nameof(name));

            Name = name;
        }

        public string Name { get; }
    }
}
