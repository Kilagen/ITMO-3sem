using Banks.Interfaces;

namespace Banks.Models
{
    public class Clock : IClock
    {
        public Clock(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public event Action? Skip;
        public DateTime DateTime { get; private set; }

        public void Tick()
        {
            DateTime = DateTime.AddDays(1);
            Skip?.Invoke();
        }
    }
}
