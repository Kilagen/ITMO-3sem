using System.IO.Compression;
using Backups.Interfaces;
using Backups.StorageEntities;

namespace Backups.Models
{
    public class ZipVisitor : IVisitor
    {
        private readonly Stack<ZipArchive> _visitStack;
        private readonly Stack<List<IZipObject>> _entriesStack;

        public ZipVisitor(ZipArchive archive)
        {
            _visitStack = new Stack<ZipArchive>();
            _visitStack.Push(archive);
            _entriesStack = new Stack<List<IZipObject>>();
            _entriesStack.Push(new List<IZipObject>());
        }

        public IReadOnlyList<IZipObject> Entries => _entriesStack.Peek();

        public void Visit(IFileObject fileObject)
        {
            ZipArchiveEntry entry = _visitStack.Peek().CreateEntry(fileObject.Name);
            using Stream filestream = fileObject.GetStream();
            using Stream stream = entry.Open();
            filestream.CopyTo(stream);

            _entriesStack.Peek().Add(new ZipFileEntry(fileObject.Name));
        }

        public void Visit(IDirObject dirObject)
        {
            ZipArchiveEntry entry = _visitStack.Peek().CreateEntry(dirObject.Name + ".zip");
            _visitStack.Push(new ZipArchive(entry.Open(), ZipArchiveMode.Create));
            _entriesStack.Push(new List<IZipObject>());
            foreach (IRepObject repObject in dirObject.GetEntries())
            {
                repObject.Accept(this);
            }

            _visitStack.Pop();
            _entriesStack.Peek().Add(new ZipFolder(dirObject.Name, _entriesStack.Pop()));
        }
    }
}
