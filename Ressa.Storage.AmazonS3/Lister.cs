using System;
using System.Collections.Generic;
using System.Linq;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class Lister : ILister
    {
        private readonly IClient client;

        public Lister(IClient client)
        {
            this.client = client;
        }

        public IEnumerable<string> ListObjectsInBucket(AmazonS3Bucket bucket, string prefix)
        {
            var response = client.ListBucket(bucket, prefix);
            return response.S3Objects.Select(o => o.Key).ToList();
        }
    }
}
