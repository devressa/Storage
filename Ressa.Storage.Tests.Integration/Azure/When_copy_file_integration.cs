using System;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Exceptions;
using Ressa.Storage.Tests.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    class When_copy_file_integration : AzureFacadeFixture
    {
        private AzureBucket targetBucket;
        private string targetFileName;

        public override void SetUp()
        {
            base.SetUp();
            targetBucket = AzureBucketBuilder.NewTestCopyBucket();
            targetFileName = "target_" + FileName;
        }

        [Test]
        public void Should_copy_file()
        {
            try
            {
                Facade.Upload(Bucket, FileName, new[] { (byte)1 });
                Facade.Copy(Bucket, FileName, targetBucket, targetFileName);
                Assert.IsTrue(Facade.Exists(targetBucket, targetFileName));
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
                Facade.Delete(targetBucket, targetFileName);
            }
        }

        [Test]
        public void Should_copy_file_and_save_metadata()
        {
            try
            {
                var now = DateTime.UtcNow;
                Facade.Upload(Bucket, FileName, new[] { (byte)1 });

                Facade.Copy(Bucket, FileName, targetBucket, targetFileName);

                var details = Facade.GetFileDetails(targetBucket, targetFileName);
                Assert.AreEqual(targetBucket.Name, details.BucketName);
                Assert.AreEqual(targetFileName, details.Filename);
                Assert.AreEqual(1, details.SizeInBytes);
                Assert.LessOrEqual(Math.Abs((details.LastModified - now).Minutes), AzureBucketBuilder.MaximumTimeDifference);
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
                Facade.Delete(targetBucket, targetFileName);
            }
        }

        [Test]
        public void Should_throw_exception_if_source_file_not_exist()
        {
            Assert.Throws<AzureException>(() => Facade.Copy(Bucket, "not exist file", targetBucket, targetFileName));
        }

        [Test]
        public void Should_throw_exception_if_source_bucket_not_exist()
        {
            Assert.Throws<AzureException>(() => Facade.Copy(AzureBucketBuilder.NewNotExitBucket(), FileName, targetBucket, targetFileName));
        }

        [Test]
        public void Should_throw_exception_if_destination_bucket_not_exist()
        {
            Assert.Throws<AzureException>(() => Facade.Copy(Bucket, FileName, AzureBucketBuilder.NewNotExitBucket(), targetFileName));
        }

    }
}
