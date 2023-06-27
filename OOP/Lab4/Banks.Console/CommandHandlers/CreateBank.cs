using Banks.Console.Exceptions;
using Banks.Entities;
using Banks.Models;
using Banks.Models.Configs;

namespace Banks.Console.CommandHandlers
{
    public class CreateBank : ICommandHandler
    {
        private ICommandHandler? next;
        public void Handle(string[] args, DataSpace space)
        {
            if (args.Length < 3 || args[0] != "create" || args[1] != "bank")
            {
                if (next is null)
                    throw new NoHandlerException();
                next.Handle(args, space);
                return;
            }

            if (space.CentralBank is null)
                throw new BankConsoleException("Central bank is not created");

            string name = args[2];
            DepositAccountConfiguration depositConfig = ParseDepositConfig();
            CreditAccountConfiguration creditConfig = ParseCreditConfig();
            DebitAccountConfiguration debitConfig = ParseDebitConfig();
            var config = new BankConfiguration(creditConfig, debitConfig, depositConfig);
            var bank = new Bank(name, space.CentralBank, config);
            System.Console.WriteLine($"Bank {bank} created");
        }

        public void Help()
        {
            System.Console.WriteLine("create bank <name> - creates a bank");
            System.Console.WriteLine("  Asks to write account configurations for bank");
            next?.Help();
        }

        public ICommandHandler SetNext(ICommandHandler handler)
        {
            next = handler;
            return next;
        }

        private DepositAccountConfiguration ParseDepositConfig()
        {
            System.Console.Write("Enter deposit account period (days): ");
            var minPeriod = TimeSpan.FromDays(int.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input")));

            Banks.Models.PercentageStrategy percentageModel = ParsePercentageModel();

            System.Console.Write("Enter deposit account unsafe transaction limit: ");
            decimal unsafeLimit = decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input"));

            return new DepositAccountConfiguration(minPeriod, percentageModel, unsafeLimit);
        }

        private Banks.Models.PercentageStrategy ParsePercentageModel()
        {
            System.Console.Write("Enter initial percentage value: ");
            var initialPercentage = new Percent(decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input")));
            var builder = new Banks.Models.PercentageStrategy.Builder(initialPercentage);
            System.Console.WriteLine("Enter \"Finish\" when you're done with percentage model");
            string value;
            while (true)
            {
                System.Console.Write("Enter threshold value: ");
                value = System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input");
                if (value == "Finish")
                    break;
                decimal threshold = decimal.Parse(value);
                System.Console.Write("Enter percentage value: ");
                value = System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input");
                if (value == "Finish")
                    break;
                var percent = new Percent(decimal.Parse(value));
                builder.WithThresholdPercent(threshold, percent);
            }

            return builder.Build();
        }

        private CreditAccountConfiguration ParseCreditConfig()
        {
            System.Console.Write("Enter credit account comission: ");
            decimal comission = decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input"));

            System.Console.Write("Enter credit account unsafe transaction limit: ");
            decimal unsafeLimit = decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input"));
            return new CreditAccountConfiguration(comission, unsafeLimit);
        }

        private DebitAccountConfiguration ParseDebitConfig()
        {
            System.Console.Write("Enter debit account percent: ");
            var percent = new Percent(decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input")));

            System.Console.Write("Enter debit account unsafe transaction limit: ");
            decimal unsafeLimit = decimal.Parse(System.Console.ReadLine() ?? throw new BankConsoleException("Invalid input"));
            return new DebitAccountConfiguration(percent, unsafeLimit);
        }
    }
}
