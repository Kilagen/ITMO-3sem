using Backups.Interfaces;

namespace Backups.Entities
{
    public class DirectoryObject : IDirObject
    {
        private readonly Func<IReadOnlyList<IRepObject>> _func;
        public DirectoryObject(string name, Func<IReadOnlyList<IRepObject>> func)
        {
            Name = name;
            _func = func;
        }

        public string Name { get; }
        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public IReadOnlyList<IRepObject> GetEntries() => _func();
    }
}
