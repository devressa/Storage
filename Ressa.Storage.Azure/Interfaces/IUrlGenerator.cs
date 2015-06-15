using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IUrlGenerator
    {
        string GetUrl(AzureBucket azureBucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null);
        string GetUploadUrl(AzureBucket azureBucket, string fileName, DateTimeOffset expiryTime);
        string GetPreviewUrl(AzureBucket azureBucket, string fileName);
    }
}
