using System;
using System.Linq;
using NUnit.Framework;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.Tests.AmazonS3
{
    [TestFixture]
    public class When_retrieving_list_of_files_in_bucket
    {
        private IAmazonS3Facade facade;

        [SetUp]
        public void SetUp()
        {
            facade = AmazonS3FacadeFactory.Create();
        }

        [Test]
        public void Should_return_filenames_of_each_file_in_the_bucket()
        {
            var prefix = string.Format("{0}/", Guid.NewGuid());
            var key = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}{1}", prefix, key);
            var bucket = AmazonS3BucketBuilder.NewTestBucket().Build();
            facade.Upload(bucket, fileName, new[] { (byte)1 });
            var s3Files = facade.GetListOfFiles(bucket, prefix);
            Assert.That(s3Files.Where(file => file == fileName).FirstOrDefault(), Is.Not.Null);
        }

        [Test]
        public void Should_return_files_in_specfied_bucket()
        {
            var bucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.bucketwithoneitem").Build();
            var s3Files = facade.GetListOfFiles(bucket);
            Assert.That(s3Files.Count(), Is.EqualTo(1));
        }
    }
}
