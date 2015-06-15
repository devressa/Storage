using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Ressa.Common.Logger;
using Ressa.Storage.Azure.Dtos;
using Ressa.Storage.Azure.Exceptions;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class Client : IClient
    {
        private readonly IAzureBuilder builder;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
        public Client(IAzureBuilder builder)
        {
            this.builder = builder;
        }

        public void PutObjectInline(AzureBucket bucket, string fileName, byte[] data)
        {
            using (var s = new MemoryStream(data))
            {
                PutObjectInline(bucket, fileName, s);
            }
        }

        public void PutObjectInline(AzureBucket bucket, string fileName, Stream stream)
        {
            try
            {
                builder.BuildContainer(bucket).GetBlockBlobReference(fileName).UploadFromStream(stream);
            }
            catch (Exception ex)
            {
                throw new AzureException("Can't upload file: " + fileName, ex);
            }
        }

        private CloudBlockBlob GetCloudBlockBlob(AzureBucket bucket, string blobName)
        {
            CloudBlockBlob blob;
            try
            {
                blob = builder.BuildContainer(bucket).GetBlockBlobReference(blobName);
            }
            catch (Exception ex)
            {
                throw new AzureException("Can't get block blob: {0}", ex);
            }
            return blob;
        }

        public AzureFileDetails GetFileDetails(AzureBucket bucket, string filename)
        {
            var item = GetCloudBlockBlob(bucket, filename);
            var notFoundMessage = string.Format("File: '{0}' was not found in bucket: '{1}'", filename, bucket);

            if (item == null)
            {
                Log.Write(notFoundMessage);
                throw new AzureFileNotFoundException(notFoundMessage);
            }

            LoadCloudBlockBlobAttributes(notFoundMessage, item);

            AzureFileDetails fileDetails;
             
            return MapCloudBlockBlobkToAzureFileDetails(bucket, filename, item, out fileDetails) ? fileDetails : fileDetails;
        }

        private static bool MapCloudBlockBlobkToAzureFileDetails(AzureBucket bucket, string filename, ICloudBlob item, out AzureFileDetails fileDetails)
        {
            fileDetails = new AzureFileDetails
            {
                Filename = item.Name,
                SizeInBytes = item.Properties.Length,
                BucketName = bucket.Name,
                ETag = item.Properties.ETag
            };

            if (!item.Properties.LastModified.HasValue)
            {
                Log.Write("Item {0} in bucket {1} has no last modified property", filename, bucket.Name);
                return true;
            }
            fileDetails.LastModified = item.Properties.LastModified.Value.UtcDateTime;

            return false;
        }

        private static void LoadCloudBlockBlobAttributes(string notFoundMessage, ICloudBlob item)
        {
            try
            {
                item.FetchAttributes();
            }
            catch (StorageException exception)
            {
                Log.Write(exception,notFoundMessage);
                throw new AzureFileNotFoundException(notFoundMessage);
            }
        }

        public IEnumerable<CloudBlockBlob> ListBucket(AzureBucket bucket, string prefix = null, int maxKeys = 1000)
        {
            prefix = prefix ?? string.Empty;

            Log.Write("ListBucket: AccountName = {0}, Name = {1}, PrimaryKey = {2}, Secondary = {3}, Prefix = {4}", bucket.AccountName, bucket.Name, bucket.PrimaryAccessKey, bucket.SecondaryAccessKey, prefix);

            try
            {
                var cloudBlockBlobItems = builder.BuildContainer(bucket)
                    .ListBlobsSegmented(prefix, true, BlobListingDetails.All, maxKeys, new BlobContinuationToken(), null, null)
                    .Results
                    .OfType<CloudBlockBlob>()
                    .ToArray();

                Log.Write("Found {0} blobs with prefix '{1}'", cloudBlockBlobItems.Count(), prefix);

                return cloudBlockBlobItems;
            }
            catch (StorageException exception)
            {
                Log.Write("Caught a StorageException: {0}", exception);
                throw new AzureException("Can't find bucket: " + bucket.Name, exception);
            }
        }

        public bool DoesFileExist(AzureBucket bucket, string fileName)
        {
            try
            {
                var blob = GetCloudBlockBlob(bucket, fileName); 

                try
                {
                    return blob.Exists();
                }
                catch (Exception exception)
                {
                    Log.Write("{0}", exception);
                    throw;
                }
            }
            catch (StorageException exception)
            {
                Log.Write("Caught a StorageException: {0}", exception);
                throw new AzureException("Can't find bucket: " + bucket.Name, exception);
            }
        }
    }
}
