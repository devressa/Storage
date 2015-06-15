using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IUrlGenerator
    {
        string GetUrl(AmazonS3Bucket s3Bucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null);
        string GetUploadUrl(AmazonS3Bucket s3Bucket, string fileName, DateTimeOffset expiryTime);
        string GetPreviewUrl(AmazonS3Bucket s3Bucket, string fileName);
    }
}
