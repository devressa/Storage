using System;

namespace Ressa.Storage.Sftp
{
    public class SftpProvider
    {
        public virtual string Key { get; set; }
        public virtual string SftpUrl { get; set; }
        public virtual string SftpUserName { get; set; }
        public virtual string SftpPrivateKey { get; set; }
        public virtual string SftpPassPhrase { get; set; }
        public virtual string SftpFolder { get; set; }

        public virtual string FulfilmentSuccessNotificationTemplateKey { get; set; }
    }
}
