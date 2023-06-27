using Backups.Interfaces;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(IEnumerable<BackupObject> backupObjects, IStorage storage, DateTime dateTime, Guid id)
        {
            BackupObjects = backupObjects;
            Storage = storage;
            DateTime = dateTime;
            Id = id;
        }

        public DateTime DateTime { get; }
        public Guid Id { get; }
        public IEnumerable<BackupObject> BackupObjects { get; }
        public IStorage Storage { get; }
    }
}
