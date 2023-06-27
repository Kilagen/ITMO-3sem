using Shops.Entities;
using Shops.Exceptions;
using Shops.Manager;
using Shops.Models;
using Xunit;

namespace Shops.Test
{
    public class ShopTests
    {
        [Fact]
        public void PersonBuysMoreThanAvailable_ThrowException()
        {
            var tyndexMarket = new ShopManager();
            Shop diksi1 = tyndexMarket.RegisterShop("Diksonchik", "Na Kronve");

            Product pivo05 = tyndexMarket.RegisterItem("Pivo 0.5");
            Product vino10 = tyndexMarket.RegisterItem("Vino 1.0");
            diksi1.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 40, 1), new StorageProductStack(vino10, 10, 2) });

            var albert = new Person("Albert", new Balance(10_000));
            albert.Cart.Add(pivo05, 50);
            Assert.Throws<ShopException>(() => diksi1.Purchase(albert));
            albert.Cart.Add(vino10, 1);
            Assert.Throws<ShopException>(() => diksi1.Purchase(albert));
            albert.Cart.Add(vino10, 11);
            Assert.Throws<ShopException>(() => diksi1.Purchase(albert));
        }

        [Fact]
        public void PersonReachNegativeBalance_ThrowException()
        {
            var tyndexMarket = new ShopManager();
            Shop diksi1 = tyndexMarket.RegisterShop("Diksonchik", "Na Kronve");

            Product pivo05 = tyndexMarket.RegisterItem("Pivo 0.5");
            Product konyak = tyndexMarket.RegisterItem("Konyak Dorogoy");
            diksi1.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 40, 100), new StorageProductStack(konyak, 5, 5000) });

            var albert = new Person("Albert", new Balance(1_000));
            albert.Cart.Add(pivo05, 10);
            albert.Cart.Add(konyak, 1);
            Assert.Throws<InvalidBalanceException>(() => diksi1.Purchase(albert));
        }

        [Fact]
        public void ShopManagerFindsCheapestShop()
        {
            var tyndexMarket = new ShopManager();
            Shop diksi1 = tyndexMarket.RegisterShop("Diksonchik", "Na Kronve");
            Shop diksi2 = tyndexMarket.RegisterShop("Diksonchik", "Na Vyazme");
            Shop diksi3 = tyndexMarket.RegisterShop("Diksonchik", "Na Kamenoostrovskom");

            Product pivo05 = tyndexMarket.RegisterItem("Pivo 0.5");
            Product vino10 = tyndexMarket.RegisterItem("Vino 1.0");

            // No vine, only 10 bears, but cheapest
            diksi1.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 10, 100) });

            // Cheapest vine, but only one bottle
            diksi2.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 40, 111), new StorageProductStack(vino10, 1, 300) });

            diksi3.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 40, 110), new StorageProductStack(vino10, 10, 500) });

            var albert = new Person("Albert", new Balance(1_000));

            // 10 * 100 vs 10 * 111 vs 10 * 110
            // 1000 vs 1110 vs 1100. 1st wins
            albert.Cart.Add(pivo05, 10);
            Assert.Equal(diksi1, tyndexMarket.FindMinimalCostShop(albert.Cart));

            // Not Enogh vs 20 * 111 vs 10 * 110
            // NE vs 2220 vs 2200. 3rd wins
            albert.Cart.Add(pivo05, 10);
            Assert.Equal(diksi3, tyndexMarket.FindMinimalCostShop(albert.Cart));

            // Not Enough vs 20 * 111 + 1 * 300 vs 20 * 110 + 1 * 500
            // NE vs 2520 vs 2700. 2nd wins
            albert.Cart.Add(vino10, 1);
            Assert.Equal(diksi2, tyndexMarket.FindMinimalCostShop(albert.Cart));

            // Not Enough vs Not Enough vs 20 * 110 + 3 * 500
            // NE vs NE vs 3700. 3rd wins
            albert.Cart.Add(vino10, 3);
            Assert.Equal(diksi3, tyndexMarket.FindMinimalCostShop(albert.Cart));

            // Not Enough vs Not Enough vs Not Enough
            albert.Cart.Add(vino10, 30);
            Assert.Null(tyndexMarket.FindMinimalCostShop(albert.Cart));
        }

        [Fact]
        public void PersonBuysProducts_BalanceAndProductsDecrease()
        {
            var tyndexMarket = new ShopManager();
            Shop diksi1 = tyndexMarket.RegisterShop("Diksonchik", "Na Kronve");

            Product pivo05 = tyndexMarket.RegisterItem("Pivo 0.5");
            Product vino10 = tyndexMarket.RegisterItem("Vino 1.0");
            diksi1.Supply(new List<StorageProductStack>()
            { new StorageProductStack(pivo05, 40, 100), new StorageProductStack(vino10, 10, 500) });

            decimal balance = 10_000;
            var albert = new Person("Albert", new Balance(balance));
            albert.Cart.Add(pivo05, 3);

            diksi1.Purchase(albert);
            Assert.Equal(10_000 - (3 * 100), albert.Balance.Amount);

            diksi1.Purchase(albert);
            Assert.Equal(10_000 - (3 * 100), albert.Balance.Amount);

            diksi1.SetPrice(pivo05, 59);
            albert.Cart.Add(vino10, 4);
            albert.Cart.Add(pivo05, 13);
            diksi1.Purchase(albert);
            Assert.Equal(10_000 - (3 * 100) - (4 * 500) - (13 * 59), albert.Balance.Amount);
        }
    }
}
