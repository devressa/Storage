using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class AzureBuilder : IAzureBuilder
    {
        public CloudBlobClient BuildClient(AzureBucket bucket)
        {
            CloudStorageAccount account;
            var accountName = bucket.AccountName;

            if (accountName.Contains("."))
            {
                var queueStorageAccountName = accountName.Replace(".blob.", ".queue.");
                var tableStorageName = accountName.Replace(".blob.", ".table.");
                var storageCredentials = new StorageCredentials(accountName.Substring(0, accountName.IndexOf(".", StringComparison.Ordinal)), bucket.PrimaryAccessKey);

                account = new CloudStorageAccount(storageCredentials, GetHttpsUri(bucket.AccountName), GetHttpsUri(queueStorageAccountName), GetHttpsUri(tableStorageName));
            }
            else
            {
                account = new CloudStorageAccount(new StorageCredentials(accountName, bucket.PrimaryAccessKey), true);
            }

            return account.CreateCloudBlobClient();
        }

        private static Uri GetHttpsUri(string accountName)
        {
            return new Uri(string.Format("https://{0}", accountName));
        }

        public CloudBlobContainer BuildContainer(AzureBucket bucket)
        {
            return BuildClient(bucket).GetContainerReference(bucket.Name);
        }
    }
}
