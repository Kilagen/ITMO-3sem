using Banks.Models;

namespace Banks.Console.ClockHandlers
{
    public interface IClockHandler
    {
        public IClockHandler SetNext(IClockHandler handler);
        public Clock Handle(string[] args);
    }
}
