using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageEntities
{
    public class ZipFolder : IZipObject
    {
        public ZipFolder(string name, IEnumerable<IZipObject> entries)
        {
            Name = name;
            Entries = entries;
        }

        public string Name { get; }

        public IEnumerable<IZipObject> Entries { get; }

        public IRepObject AsRepObject(ZipArchiveEntry entry)
        {
            return new DirectoryObject(
                entry.Name[..^4],
                () => new ZipArchive(
                    entry.Open(),
                    ZipArchiveMode.Read)
                    .Entries.Select(x => Entries.First(y => y.Name.Equals(x.Name)).AsRepObject(x)).ToList());
        }
    }
}
