using System;

namespace Ressa.Storage.AmazonS3.Exceptions
{
    public class AmazonS3UploadExeption : Exception
    {
        public AmazonS3UploadExeption(string message) : base(message) { }
    }
}
