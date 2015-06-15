using System;

namespace Ressa.Storage.AmazonS3.Exceptions
{
    public class AmazonS3Exception : Exception
    {
        public AmazonS3Exception(string message, Exception innerexception) : base(message, innerexception)
        {
        }
    }
}
