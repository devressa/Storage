using System;
using NUnit.Framework;
using Ressa.Storage.AmazonS3.Exceptions;
using Ressa.Storage.AmazonS3.Interfaces;
using Ressa.Testing.Extensions;

namespace Ressa.Storage.Tests.AmazonS3
{
    [TestFixture]
    public class When_getting_file_details
    {
        private IAmazonS3Facade facade;

        [SetUp]
        public void SetUp()
        {
            facade = AmazonS3FacadeFactory.Create();
        }

        [Test]
        public void Should_be_able_to_get_the_details()
        {
            var bucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.bucketwithoneitem").Build();

            var fileDetails = facade.GetFileDetails(bucket, "MyTest.htm");

            fileDetails.BucketName.ShouldEqual("ressa.storage.tests.bucketwithoneitem");
            fileDetails.ETag.ShouldEqual("\"\"");
            fileDetails.Filename.ShouldEqual("MyTest.htm");
            fileDetails.LastModified.ShouldEqual(new DateTime(2011, 06, 23, 18, 16, 01, DateTimeKind.Utc).ToLocalTime());
            fileDetails.SizeInBytes.ShouldEqual(393);
        }

        [Test, ExpectedException(typeof(AmazonS3FileNotFoundException))]
        public void Should_throw_if_the_file_does_not_exist()
        {
            var bucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.bucketwithoneitem").Build();
            facade.GetFileDetails(bucket, "a_file_that_does_not_exist.txt");
        }

        [Test, ExpectedException(typeof(AmazonS3Exception))]
        public void Should_throw_if_the_bucket_does_not_exist()
        {
            var bucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.a_bucket_that_does_not_exist").Build();
            facade.GetFileDetails(bucket, "a_file_that_does_not_exist.txt");
        }
    }
}
