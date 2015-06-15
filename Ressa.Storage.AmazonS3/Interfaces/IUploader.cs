using System;
using System.IO;
using Amazon.S3.Model;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IUploader
    {
        void Upload(AmazonS3Bucket bucket, string fileName, byte[] content);
        bool Upload(AmazonS3Bucket bucket, string fileName, Stream stream, S3CannedACL filePermission = S3CannedACL.PublicReadWrite, S3StorageClass storageType = S3StorageClass.Standard);
        bool Delete(AmazonS3Bucket bucket, string filename);
        void Copy(AmazonS3Bucket sourceBucket, string sourceFileName, AmazonS3Bucket targetBucket, string targetFileName);
    }
}
