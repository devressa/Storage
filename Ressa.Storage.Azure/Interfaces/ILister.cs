using System;
using System.Collections.Generic;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface ILister
    {
        IEnumerable<string> ListObjectsInBucket(AzureBucket bucket, string prefix);
    }
}
