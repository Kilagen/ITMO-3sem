using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities
{
    public class BackupObject
    {
        public BackupObject(string path, IRepository repository)
        {
            if (!repository.Contains(path))
            {
                throw BackupObjectNotFoundException.Create(path, repository.Root);
            }

            Id = Guid.NewGuid();
            Path = path;
            Repository = repository;
        }

        public Guid Id { get; }
        public string Path { get; }
        public IRepository Repository { get; }
    }
}
