using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Xunit;

namespace Banks.Test
{
    public class BanksTest
    {
        [Fact]
        public void AssertTrueTrue()
        {
            var cb = new CentralBank(new Models.Clock(DateTime.Now));
            Bank sber = cb.RegisterBank(
                "Sber",
                new Models.Configs.BankConfiguration(
                     new Models.Configs.CreditAccountConfiguration(10, 100000),
                     new Models.Configs.DebitAccountConfiguration(new Models.Percent(2), 100000),
                     new Models.Configs.DepositAccountConfiguration(
                         TimeSpan.FromDays(180),
                         new PercentageStrategy.Builder(new Percent(10)).Build(),
                         10000)));
            Client me = new Client.ClientBuilder("Kirill", "Zakharov")
                .WithAddress(new Address("Spb", "Vyazma", "5-7", "1222"))
                .WithPassport(new Passport("123", "r4e34")).Build();
            IBankAccount acc = sber.RegisterDebitAccount(me);
            ITransaction trans = new Replenishment(acc, 2000);
            Assert.Equal(2000, acc.Balance.Amount);
        }
    }
}
