namespace Backups.Interfaces
{
    public interface IVisitor
    {
        public void Visit(IFileObject fileObject);
        public void Visit(IDirObject dirObject);
    }
}
