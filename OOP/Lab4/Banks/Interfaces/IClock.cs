namespace Banks.Interfaces
{
    public interface IClock
    {
        public event Action? Skip;
        public DateTime DateTime { get; }

        public void Tick();
    }
}
