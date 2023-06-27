namespace Backups.Interfaces
{
    public interface IDirObject : IRepObject
    {
        public IReadOnlyList<IRepObject> GetEntries();
    }
}
