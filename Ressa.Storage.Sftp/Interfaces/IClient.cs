using System;
using Ressa.Storage.Sftp.Dtos;

namespace Ressa.Storage.Sftp.Interfaces
{
    public interface IClient : IDisposable
    {
        SftpFileDetails CopyFile(string srcFileName, string dstFileName);
        void Connect();
        bool Exists(string path);
        void CreateDirectory(string path);
    }
}
