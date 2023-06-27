using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities
{
    public class Shop
    {
        private readonly Storage _storage;

        public Shop(string name, string address, Guid id)
        {
            _storage = new Storage();
            Name = name;
            Address = address;
            Id = id;
        }

        public Shop(string name, string address)
            : this(name, address, Guid.NewGuid()) { }

        public string Name { get; }
        public string Address { get; }
        public Guid Id { get; }

        public void Supply(List<StorageProductStack> supplies)
        {
            supplies.ForEach(_storage.Supply);
        }

        public bool Contains(Cart cart)
        {
            foreach (CartProductStack cartStack in cart.ProductStacks)
            {
                StorageProductStack? foundStack = _storage.Find(cartStack.Product);
                if (foundStack is null || foundStack.Count < cartStack.Count)
                {
                    return false;
                }
            }

            return true;
        }

        public decimal GetTotalCost(Cart cart)
        {
            if (!Contains(cart))
            {
                throw new ShopException("Shop doesn't contain all items from cart");
            }

            return cart.ProductStacks
                .Sum(cartStack => _storage.Get(cartStack.Product).Price * cartStack.Count);
        }

        public void Purchase(Person person)
        {
            if (!Contains(person.Cart))
            {
                throw new ShopException("Shop doesn't contain all items from cart");
            }

            decimal cost = GetTotalCost(person.Cart);

            // If balance is not enough, throws exception. So the storage, balance and cart won't change
            person.DecreaseBalance(cost);
            foreach (CartProductStack cartStack in person.Cart.ProductStacks)
            {
                _storage.DecreaseCount(cartStack.Product, cartStack.Count);
            }

            person.EmptyCart();
        }

        public void SetPrice(Product product, decimal price)
        {
            _storage.SetPrice(product, price);
        }
    }
}
