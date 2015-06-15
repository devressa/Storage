using System;

namespace Ressa.Storage.Sftp.Interfaces
{
    public interface ISftpUrlParser
    {
        int GetPort(string sftpUrl);
        string GetHost(string sftpUrl);
    }
}
