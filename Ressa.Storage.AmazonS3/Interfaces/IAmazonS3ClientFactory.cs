using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IAmazonS3ClientFactory
    {
        IAmazonS3ClientWrapper CreateInstance(AmazonS3Bucket bucket);
    }
}
