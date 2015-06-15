using System;
using Microsoft.WindowsAzure.Storage.Blob;
using NUnit.Framework;
using Ressa.Storage.Azure;

namespace Ressa.Storage.Tests.Azure
{
    [TestFixture]
    public class When_building_an_azure_client
    {
        private const string DefaultAccountName = "DefaultAccountName";
        private const string DefaultContainerName = "DefaultContainerName";
        private static readonly string DefaultPrimaryAccessKey = Base64Encode("DefaultPrimaryAccessKey");
        private static readonly string DefaultSecondaryAccessKey = Base64Encode("DefaultSecondaryAccessKey");

        [TestCase("MyAccount", "MyAccount", Description = "Should set the storage credentials account name when name has no full-stop symbols")]
        [TestCase("MyAccount.blob.something", "MyAccount", Description = "Should set the storage credentials account name up to first full-stop symbool if when name has full-stop symbols")]
        public void Should_set_the_storage_credentials_account_name(string accountName, string expectedAccountName)
        {
            var actualAzureClient = BuildClient(accountName, DefaultContainerName, DefaultPrimaryAccessKey, DefaultSecondaryAccessKey);

            Assert.AreEqual(expectedAccountName, actualAzureClient.Credentials.AccountName);
        }

        [Test]
        public void Should_set_the_storage_credentials_primary_access_key()
        {
            var actualAzureClient = BuildClient(DefaultAccountName, DefaultContainerName, DefaultPrimaryAccessKey, DefaultSecondaryAccessKey);
            Assert.AreEqual(DefaultAccountName, actualAzureClient.Credentials.AccountName);
        }

        [Test]
        public void Should_set_base_uri_to_https()
        {
            var actualAzureClient = BuildClient(DefaultAccountName, DefaultContainerName, DefaultPrimaryAccessKey, DefaultSecondaryAccessKey);
            StringAssert.StartsWith("https://", actualAzureClient.BaseUri.ToString());
        }

        [Test]
        public void Should_set_storage_uri_to_https()
        {
            var actualAzureClient = BuildClient(DefaultAccountName, DefaultContainerName, DefaultPrimaryAccessKey, DefaultSecondaryAccessKey);
            StringAssert.Contains("https://", actualAzureClient.StorageUri.ToString());
        }

        private static CloudBlobClient BuildClient(string accountName, string containerName, string primaryAccessKey, string secondaryAccessKey)
        {
            return new AzureBuilder().BuildClient(new AzureBucket(accountName, containerName, primaryAccessKey, secondaryAccessKey));
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
