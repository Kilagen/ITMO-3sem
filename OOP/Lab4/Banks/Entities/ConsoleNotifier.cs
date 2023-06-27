using Banks.Interfaces;

namespace Banks.Entities
{
    public class ConsoleNotifier : INotifier
    {
        private Client client;

        public ConsoleNotifier(Client client)
        {
            this.client = client;
        }

        public void Notify(string message)
        {
            System.Console.WriteLine($"{client}: {message}");
        }
    }
}
