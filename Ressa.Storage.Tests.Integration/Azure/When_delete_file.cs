using System;
using NUnit.Framework;
using Ressa.Storage.Tests.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    class When_delete_file : AzureFacadeFixture
    {
        [Test]
        public void Should_delete_file_and_return_true_if_exist()
        {
            Facade.Upload(Bucket, FileName, new[] { (byte)1 });
            var result = Facade.Delete(Bucket, FileName);
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_delete_file()
        {
            Facade.Upload(Bucket, FileName, new[] { (byte)1 });
            Facade.Delete(Bucket, FileName);
            Assert.IsFalse(Facade.Exists(Bucket, Prefix));

        }

        [Test]
        public void Should_return_false_if_delete_not_exist_file()
        {
            var result = Facade.Delete(Bucket, FileName);
            Assert.IsFalse(result);
        }

        [Test]
        public void Should_return_false_if_delete_from_not_exist_bucket()
        {
            var result = Facade.Delete(AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameNotExist).Build(), FileName);
            Assert.IsFalse(result);
        }
    }
}
