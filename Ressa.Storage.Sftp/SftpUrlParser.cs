using System;
using Ressa.Storage.Sftp.Interfaces;

namespace Ressa.Storage.Sftp
{
    public class SftpUrlParser : ISftpUrlParser
    {
        private const int DefaultPort = 22;
        private const string SftpSchema = "sftp://";

        public int GetPort(string sftpUrl)
        {
            var uri = CreateSftpUri(sftpUrl);
            return uri.Port == -1 ? DefaultPort : uri.Port;
        }

        public string GetHost(string sftpUrl)
        {
            var uri = CreateSftpUri(sftpUrl);
            return uri.Host;
        }

        private static Uri CreateSftpUri(string sftpUrl)
        {
            return new Uri(SftpSchema + sftpUrl);
        }
    }
}
