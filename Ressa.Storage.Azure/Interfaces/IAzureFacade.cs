using System;
using System.Collections.Generic;
using System.IO;
using Ressa.Storage.Azure.Dtos;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureFacade
    {
        void Upload(AzureBucket bucket, string fileName, byte[] content);
        bool Upload(AzureBucket bucket, string fileName, Stream stream);

        string GetUrl(AzureBucket bucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null);
        string GetUploadUrl(AzureBucket bucket, string fileName);
        string GetPreviewUrl(AzureBucket bucket, string fileName);

        bool Exists(AzureBucket bucket, string fileName);
        bool Delete(AzureBucket bucket, string fileName);
        void Copy(AzureBucket sourceBucket, string sourceFileName, AzureBucket targetBucket, string targetFileName);

        IEnumerable<string> GetListOfFiles(AzureBucket bucket, string prefix = null);
        AzureFileDetails GetFileDetails(AzureBucket bucket, string fileName);
    }
}
