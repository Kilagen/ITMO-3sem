using System.IO.Compression;
using Backups.Interfaces;
using Backups.Models;
using Backups.StorageEntities;

namespace Backups.Entities
{
    public class ZipArchivator : IArchivator
    {
        public string Extention => ".zip";

        public IStorage CreateStorage(IReadOnlyList<IRepObject> objects, IRepository repository, string path, Stream stream)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            var visitor = new ZipVisitor(archive);
            foreach (IRepObject obj in objects)
            {
                obj.Accept(visitor);
            }

            return new ZipStorage(new ZipFolder(Guid.NewGuid().ToString(), visitor.Entries), path, repository);
        }
    }
}
