using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Algorithms
{
    public class SingleStorageAlgorithm : IAlgorithm
    {
        private readonly IArchivator _archivator;
        public SingleStorageAlgorithm(IArchivator archivator)
        {
            _archivator = archivator;
        }

        public IStorage CreateStorage(
            IReadOnlyList<BackupObject> backupObjects,
            IRepository repository,
            string saveDirectory)
        {
            string filePath = $"{repository.JoinPath(saveDirectory, Guid.NewGuid().ToString())}.{_archivator.Extention}";
            Stream stream = repository.CreateFile(filePath);
            return _archivator.CreateStorage(
                backupObjects.Select(obj => repository.GetRepObject(obj.Path)).ToList(), repository, filePath, stream);
        }
    }
}
