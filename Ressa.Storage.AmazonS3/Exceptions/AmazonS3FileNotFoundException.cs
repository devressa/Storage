using System;

namespace Ressa.Storage.AmazonS3.Exceptions
{
    public class AmazonS3FileNotFoundException : Exception
    {
        public AmazonS3FileNotFoundException(string message) : base(message)
        {
        }
    }
}
