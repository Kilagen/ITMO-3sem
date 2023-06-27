namespace Backups.Interfaces
{
    public interface IFileObject : IRepObject
    {
        public Stream GetStream();
    }
}
