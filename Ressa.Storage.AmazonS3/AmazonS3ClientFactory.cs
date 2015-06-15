using System;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class AmazonS3ClientFactory : IAmazonS3ClientFactory
    {
        public IAmazonS3ClientWrapper CreateInstance(AmazonS3Bucket bucket)
        {
            return new AmazonS3ClientWrapper(bucket);
        }
    }
}
