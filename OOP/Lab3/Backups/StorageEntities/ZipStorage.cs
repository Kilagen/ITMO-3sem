using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageEntities
{
    public class ZipStorage : IStorage
    {
        private readonly ZipFolder _zipFolder;

        public ZipStorage(ZipFolder zipFolder, string path, IRepository repository)
        {
            _zipFolder = zipFolder;
            Path = path;
            Repository = repository;
        }

        public string Path { get; }
        public IRepository Repository { get; }

        public IZipToRepMapper GetMapper() => new ZipMapper(_zipFolder, Repository, Path);
    }
}
