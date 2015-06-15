using System;
using System.Runtime.Serialization;
using System.Web;

namespace Ressa.Storage.WebDav.Exceptions
{
    public class WebDavException : HttpException
    {
        public WebDavException()
        {
        }

        public WebDavException(string message) : base(message)
        {
        }

        public WebDavException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WebDavException(int httpCode, string message, Exception innerException) : base(httpCode, message, innerException)
        {
        }

        public WebDavException(int httpCode, string message) : base(httpCode, message)
        {
        }

        protected WebDavException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
