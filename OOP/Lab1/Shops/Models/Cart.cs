using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models
{
    public class Cart
    {
        private List<CartProductStack> _productStacks;
        public Cart()
        {
            _productStacks = new List<CartProductStack>();
        }

        public IReadOnlyList<CartProductStack> ProductStacks => _productStacks;

        public bool Contains(Product item)
        {
            return _productStacks.Any(stack => stack.Product.Id == item.Id);
        }

        public CartProductStack? Find(Product product)
        {
            return _productStacks.FirstOrDefault(stack => stack.Product.Id == product.Id);
        }

        public CartProductStack Get(Product product)
        {
            CartProductStack? found = Find(product);
            if (found is null)
            {
                throw new CartException("Product not found");
            }

            return found;
        }

        public bool Add(Product item, int count)
        {
            if (count <= 0)
            {
                throw new CartException("Count must be bigger than zero");
            }

            if (Contains(item))
            {
                CartProductStack found = Get(item);
                _productStacks.Remove(found);
                _productStacks.Add(new CartProductStack(found.Product, found.Count + count));
            }
            else
            {
                _productStacks.Add(new CartProductStack(item, count));
            }

            return true;
        }

        public bool Remove(Product item)
        {
            if (!Contains(item))
            {
                return false;
            }

            CartProductStack found = Get(item);
            _productStacks.Remove(found);
            return true;
        }

        public void SetCount(Product product, int count)
        {
            if (count <= 0)
            {
                throw new CartException("Count must be bigger than zero");
            }

            if (!Contains(product))
            {
                throw new CartException("Product not found");
            }

            CartProductStack found = Get(product);
            _productStacks.Remove(found);
            _productStacks.Add(new CartProductStack(found.Product, count));
        }
    }
}
