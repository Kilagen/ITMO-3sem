namespace Isu.Exceptions
{
    public class InvalidStudentIdException : IsuException
    {
        public InvalidStudentIdException() { }

        public InvalidStudentIdException(int id)
            : base($"No student with id {id:000000}") { }
    }
}
