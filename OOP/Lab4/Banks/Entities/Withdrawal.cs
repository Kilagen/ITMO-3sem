using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities
{
    public class Withdrawal : ITransaction
    {
        private Action? revert;
        public Withdrawal(IBankAccount account, decimal amount)
        {
            if (amount <= 0)
                throw new TransactionException("Cannot transfer negative amount");
            if (!account.CanDecrease(amount))
                throw new TransactionException("Not enough money on sender account");

            decimal before = account.Balance.Amount;
            account.Decrease(amount);
            decimal after = account.Balance.Amount;

            revert = () => account.Balance.Increase(before - after);

            Amount = amount;
            AccountId = account.Id;
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
            return $"Withdrawal {AccountId} {Amount}";
        }
    }
}
