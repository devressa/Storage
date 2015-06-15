using System;
using System.Collections.Generic;
using Amazon.S3.Model;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IAmazonS3ClientWrapper : IDisposable
    {
        InitiateMultipartUploadResponse InitiateMultipartUpload(string bucketName, string key);
        UploadPartResponse UploadPart(string bucketName, string key, string uploadId, int partNumber, long fileLength, string file);
        void CompleteMultipartUpload(string bucketName, string key, string uploadId, IEnumerable<UploadPartResponse> uploadPartResponses);
        void AbortMultipartUpload(string bucketName, string key, string uploadId);
        void Delete(string bucketName, string key);
        void Copy(string sourceBucketName, string sourceKey, string targetBucketName, string targetKey);
    }
}
