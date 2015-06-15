using System;

namespace Ressa.Storage.Sftp.Exceptions
{
    public class SftpTimeoutException : Exception
    {
        public string FileSource { get; private set; }
        public string FileDestination { get; private set; }
        public ulong UploadedBytes { get; private set; }
        public long ElapsedMilliseconds { get; private set; }

        public SftpTimeoutException(string fileSource, string fileDestination, ulong uploadedBytes, long elapsedMilliseconds)
            : this(fileSource, fileDestination, uploadedBytes, elapsedMilliseconds, null)
        {
        }

        public SftpTimeoutException(string fileSource, string fileDestination, ulong uploadedBytes, long elapsedMilliseconds, Exception innerException)
            : base(string.Format("Did not complete copying of file '{0}' to SFTP '{1}'. Bytes transferred: {2} ({3:########0} KB).  Upload timed out after {4}ms.",
                                 fileSource,fileDestination, uploadedBytes, uploadedBytes / 1024, elapsedMilliseconds), innerException)
        {
            FileSource = fileSource;
            FileDestination = fileDestination;
            UploadedBytes = uploadedBytes;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
