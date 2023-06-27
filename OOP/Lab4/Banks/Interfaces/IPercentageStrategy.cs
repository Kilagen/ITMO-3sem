using Banks.Models;

namespace Banks.Interfaces
{
    public interface IPercentageStrategy
    {
        public Percent GetPercent(decimal amount);
    }
}
