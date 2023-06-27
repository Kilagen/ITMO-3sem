using Backups.Entities;
using Backups.Interfaces;
using Backups.StorageEntities;

namespace Backups.Algorithms
{
    public class SplitStorageAlgorithm : IAlgorithm
    {
        private readonly IArchivator _archivator;

        public SplitStorageAlgorithm(IArchivator archivator)
        {
            _archivator = archivator;
        }

        public IStorage CreateStorage(IReadOnlyList<BackupObject> backupObjects, IRepository repository, string saveDirectory)
        {
            var storages = new List<IStorage>();
            foreach (BackupObject obj in backupObjects)
            {
                string filePath = $"{repository.JoinPath(saveDirectory, Guid.NewGuid().ToString())}.{_archivator.Extention}";
                Stream stream = repository.CreateFile(filePath);
                storages.Add(_archivator.CreateStorage(new List<IRepObject> { repository.GetRepObject(obj.Path) }, repository, filePath, stream));
            }

            return new SplitStorageAdapter(storages);
        }
    }
}
