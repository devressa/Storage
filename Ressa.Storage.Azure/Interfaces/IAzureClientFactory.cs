using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureClientFactory
    {
        IAzureClientWrapper CreateInstance(AzureBucket bucket);
    }
}
