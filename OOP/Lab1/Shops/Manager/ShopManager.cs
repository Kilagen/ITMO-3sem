using Shops.Entities;
using Shops.Models;

namespace Shops.Manager
{
    public class ShopManager : IShopManager
    {
        private readonly List<Shop> _shops;
        private readonly List<Product> _products;
        public ShopManager()
        {
            _shops = new List<Shop>();
            _products = new List<Product>();
        }

        public IReadOnlyList<Shop> Shops => _shops;
        public IReadOnlyList<Product> Products => _products;

        public Shop? FindMinimalCostShop(Cart cart)
        {
            return _shops.Where(shop => shop.Contains(cart)).MinBy(shop => shop.GetTotalCost(cart));
        }

        public Product RegisterItem(string name)
        {
            var newShop = new Product(name);
            _products.Add(newShop);
            return newShop;
        }

        public Shop RegisterShop(string name, string address)
        {
            var newShop = new Shop(name, address);
            _shops.Add(newShop);
            return newShop;
        }
    }
}
