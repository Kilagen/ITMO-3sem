using Backups.Interfaces;

namespace Backups.Entities
{
    public class FileObject : IFileObject
    {
        private readonly Func<Stream> _func;
        public FileObject(string name, Func<Stream> func)
        {
            Name = name;
            _func = func;
        }

        public string Name { get; }
        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public Stream GetStream() => _func();
    }
}
