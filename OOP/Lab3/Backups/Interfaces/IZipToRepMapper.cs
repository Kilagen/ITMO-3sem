namespace Backups.Interfaces
{
    public interface IZipToRepMapper : IDisposable
    {
        public IReadOnlyList<IRepObject> GetRepObjects();
    }
}
