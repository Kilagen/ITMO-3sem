using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities
{
    public class FileSystemRepository : IRepository
    {
        public FileSystemRepository(string root)
        {
            if (string.IsNullOrEmpty(root))
                throw new BackupsException("Root directory cannot be null or empty");

            Root = root;
            Directory.CreateDirectory(Root);
        }

        public string Root { get; }

        public bool Contains(string path)
        {
            string fullPath = JoinPath(Root, path);
            return File.Exists(fullPath) || Directory.Exists(fullPath);
        }

        public void CreateDir(string path)
        {
            Directory.CreateDirectory(JoinPath(Root, path));
        }

        public Stream CreateFile(string path)
            => File.Open(JoinPath(Root, path), FileMode.Create, FileAccess.Write);

        public IRepObject GetRepObject(string path)
        {
            string fullPath = JoinPath(Root, path);
            if (File.Exists(fullPath))
            {
                return new FileObject(
                    Path.GetFileName(fullPath),
                    () => File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            }

            if (Directory.Exists(fullPath))
            {
                return new DirectoryObject(
                    Path.GetFileName(fullPath),
                    () => Directory
                    .EnumerateFileSystemEntries(fullPath, "*", SearchOption.TopDirectoryOnly)
                    .Select(x => GetRepObject(x[Root.Length..]))
                    .ToList());
            }

            throw BackupObjectNotFoundException.Create(path, Root);
        }

        public string JoinPath(string path1, string path2) => Path.Combine(path1, path2);
    }
}
