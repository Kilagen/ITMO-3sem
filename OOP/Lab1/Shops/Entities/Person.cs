using Shops.Models;

namespace Shops.Entities
{
    public class Person
    {
        public Person(string name, Balance balance)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (balance is null)
                throw new ArgumentNullException(nameof(balance));

            Name = name;
            Balance = balance;
            Cart = new Cart();
            Id = Guid.NewGuid();
        }

        public Person(string name)
            : this(name, new Balance(0)) { }

        public string Name { get; }
        public Guid Id { get; }
        public Balance Balance { get; }
        public Cart Cart { get; private set; }

        public void EmptyCart()
        {
            Cart = new Cart();
        }

        public decimal IncreaseBalance(decimal amount)
        {
            return Balance.Increase(amount);
        }

        public decimal DecreaseBalance(decimal amount)
        {
            return Balance.Decrease(amount);
        }
    }
}
