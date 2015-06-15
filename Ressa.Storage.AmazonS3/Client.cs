using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Amazon.S3.Model;
using Ressa.Storage.AmazonS3.Dtos;
using Ressa.Storage.AmazonS3.Exceptions;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class Client : IClient
    {
        private readonly ISigner signer;

        public Client(ISigner signer)
        {
            this.signer = signer;
        }

        private static DateTime GetCurrentDateTime()
        {
            var timeStamp = DateTime.Now;
            return new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day, timeStamp.Hour, timeStamp.Minute, timeStamp.Second, timeStamp.Millisecond, DateTimeKind.Local).ToUniversalTime();
        }

        private static AmazonS3Client1 GetService()
        {
            return new AmazonS3Client1(new BasicHttpBinding
            {
                Security = { Mode = BasicHttpSecurityMode.Transport },
                MaxReceivedMessageSize = 214748364
            }, new EndpointAddress("https://s3.amazonaws.com/soap"));
        }

        private string Sign(AmazonS3Bucket bucket, string methodName, DateTime currentTime)
        {
            var toSign = string.Format("AmazonS3{0}{1}", methodName, currentTime.ToString(@"yyyy-MM-dd\THH:mm:ss.fff\Z", CultureInfo.InvariantCulture));
            return signer.Sign(bucket, toSign);
        }

        public void PutObjectInline(AmazonS3Bucket bucket, string fileName, MetadataEntry[] metadataEntries, byte[] data, Grant[] accessControlList, StorageClass storageClass, string credential)
        {
            var currentTime = GetCurrentDateTime();
            GetService().PutObjectInline(bucket.Name, fileName, metadataEntries, data, data.Length, accessControlList, storageClass, bucket.PublicKey, currentTime, Sign(bucket, "PutObjectInline", currentTime), credential);
        }

        public AmazonS3FileDetails GetFileDetails(S3Bucket bucket, string filename)
        {
            var s3Object = ListBucket(bucket, filename, 1).S3Objects.SingleOrDefault();

            if (s3Object == null)
                throw new AmazonS3FileNotFoundException(string.Format("File: '{0}' was not found in bucket: '{1}'", filename, bucket));

            return new AmazonS3FileDetails
            {
                Filename = s3Object.Key,
                SizeInBytes = s3Object.Size,
                BucketName = s3Object.BucketName,
                ETag = s3Object.ETag,
                LastModified = DateTime.Parse(s3Object.LastModified)
            };
        }

        public ListObjectsResponse ListBucket(AmazonS3Bucket bucket, string prefix = null, int maxKeys = 1000, string marker = null, string delimiter = null)
        {
            prefix = prefix ?? string.Empty;
            marker = marker ?? string.Empty;
            delimiter = delimiter ?? string.Empty;

            using (var client = new Amazon.S3.AmazonS3Client(bucket.PublicKey, bucket.SecretKey))
            {
                return client.ListObjects(new ListObjectsRequest().WithBucketName(bucket.Name).WithPrefix(prefix).WithMarker(marker).WithMaxKeys(maxKeys).WithDelimiter(delimiter));
            }
        }
    }
}
