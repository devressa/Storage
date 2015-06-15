using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureBucketManipulator
    {
        bool CreateBucket(string name);
        bool CheckIfBucketExists(string name);
        bool DeleteBucket(string name);
        CloudBlobContainer GetBucket(string name);
        IEnumerable<string> GetAllBucketsName();
    }
}

