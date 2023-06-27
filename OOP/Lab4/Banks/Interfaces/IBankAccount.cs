using Banks.Entities;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankAccount : IUpdateState
    {
        public Balance Balance { get; }
        public Bank Bank { get; }
        public Client Client { get; }
        public Guid Id { get; }
        public bool CanDecrease(decimal amount);
        public bool CanIncrease(decimal amount);
        public void Decrease(decimal amount);
        public void Increase(decimal amount);
    }
}
