using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models
{
    public class StorageProductStack
    {
        public StorageProductStack(Product product, int count, decimal price)
        {
            if (count <= 0)
            {
                throw new StacksException("Count must be bigger than zero");
            }

            if (price <= 0)
            {
                throw new StacksException("Price must be bigger than zero");
            }

            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            Product = product;
            Price = price;
            Count = count;
        }

        public int Count { get; }

        public decimal Price { get; }

        public Product Product { get; }
    }
}
