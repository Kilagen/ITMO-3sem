namespace Backups.Interfaces
{
    public interface IRepObject
    {
        string Name { get; }

        public void Accept(IVisitor visitor);
    }
}
