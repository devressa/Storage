using System;

namespace Ressa.Storage.Sftp.Interfaces
{
    public interface ISftpClientFactory
    {
        IClient Create(SftpProvider sftpDetails);
        void Release(IClient component);
    }
}
