namespace Backups.Interfaces
{
    public interface IStorage
    {
        public IZipToRepMapper GetMapper();
    }
}
