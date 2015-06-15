using System;
using System.Collections.Generic;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface ILister
    {
        IEnumerable<string> ListObjectsInBucket(AmazonS3Bucket bucket, string prefix);
    }
}
