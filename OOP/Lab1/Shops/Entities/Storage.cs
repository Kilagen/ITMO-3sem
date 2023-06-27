using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities
{
    public class Storage
    {
        private List<StorageProductStack> _itemStacks;
        public Storage()
        {
            _itemStacks = new List<StorageProductStack>();
        }

        public IReadOnlyList<StorageProductStack> ItemStacks => _itemStacks;

        public StorageProductStack? Find(Product product)
        {
            return _itemStacks.FirstOrDefault(stack => stack.Product.Id == product.Id);
        }

        public StorageProductStack Get(Product product)
        {
            StorageProductStack? found = Find(product);
            if (found is null)
            {
                throw new StorageException("Storage doesn't contain this product");
            }

            return found;
        }

        public void DecreaseCount(Product product, int count)
        {
            StorageProductStack foundStack = Get(product);

            if (foundStack.Count - count < 0)
            {
                throw new StorageException("Count must be bigger than zero");
            }

            _itemStacks.Remove(foundStack);
            if (foundStack.Count - count > 0)
            {
                _itemStacks.Add(new StorageProductStack(foundStack.Product, foundStack.Count - count, foundStack.Price));
            }
        }

        public void SetPrice(Product product, decimal price)
        {
            StorageProductStack foundStack = Get(product);

            if (price <= 0)
            {
                throw new StorageException("Price must be bigger than zero");
            }

            _itemStacks.Remove(foundStack);
            _itemStacks.Add(new StorageProductStack(foundStack.Product, foundStack.Count, price));
        }

        public void Supply(StorageProductStack supplyStack)
        {
            StorageProductStack? foundStack = Find(supplyStack.Product);

            if (foundStack is null)
            {
                _itemStacks.Add(supplyStack);
            }
            else
            {
                _itemStacks.Remove(foundStack);
                _itemStacks.Add(
                    new StorageProductStack(
                        foundStack.Product, foundStack.Count + supplyStack.Count, foundStack.Price));
            }
        }
    }
}
