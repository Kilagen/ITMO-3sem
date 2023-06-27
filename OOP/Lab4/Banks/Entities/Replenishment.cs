using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities
{
    public class Replenishment : ITransaction
    {
        private Action? revert;
        public Replenishment(IBankAccount account, decimal amount)
        {
            if (amount <= 0)
                throw new TransactionException("Cannot transfer negative amount");
            if (!account.CanIncrease(amount))
                throw new TransactionException("Not enough space on receiver account");

            decimal before = account.Balance.Amount;
            account.Increase(amount);
            decimal after = account.Balance.Amount;
            revert = () => account.Balance.Decrease(after - before);

            AccountId = account.Id;
            Amount = amount;
            IsReverted = false;
        }

        public bool IsReverted { get; private set; }
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public void Revert()
        {
            if (IsReverted)
                throw new TransactionException("Transaction is already reverted");
            revert?.Invoke();
            revert = null;
            IsReverted = true;
        }

        public override string ToString()
        {
            return $"Replenishment: {AccountId} {Amount}";
        }
    }
}
