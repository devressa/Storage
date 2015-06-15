using System;
using System.Runtime.Serialization;

namespace Ressa.Storage.Sftp.Exceptions
{
    public class SftpUploadException : Exception
    {
        public string DestinationFile { get; private set; }
        public long ExpectedSize { get; private set; }
        public long ActualSize { get; private set; }
        public ulong UploadedBytes { get; private set; }
        public long ElapsedMilliseconds { get; private set; }

        public SftpUploadException(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                ErrorMessage = info.GetString("ErrorMessage");
            }
        }

        public SftpUploadException(string destinationFile, long expectedSize, long actualSize, ulong uploadedBytes, long elapsedMilliseconds)
            : base(string.Format("Destination file '{0}' is not the expected size.  Expected size: '{1}'; Actual size: '{2}'. Bytes transferred: {3} ({4:########0} KB).  Upload timed out after {5}ms.",
                                 destinationFile, expectedSize, actualSize, uploadedBytes, uploadedBytes / 1024, elapsedMilliseconds))
        {
            DestinationFile = destinationFile;
            ExpectedSize = expectedSize;
            ActualSize = actualSize;
            UploadedBytes = uploadedBytes;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public string ErrorMessage { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ErrorMessage", ErrorMessage);
        }
    }
}
