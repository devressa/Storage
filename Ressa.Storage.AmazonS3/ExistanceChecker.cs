using System;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class ExistanceChecker : IExistanceChecker
    {
        private readonly IClient client;

        public ExistanceChecker(IClient client)
        {
            this.client = client;
        }

        public bool Exists(AmazonS3Bucket bucket, string fileName)
        {
            return client.ListBucket(bucket, fileName, 1).S3Objects.Count > 0;
        }
    }
}
