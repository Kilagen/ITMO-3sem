using Shops.Entities;
using Shops.Models;

namespace Shops.Manager
{
    public interface IShopManager
    {
        Shop RegisterShop(string name, string address);
        Product RegisterItem(string name);
        Shop? FindMinimalCostShop(Cart cart);
    }
}
