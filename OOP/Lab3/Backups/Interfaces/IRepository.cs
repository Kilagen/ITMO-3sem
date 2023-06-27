namespace Backups.Interfaces
{
    public interface IRepository
    {
        public string Root { get; }
        public bool Contains(string path);
        public IRepObject GetRepObject(string path);
        public Stream CreateFile(string path);
        public void CreateDir(string path);
        public string JoinPath(string path1, string path2);
    }
}
