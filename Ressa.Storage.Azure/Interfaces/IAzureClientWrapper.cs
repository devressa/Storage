using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IAzureClientWrapper : IDisposable
    {
        CloudBlockBlob InitiateMultipartUpload(string bucketName, string key);
        void UploadPart(CloudBlockBlob blob, string blockIdBase64, string filePath);
        void CompleteMultipartUpload(CloudBlockBlob blob, IEnumerable<string> uploadPartResponses);
        void AbortMultipartUpload(CloudBlockBlob blob);
        bool Delete(string bucketName, string key);
        void Copy(string sourceBucketName, string sourceKey, string targetBucketName, string targetKey);
    }
}
