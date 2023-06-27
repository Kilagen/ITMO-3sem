using System.IO.Compression;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.StorageEntities;

namespace Backups.Entities
{
    public class ZipMapper : IZipToRepMapper
    {
        private readonly ZipFolder _zipFolder;
        private Stream? _archiveStream;

        public ZipMapper(ZipFolder zipFolder, IRepository repository, string path)
        {
            _zipFolder = zipFolder;
            Repository = repository;
            Path = path;
        }

        public IRepository Repository { get; }
        public string Path { get; }

        public void Dispose()
        {
            if (_archiveStream is not null)
            {
                _archiveStream.Dispose();
            }
        }

        public IReadOnlyList<IRepObject> GetRepObjects()
        {
            if (Repository.GetRepObject(Path) is not IFileObject obj)
                throw new BackupsException($"{Path} is not a file");

            if (_archiveStream is null)
                _archiveStream = obj.GetStream();

            using var archive = new ZipArchive(_archiveStream, ZipArchiveMode.Read);
            return archive.Entries
                .Select(entry => _zipFolder.Entries.First(child => child.Name.Equals(entry.Name)).AsRepObject(entry))
                .ToList();
        }
    }
}
