using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IAlgorithm
    {
        IStorage CreateStorage(IReadOnlyList<BackupObject> backupObjects, IRepository repository, string saveDirectory);
    }
}
