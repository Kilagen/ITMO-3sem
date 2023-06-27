using Isu.Exceptions;

namespace Isu.Models
{
    public class IsuNumber
    {
        private int _id;
        public IsuNumber(int id = 99999)
        {
            _id = id++;
        }

        public int NextId()
        {
            if (_id == 999999)
            {
                throw new IsuLimitException("id");
            }

            return _id++;
        }
    }
}
