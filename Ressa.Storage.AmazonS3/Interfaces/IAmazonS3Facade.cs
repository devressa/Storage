using System;
using System.Collections.Generic;
using System.IO;
using Ressa.Storage.AmazonS3.Dtos;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IAmazonS3Facade
    {
        void Upload(AmazonS3Bucket bucket, string fileName, byte[] content);
        bool Upload(AmazonS3Bucket bucket, string fileName, Stream stream);

        string GetUrl(AmazonS3Bucket bucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null);
        string GetUploadUrl(AmazonS3Bucket bucket, string fileName);
        string GetPreviewUrl(AmazonS3Bucket bucket, string fileName);

        bool Exists(AmazonS3Bucket bucket, string fileName);
        bool Delete(AmazonS3Bucket bucket, string fileName);
        void Copy(AmazonS3Bucket sourceBucket, string sourceFileName, AmazonS3Bucket targetBucket, string targetFileName);

        IEnumerable<string> GetListOfFiles(AmazonS3Bucket bucket, string prefix = null);
        AmazonS3FileDetails GetFileDetails(AmazonS3Bucket bucket, string fileName);
    }
}
