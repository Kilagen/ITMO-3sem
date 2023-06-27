using Banks.Console.Exceptions;
using Banks.Models;

namespace Banks.Console.ClockHandlers
{
    public class ClockHandler : IClockHandler
    {
        private IClockHandler? next;
        public Clock Handle(string[] args)
        {
            if (args.Length < 2 || args[0] != "clock")
            {
                if (next is null)
                {
                    throw new NoHandlerException();
                }

                next.Handle(args);
            }

            if (args[1] == "default")
            {
                return new Clock(DateTime.Today);
            }

            return new Clock(DateTime.Parse(args[1]));
        }

        public IClockHandler SetNext(IClockHandler handler)
        {
            next = handler;
            return next;
        }
    }
}
