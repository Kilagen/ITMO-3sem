namespace Banks.Interfaces
{
    public interface ITransaction
    {
        public bool IsReverted { get; }
        public decimal Amount { get; }
        public void Revert();
    }
}
