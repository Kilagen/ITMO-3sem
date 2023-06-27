using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities
{
    public class Backup : IBackup
    {
        private readonly List<RestorePoint> _points;

        public Backup()
        {
            _points = new List<RestorePoint>();
        }

        public IEnumerable<RestorePoint> RestorePoints => _points;
        public void Add(RestorePoint restorePoint)
        {
            if (_points.Contains(restorePoint))
                throw new BackupsException("Restore point is already in Backup");
            _points.Add(restorePoint);
        }
    }
}
