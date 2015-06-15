using System;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Interfaces;
using Ressa.Testing.Extensions;

namespace Ressa.Storage.Tests.Azure
{
    public class ExistanceChecking
    {
        private IAzureFacade facade;
        private AzureBucket bucket;

        [SetUp]
        public void SetUp()
        {
            bucket = AzureBucketBuilder.NewTestBucket().Build();
            facade = AzureFacadeFactory.Create();
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
