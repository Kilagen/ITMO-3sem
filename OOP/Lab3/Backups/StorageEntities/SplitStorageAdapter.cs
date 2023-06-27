using Backups.Entities;
using Backups.Interfaces;

namespace Backups.StorageEntities
{
    public class SplitStorageAdapter : IStorage
    {
        public SplitStorageAdapter(IReadOnlyList<IStorage> storages)
        {
            Storages = storages;
        }

        public IEnumerable<IStorage> Storages { get; }
        public IZipToRepMapper GetMapper() => new SplitStorageMapper(Storages.Select(s => s.GetMapper()).ToList());
    }
}
