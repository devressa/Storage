using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureBucketManipulatorFactory
    {
        IAzureBucketManipulator Create(string accountName, string primaryAccessKey);
    }
}
