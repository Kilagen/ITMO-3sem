using Banks.Entities;

namespace Banks.Console
{
    public class DataSpace
    {
        private static DataSpace? _instance;
        private readonly List<Bank> banks = new List<Bank>();
        private readonly List<Client> clients = new List<Client>();

        public IReadOnlyList<Bank> Banks => banks;
        public IReadOnlyList<Client> Clients => clients;

        public CentralBank? CentralBank { get; set; }
        public bool Exit { get; set; }

        public static DataSpace Instance() => _instance ??= new DataSpace();

        public void Add(Bank bank)
        {
            if (banks.Contains(bank))
            {
                System.Console.WriteLine($"{bank} is already in the list");
                return;
            }

            banks.Add(bank);
            System.Console.WriteLine($"{bank} added. Bank id: {banks.Count}");
        }

        public void Add(Client client)
        {
            if (clients.Contains(client))
            {
                System.Console.WriteLine($"{client} is already in the list");
                return;
            }

            clients.Add(client);
            System.Console.WriteLine($"{client} added. Client id: {clients.Count}");
        }
    }
}
