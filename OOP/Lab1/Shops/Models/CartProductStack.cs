using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models
{
    public class CartProductStack
    {
        public CartProductStack(Product product, int count)
        {
            if (count <= 0)
            {
                throw new StacksException("Count must be bigger than zero");
            }

            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            Product = product;
            Count = count;
        }

        public Product Product { get; }
        public int Count { get; }
    }
}
