using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IBackup
    {
        IEnumerable<RestorePoint> RestorePoints { get; }
        void Add(RestorePoint restorePoint);
    }
}
