using System;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureBuilder
    {
        CloudBlobClient BuildClient(AzureBucket bucket);
        CloudBlobContainer BuildContainer(AzureBucket bucket);
    }
}
