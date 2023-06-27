using Backups.Entities;

namespace Backups.Interfaces
{
    public interface IBackupTask
    {
        void Add(BackupObject backupObject);
        void Remove(BackupObject backupObject);

        RestorePoint Restore(DateTime dateTime);
    }
}
