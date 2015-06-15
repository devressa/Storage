using System;
using NUnit.Framework;
using Ressa.Storage.Tests.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    class When_check_file_exist : AzureFacadeFixture
    {
        [Test]
        public void Should_check_file_exist_and_return_true_if_exist()
        {
            try
            {
                Facade.Upload(Bucket, FileName, new[] { (byte)1 });
                var exist = Facade.Exists(Bucket, FileName);
                Assert.IsTrue(exist);
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_check_file_exist_and_return_false_if_not_exist()
        {
            var exist = Facade.Exists(Bucket, Prefix);
            Assert.IsFalse(exist);
        }
        
        [Test]
        public void Should_check_file_exist_and_return_false_if_bucket_not_exist()
        {
            var result = Facade.Delete(AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameNotExist).Build(), FileName);
            Assert.IsFalse(result);
        }
    }
}
