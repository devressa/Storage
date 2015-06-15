using System;
using System.Runtime.Serialization;

namespace Ressa.Storage.Sftp.Exceptions
{
    public class SftpException : Exception
    {
        public SftpException()
        {
        }

        public SftpException(string message) : base(message)
        {
        }

        public SftpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SftpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
