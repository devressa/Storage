using System;
using System.Collections.Generic;
using System.IO;
using Amazon.S3.Model;
using Ressa.Storage.AmazonS3.Dtos;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class AmazonS3Facade : IAmazonS3Facade
    {
        private readonly IUploader uploader;
        private readonly IUrlGenerator urlGenerator;
        private readonly IExistanceChecker existanceChecker;
        private readonly ILister lister;
        private readonly IClient client;

        public AmazonS3Facade(IUploader uploader, IUrlGenerator urlGenerator, IExistanceChecker existanceChecker, ILister lister, IClient client)
        {
            this.uploader = uploader;
            this.urlGenerator = urlGenerator;
            this.existanceChecker = existanceChecker;
            this.lister = lister;
            this.client = client;
        }

        public bool Upload(AmazonS3Bucket bucket, string fileName, Stream stream)
        {
            var result = uploader.Upload(bucket, fileName, stream);
            return result;
        }

        public bool Delete(AmazonS3Bucket bucket, string fileName)
        {
			var result = uploader.Delete(bucket, fileName);
            return result;
        }

        public void Copy(AmazonS3Bucket sourceBucket, string sourceFileName, AmazonS3Bucket targetBucket, string targetFileName)
        {
            uploader.Copy(sourceBucket, sourceFileName, targetBucket, targetFileName);
        }

        public AmazonS3FileDetails GetFileDetails(AmazonS3Bucket bucket, string fileName)
        {
            return client.GetFileDetails(bucket, fileName);
        }

        public string GetPreviewUrl(AmazonS3Bucket bucket, string fileName)
        {
            return urlGenerator.GetPreviewUrl(bucket, fileName);
        }

        public void Upload(AmazonS3Bucket bucket, string fileName, byte[] content)
        {
            uploader.Upload(bucket, fileName, content);
        }

        public string GetUrl(AmazonS3Bucket bucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null)
        {
            return Exists(bucket, fileName) ? urlGenerator.GetUrl(bucket, fileName, expiryTime, contentDispositionFileName) : "";
        }

        public string GetUploadUrl(AmazonS3Bucket bucket, string fileName)
		{
			return urlGenerator.GetUploadUrl(bucket, fileName, DateTimeOffset.UtcNow.AddHours(24));
		}

        public bool Exists(AmazonS3Bucket bucket, string fileName)
        {
            return existanceChecker.Exists(bucket, fileName);
        }

        public IEnumerable<string> GetListOfFiles(AmazonS3Bucket bucket, string prefix = null)
        {
            return lister.ListObjectsInBucket(bucket, prefix);
        }
 
    }
}
