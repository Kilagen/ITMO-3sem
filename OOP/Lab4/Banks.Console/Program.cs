using Banks.Console;
using Banks.Console.AccountHandlers;
using Banks.Console.ClockHandlers;
using Banks.Console.CommandHandlers;
using Banks.Console.TransactionHandlers;

var space = new DataSpace();
IAccountHandler accountHandler = new DebitAccountHandler();
accountHandler
    .SetNext(new DepositAccountHandler())
    .SetNext(new CreditAccountHandler());

ITransactionHandler transactionHandler = new TransactionHandler();
transactionHandler
    .SetNext(new ReplenishmentHandler())
    .SetNext(new WithdrawalHandler());

ICommandHandler commandHandler = new HelpConsole();
commandHandler
    .SetNext(new CreateCentralBank(new ClockHandler()))
    .SetNext(new CreateBank())
    .SetNext(new CreateClient())
    .SetNext(new CreateAccount(accountHandler))
    .SetNext(new GetClientAccounts())
    .SetNext(new CreateTransaction(transactionHandler))
    .SetNext(new SkipDay())
    .SetNext(new ExitConsole());

while (!space.Exit)
{
    string[] userArgs = System.Console.ReadLine() !.ToLower().Split(' ');
    try
    {
        commandHandler.Handle(userArgs, space);
    }
    catch (Exception exception)
    {
        System.Console.WriteLine(exception.Message);
    }

    System.Console.WriteLine();
}

Environment.Exit(0);