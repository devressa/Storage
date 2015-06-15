using System;
using System.IO;

namespace Ressa.Common.Logger
{
    public class FileLog : BaseLog
    {
        private readonly object locker = new object();
        private readonly string path;

        public FileLog(string filePath)
        {
            path = filePath;
            if (File.Exists(path))
                File.Delete(path);
        }

        public override void Write(string value)
        {
            lock (locker)
            {
                using (var streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine("{0}: {1}", DateTime.Now, value);
                }
            }
        }
    }
}
