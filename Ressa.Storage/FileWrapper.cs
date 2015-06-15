using System;
using System.IO;
using Ressa.Storage.Interfaces;

namespace Ressa.Storage
{
    public class FileWrapper : IFileWrapper
    {
        public string GetTempFileName { get { return Path.GetTempFileName(); } }

        public Stream CreateFileStream(string path, FileMode fileMode, FileAccess fileAccess)
        {
            return new FileStream(path, fileMode, fileAccess);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
