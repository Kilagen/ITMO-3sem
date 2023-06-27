using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models
{
    public class BackupTask : IBackupTask
    {
        private readonly List<BackupObject> _backupObjects;
        private readonly IBackup _backup;
        public BackupTask(string name, IRepository repository, IAlgorithm algorithm, IBackup backup)
        {
            _backup = backup;
            _backupObjects = new List<BackupObject>();
            Repository = repository;
            Name = name;
            Algorithm = algorithm;

            Repository.CreateDir(name);
        }

        public BackupTask(string name, IRepository repository, IAlgorithm algorithm)
            : this(name, repository, algorithm, new Backup()) { }

        public string Name { get; }
        public IReadOnlyList<BackupObject> BackupObjects => _backupObjects;
        public IAlgorithm Algorithm { get; }
        public IRepository Repository { get; }
        public void Add(BackupObject backupObject)
        {
            if (_backupObjects.Contains(backupObject))
                throw new BackupsException("Backup object already tracked");

            if (backupObject.Repository != Repository || !Repository.Contains(backupObject.Path))
                throw BackupObjectNotFoundException.Create(backupObject.Path);

            _backupObjects.Add(backupObject);
        }

        public void Remove(BackupObject backupObject)
        {
            if (!_backupObjects.Contains(backupObject))
                throw new BackupsException("Backup object isn't tracked");

            _backupObjects.Remove(backupObject);
        }

        public RestorePoint Restore(DateTime dateTime)
        {
            var id = Guid.NewGuid();
            string path = Repository.JoinPath(Name, id.ToString());
            Repository.CreateDir(path);
            IStorage storage = Algorithm.CreateStorage(_backupObjects, Repository, path);
            var restorePoint = new RestorePoint(_backupObjects, storage, dateTime, id);
            _backup.Add(restorePoint);
            return restorePoint;
        }
    }
}
