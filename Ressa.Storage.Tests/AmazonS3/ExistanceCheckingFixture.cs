using System;
using NUnit.Framework;
using Ressa.Storage.AmazonS3;
using Ressa.Storage.AmazonS3.Interfaces;
using Ressa.Testing.Extensions;

namespace Ressa.Storage.Tests.AmazonS3
{
    public class ExistanceCheckingFixture
    {
        private IAmazonS3Facade facade;
        private AmazonS3Bucket bucket;

        [SetUp]
        public void SetUp()
        {
            bucket = AmazonS3BucketBuilder.NewTestBucket().Build();
            facade = AmazonS3FacadeFactory.Create();
        }

        [Test]
        public void Should_return_true_if_the_file_exists()
        {
            var fileName = GetTempFileName();

            facade.Upload(bucket, fileName, new[] { (byte)1 });

            facade.Exists(bucket, fileName).ShouldBeTrue();
        }

        [Test]
        public void Should_return_false_if_the_file_does_not_exist()
        {
            facade.Exists(bucket, GetTempFileName()).ShouldBeFalse();
        }

        private static string GetTempFileName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
