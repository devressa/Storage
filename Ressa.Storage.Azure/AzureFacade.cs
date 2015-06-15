using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ressa.Common.Logger;
using Ressa.Storage.Azure.Dtos;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class AzureFacade : IAzureFacade
    {
        private readonly IUploader uploader;
        private readonly IUrlGenerator urlGenerator;
        private readonly IExistanceChecker existanceChecker;
        private readonly ILister lister;
        private readonly IClient client;
        private readonly ILog log = LogManager.GetLogger(typeof(AzureFacade));

        public AzureFacade(IUploader uploader, IUrlGenerator urlGenerator, IExistanceChecker existanceChecker, ILister lister, IClient client)
        {
            this.uploader = uploader;
            this.urlGenerator = urlGenerator;
            this.existanceChecker = existanceChecker;
            this.lister = lister;
            this.client = client;
        }

        public bool Upload(AzureBucket bucket, string fileName, Stream stream)
        {
            return uploader.Upload(bucket, fileName, stream);
        }

        public bool Delete(AzureBucket bucket, string fileName)
        {
            return uploader.Delete(bucket, fileName);
        }

        public void Copy(AzureBucket sourceBucket, string sourceFileName, AzureBucket targetBucket, string targetFileName)
        {
            uploader.Copy(sourceBucket, sourceFileName, targetBucket, targetFileName);
        }

        public AzureFileDetails GetFileDetails(AzureBucket bucket, string fileName)
        {
            return client.GetFileDetails(bucket, fileName);
        }

        public string GetPreviewUrl(AzureBucket bucket, string fileName)
        {
            return urlGenerator.GetPreviewUrl(bucket, fileName);
        }

        public void Upload(AzureBucket bucket, string fileName, byte[] content)
        {
            uploader.Upload(bucket, fileName, content);
        }

        public string GetUrl(AzureBucket bucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null)
        {
            log.Write(
                "{0}: bucket.Name = {1}, bucket.AccountName = {2}, bucket.PrimaryAccessKey = {3}, bucket.SecondaryAccessKey = {4}, filename = {5}, contentDispositionFileName = {6}",
                MethodBase.GetCurrentMethod().Name, bucket.Name, bucket.AccountName, bucket.PrimaryAccessKey, bucket.SecondaryAccessKey, fileName, contentDispositionFileName);

            return Exists(bucket, fileName) ? urlGenerator.GetUrl(bucket, fileName, expiryTime, contentDispositionFileName) : string.Empty;
        }

        public string GetUploadUrl(AzureBucket bucket, string fileName)
        {
            return urlGenerator.GetUploadUrl(bucket, fileName, DateTimeOffset.UtcNow.AddHours(24));
        }

        public bool Exists(AzureBucket bucket, string fileName)
        {
            var exists = existanceChecker.Exists(bucket, fileName);

            log.Write("Bucket exists = {0}", exists);

            return exists;
        }

        public IEnumerable<string> GetListOfFiles(AzureBucket bucket, string prefix = null)
        {
            return lister.ListObjectsInBucket(bucket, prefix);
        }
    }
}
