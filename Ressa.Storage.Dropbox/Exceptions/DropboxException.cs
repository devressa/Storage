using System;

namespace Ressa.Storage.Dropbox.Exceptions
{
    public class DropboxException : Exception
    {
        public DropboxException()
        {
        }

        public DropboxException(string message) : base(message)
        {
        }
    }
}
