using System;

namespace Ressa.Storage.WebDav.Exceptions
{
    public class WebDavConflictException : WebDavException
    {
        public WebDavConflictException()
        {
        }

        public WebDavConflictException(string message) : base(message)
        {
        }

        public WebDavConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WebDavConflictException(int httpCode, string message, Exception innerException) : base(httpCode, message, innerException)
        {
        }

        public WebDavConflictException(int httpCode, string message) : base(httpCode, message)
        {
        }

    }
}
