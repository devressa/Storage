using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class AzureBucketManipulator : IAzureBucketManipulator
    {
        private readonly CloudBlobClient client;

        public AzureBucketManipulator(string accountName, string primaryAccessKey)
        {
            var account = new CloudStorageAccount(new StorageCredentials(accountName, primaryAccessKey), true);
            client = account.CreateCloudBlobClient();
        }

        public bool CreateBucket(string name)
        {
            var container = GetBucket(name);
            return container.CreateIfNotExists();
        }

        public bool CheckIfBucketExists(string name)
        {
            var container = GetBucket(name);
            return container.Exists();
        }

        public bool DeleteBucket(string name)
        {
            var container = GetBucket(name);
            return container.DeleteIfExists();
        }

        public IEnumerable<string> GetAllBucketsName()
        {
            return client.ListContainers().Select(cont => cont.Name);
        }

        public CloudBlobContainer GetBucket(string name)
        {
            return client.GetContainerReference(name);
        }
    }
}
