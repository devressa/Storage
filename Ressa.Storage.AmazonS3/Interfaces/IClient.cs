using System;
using Amazon.S3.Model;
using Ressa.Storage.AmazonS3.Dtos;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IClient
    {
        void PutObjectInline(AmazonS3Bucket bucket, string fileName, MetadataEntry[] metadataEntries, byte[] data, Grant[] accessControlList, StorageClass storageClass, string credential);
        AmazonS3FileDetails GetFileDetails(AmazonS3Bucket bucket, string filename);
        ListObjectsResponse ListBucket(AmazonS3Bucket bucket, string prefix = null, int maxKeys = 10000, string marker = null, string delimiter = null);
    }
}
