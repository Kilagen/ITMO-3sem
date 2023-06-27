using Backups.Interfaces;

namespace Backups.Entities
{
    public class SplitStorageMapper : IZipToRepMapper
    {
        public SplitStorageMapper(IReadOnlyList<IZipToRepMapper> storages)
        {
            Storages = storages;
        }

        public IReadOnlyList<IZipToRepMapper> Storages { get; }
        public void Dispose()
        {
            foreach (IZipToRepMapper storage in Storages)
            {
                storage.Dispose();
            }
        }

        public IReadOnlyList<IRepObject> GetRepObjects()
        {
            return Storages.SelectMany(x => x.GetRepObjects()).ToList();
        }
    }
}
