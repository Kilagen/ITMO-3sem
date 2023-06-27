using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageEntities
{
    public class ZipFileEntry : IZipObject
    {
        public ZipFileEntry(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public IRepObject AsRepObject(ZipArchiveEntry entry) => new FileObject(entry.Name, () => entry.Open());
    }
}
