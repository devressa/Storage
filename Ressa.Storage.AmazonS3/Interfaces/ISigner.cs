using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface ISigner
    {
        string Sign(AmazonS3Bucket bucket, string toSign);
    }
}
