using Backups.Exceptions;
using Backups.Interfaces;
using Zio.FileSystems;

namespace Backups.Entities
{
    public class InMemoryRepository : IRepository
    {
        public InMemoryRepository(string root, MemoryFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(root))
                throw new BackupsException("Root directory cannot be null or empty");

            Root = root;
            RepFileSystem = fileSystem;

            RepFileSystem.CreateDirectory(Root);
        }

        public string Root { get; }
        public MemoryFileSystem RepFileSystem { get; }

        public bool Contains(string path)
        {
            string fullPath = JoinPath(Root, path);
            return RepFileSystem.FileExists(fullPath) || RepFileSystem.DirectoryExists(fullPath);
        }

        public void CreateDir(string path) => RepFileSystem.CreateDirectory(JoinPath(Root, path));

        public Stream CreateFile(string path) => RepFileSystem.OpenFile(JoinPath(Root, path), FileMode.Create, FileAccess.Write);

        public IRepObject GetRepObject(string path)
        {
            string fullPath = JoinPath(Root, path);
            if (RepFileSystem.FileExists(fullPath))
            {
                return new FileObject(
                    Path.GetFileName(fullPath),
                    () => RepFileSystem.OpenFile(fullPath, FileMode.Open, FileAccess.Read));
            }

            if (RepFileSystem.DirectoryExists(fullPath))
            {
                return new DirectoryObject(
                    Path.GetFileName(fullPath),
                    () => RepFileSystem
                    .EnumerateItems(fullPath, SearchOption.TopDirectoryOnly)
                    .Select(x => GetRepObject(x.FullName[Root.Length..]))
                    .ToList());
            }

            throw BackupObjectNotFoundException.Create(path, Root);
        }

        public string JoinPath(string path1, string path2) => $"{path1}/{path2}";
    }
}
