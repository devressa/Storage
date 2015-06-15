using System.IO;

namespace Ressa.Storage.Interfaces
{
    public interface IFileWrapper
    {
        string GetTempFileName { get; }
        Stream CreateFileStream(string path, FileMode fileMode, FileAccess fileAccess);
        void Delete(string path);
    }
}
