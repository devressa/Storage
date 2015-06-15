using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Ressa.Storage.Azure.Exceptions;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public sealed class AzureClientWrapper : IAzureClientWrapper
    {
        private readonly CloudBlobClient client;

        public AzureClientWrapper(AzureBucket bucket, IAzureBuilder azureComponentsBuilder)
        {
            client = azureComponentsBuilder.BuildClient(bucket);
        }
        public CloudBlockBlob InitiateMultipartUpload(string bucketName, string key)
        {
            return client.GetContainerReference(bucketName).GetBlockBlobReference(key);
        }

        public void UploadPart(CloudBlockBlob blob, string blockIdBase64, string filePath)
        {
            using (var s = File.OpenRead(filePath))
            {
                blob.PutBlock(blockIdBase64, s, null);
            }
        }

        public void CompleteMultipartUpload(CloudBlockBlob blob, IEnumerable<string> uploadPartResponses)
        {
            blob.PutBlockList(uploadPartResponses);
        }

        public void AbortMultipartUpload(CloudBlockBlob blob)
        {
            blob.Delete();
        }

        public bool Delete(string bucketName, string key)
        {
            var reference = client.GetContainerReference(bucketName).GetBlockBlobReference(key);
            return reference.DeleteIfExists();
        }

        public void Copy(string sourceBucketName, string sourceKey, string targetBucketName, string targetKey)
        {
            try
            {
                var source = client.GetContainerReference(sourceBucketName).GetBlockBlobReference(sourceKey);
                var target = client.GetContainerReference(targetBucketName).GetBlockBlobReference(targetKey);
                target.StartCopyFromBlob(source);
            }
            catch (StorageException ex)
            {
                throw new AzureException("Can't copy file", ex);
            }
        }

        public void Dispose()
        {
        }
    }
}
