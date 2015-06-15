using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3.Model;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public sealed class AmazonS3ClientWrapper : IAmazonS3ClientWrapper
    {
        private readonly AmazonS3Client1 amazonS3Client;

        public AmazonS3ClientWrapper(AmazonS3Bucket s3Bucket)
        {
            amazonS3Client = new AmazonS3Client1(s3Bucket.PublicKey, s3Bucket.SecretKey);
        }

        public InitiateMultipartUploadResponse InitiateMultipartUpload(string bucketName, string key)
        {
            var initiateMultipartUploadRequest = new InitiateMultipartUploadRequest().WithBucketName(bucketName).WithKey(key);
            return amazonS3Client.InitiateMultipartUpload(initiateMultipartUploadRequest);
        }

        public UploadPartResponse UploadPart(string bucketName, string key, string uploadId, int partNumber, long fileLength, string file)
        {
            UploadPartRequest uploadRequest = new UploadPartRequest().WithBucketName(bucketName).WithKey(key).WithUploadId(uploadId).WithPartNumber(partNumber).WithPartSize(fileLength).WithFilePosition(0).WithFilePath(file);
            return amazonS3Client.UploadPart(uploadRequest);
        }

        public void CompleteMultipartUpload(string bucketName, string key, string uploadId, IEnumerable<UploadPartResponse> uploadPartResponses)
        {
            var completeRequest = new CompleteMultipartUploadRequest().WithBucketName(bucketName).WithKey(key).WithUploadId(uploadId).WithPartETags(uploadPartResponses.OrderBy(x => x.PartNumber));
            amazonS3Client.CompleteMultipartUpload(completeRequest);
        }

        public void AbortMultipartUpload(string bucketName, string key, string uploadId)
        {
            amazonS3Client.AbortMultipartUpload(new AbortMultipartUploadRequest().WithBucketName(bucketName).WithKey(key).WithUploadId(uploadId));
        }

        public void Delete(string bucketName, string key)
        {
            DeleteObjectRequest deleteRequest = new DeleteObjectRequest().WithBucketName(bucketName).WithKey(key);
            amazonS3Client.DeleteObject(deleteRequest);
        }

        public void Copy(string sourceBucketName, string sourceKey, string targetBucketName, string targetKey)
        {
            var copyRequest = new CopyObjectRequest().WithSourceBucket(sourceBucketName).WithSourceKey(sourceKey).WithDestinationBucket(targetBucketName).WithDestinationKey(targetKey);
            amazonS3Client.CopyObject(copyRequest).Dispose();
        }

        public void Dispose()
        {
            amazonS3Client.Dispose();
        }
    }
}
