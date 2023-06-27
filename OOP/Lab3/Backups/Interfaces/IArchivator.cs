namespace Backups.Interfaces
{
    public interface IArchivator
    {
        public string Extention { get; }
        IStorage CreateStorage(IReadOnlyList<IRepObject> objects, IRepository repository, string path, Stream stream);
    }
}
