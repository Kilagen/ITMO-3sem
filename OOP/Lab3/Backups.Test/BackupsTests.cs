using Backups.Algorithms;
using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;
using Backups.StorageEntities;
using Xunit;
using Zio.FileSystems;

namespace Backups.Test
{
    public class BackupsTests : IDisposable
    {
        private MemoryFileSystem fileSystem;
        private IRepository repository;
        private BackupObject backupObject1;
        private BackupObject backupObject2;
        private BackupObject backupObject3;

        public BackupsTests()
        {
            fileSystem = new MemoryFileSystem();
            repository = new InMemoryRepository("/TestBackups", fileSystem);
            repository.CreateDir("testDir");
            string path = repository.JoinPath("testDir", "testFile1");
            Stream fileStream = repository.CreateFile(path);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write("Hello World!");
            }

            fileStream.Dispose();

            backupObject1 = new BackupObject(path, repository);

            path = repository.JoinPath("testDir", "testFile1");
            fileStream = repository.CreateFile(path);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write("Hello Suren!");
            }

            fileStream.Dispose();

            backupObject2 = new BackupObject(path, repository);

            fileStream = repository.CreateFile("testFile3");
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write("Hello World!");
            }

            fileStream.Dispose();

            backupObject3 = new BackupObject("testFile3", repository);
        }

        [Fact]
        public void SplitStorageTest()
        {
            IBackup backup = new Backup();
            IBackupTask backupTask = new BackupTask(
                "SplitTest",
                repository,
                new SplitStorageAlgorithm(new ZipArchivator()),
                backup);
            backupTask.Add(backupObject1);
            backupTask.Add(backupObject2);
            backupTask.Restore(DateTime.Now);
            backupTask.Remove(backupObject2);
            backupTask.Restore(DateTime.Now);
            Assert.Equal(2, backup.RestorePoints.Count());

            Assert.Equal(2, backup.RestorePoints.ElementAt(0).Storage.GetMapper().GetRepObjects().Count);
            Assert.Equal(1, backup.RestorePoints.ElementAt(1).Storage.GetMapper().GetRepObjects().Count);
        }

        [Fact]
        public void SingleStorageTest()
        {
            IBackup backup = new Backup();
            IBackupTask backupTask = new BackupTask(
                "SingleTest",
                repository,
                new SingleStorageAlgorithm(new ZipArchivator()),
                backup);
            backupTask.Add(backupObject1);
            backupTask.Add(backupObject2);
            backupTask.Restore(DateTime.Now);
            backupTask.Remove(backupObject2);
            backupTask.Restore(DateTime.Now);
            backupTask.Add(backupObject3);
            backupTask.Restore(DateTime.Now);
            Assert.Equal(3, backup.RestorePoints.Count());
            foreach (RestorePoint restorePoint in backup.RestorePoints)
            {
                Assert.True(restorePoint.Storage is ZipStorage);
            }
        }

        [Fact]
        public void BackupTaskThrowsExceptions()
        {
            IBackup backup = new Backup();
            IBackupTask backupTask = new BackupTask(
                "ExceptionsTest",
                repository,
                new SingleStorageAlgorithm(new ZipArchivator()),
                backup);
            backupTask.Add(backupObject1);
            Assert.Throws<BackupsException>(() => backupTask.Add(backupObject1));
            Assert.Throws<BackupsException>(() => backupTask.Remove(backupObject2));
            Assert.Throws<BackupsException>(() => backupTask.Remove(backupObject3));
        }

        public void Dispose()
        {
            fileSystem.Dispose();
        }
    }
}
