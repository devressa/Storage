using System;
using NUnit.Framework;
using Ressa.Storage.Azure.Exceptions;
using Ressa.Storage.Azure.Interfaces;
using Ressa.Storage.Tests.Azure;
using Ressa.Testing.Extensions;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    public class When_getting_file_details
    {
        private IAzureFacade facade;

        [SetUp]
        public void SetUp()
        {
            facade = AzureFacadeFactory.Create();
        }

        [Test]
        public void Should_be_able_to_get_the_details()
        {
            var bucket = AzureBucketBuilder.NewTestBucket().WithName("bucketwithoneitem").Build();

            var fileDetails = facade.GetFileDetails(bucket, "MyTest.htm");

            fileDetails.BucketName.ShouldEqual(AzureBucketBuilder.BucketNameWithOneItem);
            fileDetails.Filename.ShouldEqual("MyTest.htm");
            fileDetails.LastModified.ShouldEqual(new DateTime(2013, 06, 4, 7, 57, 20));
            fileDetails.SizeInBytes.ShouldEqual(18);
        }

        [Test]
        public void Should_throw_if_the_file_does_not_exist()
        {
            var bucket = AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameWithOneItem).Build();
            Assert.Throws<AzureFileNotFoundException>(() => facade.GetFileDetails(bucket, "a_file_that_does_not_exist.txt"));
        }

        [Test]
        public void Should_throw_if_the_bucket_does_not_exist()
        {
            var bucket = AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameNotExist).Build();
            Assert.Throws<AzureFileNotFoundException>(() => facade.GetFileDetails(bucket, "afilethatdoesnotexist.txt"));
        }
    }
}
