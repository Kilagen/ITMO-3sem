namespace Backups.Exceptions
{
    public class BackupObjectNotFoundException : BackupsException
    {
        public BackupObjectNotFoundException(string message)
            : base(message) { }

        public static BackupObjectNotFoundException Create(string path, string root)
        {
            return new BackupObjectNotFoundException($"{path} in {root} doesn't exist");
        }

        public static BackupObjectNotFoundException Create(string path)
        {
            return new BackupObjectNotFoundException($"{path} doesn't exist");
        }
    }
}
