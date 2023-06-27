using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities
{
    public class Transaction : ITransaction
    {
        private Action? revert;
        public Transaction(IBankAccount sender, IBankAccount receiver, decimal amount)
        {
            if (amount <= 0)
                throw new TransactionException("Cannot transfer negative amount");
            if (receiver.Equals(sender))
                throw new TransactionException("Cannot transfer to the same account");
            if (!sender.CanDecrease(amount))
                throw new TransactionException("Not enough money on sender account");
            if (!receiver.CanIncrease(amount))
                throw new TransactionException("Not enough space on receiver account");

            decimal senderBefore = sender.Balance.Amount;
            decimal receiverBefore = receiver.Balance.Amount;
            sender.Decrease(amount);
            receiver.Increase(amount);
            decimal senderAfter = sender.Balance.Amount;
            decimal receiverAfter = receiver.Balance.Amount;

            revert = () =>
            {
                sender.Balance.Increase(senderBefore - senderAfter);
                receiver.Balance.Decrease(receiverAfter - receiverBefore);
            };

            SenderId = sender.Id;
            ReceiverId = receiver.Id;
            Amount = amount;
            IsReverted = false;
        }

        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public decimal Amount { get; }
        public bool IsReverted { get; private set; }

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
            return $"Transaction from {SenderId} to {ReceiverId} for {Amount}";
        }
    }
}
