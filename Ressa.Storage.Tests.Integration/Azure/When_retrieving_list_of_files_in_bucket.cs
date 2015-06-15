using System;
using System.Linq;
using NUnit.Framework;
using Ressa.Storage.Tests.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    public class When_retrieving_list_of_files_in_bucket : AzureFacadeFixture
    {
        [Test]
        public void Should_return_filenames_of_each_file_in_the_bucket()
        {
            try
            {
                Facade.Upload(Bucket, FileName, new[] { (byte)1 });
                var files = Facade.GetListOfFiles(Bucket, Prefix);
                Assert.That(files.FirstOrDefault(file => file == FileName), Is.Not.Null);
            }
            finally
            {
                Facade.Delete(Bucket, FileName);
            }
        }

        [Test]
        public void Should_return_files_in_specfied_bucket()
        {
            var azureBucket = AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameWithOneItem).Build();
            var files = Facade.GetListOfFiles(azureBucket);
            Assert.That(files.Count(), Is.EqualTo(1));
        }
    }
}
