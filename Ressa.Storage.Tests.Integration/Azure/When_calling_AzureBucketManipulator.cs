using System;
using NUnit.Framework;
using Ressa.Storage.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    internal class When_calling_AzureBucketManipulator
    {
        private AzureBucketManipulator azureBucketManipulator;
        private const string accountName = "ressatesting";
        private const string primaryAccessKey = "";
        
        [SetUp]
        public void SetUp()
        {
            azureBucketManipulator = new AzureBucketManipulator(accountName, primaryAccessKey);
        }

        [Test]
        public void Should_check_bucket_exist_and_return_true_if_exist()
        {
            var bucketName = Guid.NewGuid().ToString();
            try
            {
                azureBucketManipulator.CreateBucket(bucketName);
                var exist = azureBucketManipulator.CheckIfBucketExists(bucketName);
                Assert.IsTrue(exist);
            }
            finally
            {
                azureBucketManipulator.DeleteBucket(bucketName);
            }
        }

        [Test]
        public void Should_check_bucket_exist_and_return_false_if_not_exist()
        {
            var bucketName = Guid.NewGuid().ToString();
            var exist = azureBucketManipulator.CheckIfBucketExists(bucketName);
            Assert.IsFalse(exist);
        }
        
        [Test]
        public void Should_create_bucket_and_return_true_if_created()
        {
            var bucketName = Guid.NewGuid().ToString();
            try
            {
                var result = azureBucketManipulator.CreateBucket(bucketName);
                var exist = azureBucketManipulator.CheckIfBucketExists(bucketName);
                Assert.IsTrue(result);
                Assert.IsTrue(exist);
            }
            finally
            {
                azureBucketManipulator.DeleteBucket(bucketName);
            }
        }

        [Test]
        public void Should_return_false_if_when_creating_bucket_with_name_that_already_exist()
        {

            var bucketName = Guid.NewGuid().ToString();
            try
            {
                azureBucketManipulator.CreateBucket(bucketName);
                var result = azureBucketManipulator.CreateBucket(bucketName);
                Assert.IsFalse(result);
            }
            finally
            {
                azureBucketManipulator.DeleteBucket(bucketName);
            }
        }
        
        [Test]
        public void Should_delete_bucket_and_return_true_if_deleted()
        {
            var bucketName = Guid.NewGuid().ToString();
            try
            {
                azureBucketManipulator.CreateBucket(bucketName);
                var result = azureBucketManipulator.DeleteBucket(bucketName);
                Assert.IsTrue(result);
            }
            finally
            {
                azureBucketManipulator.DeleteBucket(bucketName);
            }
        }

        [Test]
        public void Should_return_false_if_bucket_not_exist()
        {
            var bucketName = Guid.NewGuid().ToString();
            var result = azureBucketManipulator.DeleteBucket(bucketName);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void Should_return_bucket()
        {
            var bucketName = Guid.NewGuid().ToString();
            try
            {
                azureBucketManipulator.CreateBucket(bucketName);
                var result = azureBucketManipulator.GetBucket(bucketName);
                Assert.NotNull(result);
            }
            finally
            {
                azureBucketManipulator.DeleteBucket(bucketName);
            }
        }
    }
}
