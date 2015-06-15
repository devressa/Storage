using System;
using System.Collections.Generic;
using System.Linq;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class Lister : ILister
    {
        private readonly IClient client;

        public Lister(IClient client)
        {
            this.client = client;
        }

        public IEnumerable<string> ListObjectsInBucket(AzureBucket bucket, string prefix)
        {
            return client.ListBucket(bucket, prefix).Select(o => o.Name).ToArray();
        }
    }
}
