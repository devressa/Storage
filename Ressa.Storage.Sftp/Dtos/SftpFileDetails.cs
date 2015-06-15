using System;


namespace Ressa.Storage.Sftp.Dtos
{
    public class SftpFileDetails
    {
        public ulong UploadedBytes { get; private set; }
        public long ElapsedMilliseconds { get; private set; } 

        public SftpFileDetails(ulong uploadedBytes, long elapsedMilliseconds)
        {
            UploadedBytes = uploadedBytes;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
