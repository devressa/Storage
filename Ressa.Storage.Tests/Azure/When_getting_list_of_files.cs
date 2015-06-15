using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Interfaces;
using Rhino.Mocks;

namespace Ressa.Storage.Tests.Azure
{
    [TestFixture]
    public class When_getting_list_of_files
    {
        private IClient client;
        private AzureBucket bucket;
        private IEnumerable<CloudBlockBlob> listBucketresult;

        [SetUp]
        public void SetUp()
        {
            client = MockRepository.GenerateMock<IClient>();
            bucket = AzureBucketBuilder.NewTestBucket().Build();
            listBucketresult = new List<CloudBlockBlob>();
        }

        [Test]
        public void Should_ask_client_for_listofobjects()
        {
            client.Expect(c => c.ListBucket(null)).Return(listBucketresult);
            var lister = new Lister(client);
            lister.ListObjectsInBucket(null, null);
            client.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_empty_list_no_files_returned_from_azure()
        {
            client.Stub(c => c.ListBucket(bucket)).Return(listBucketresult);
            var lister = new Lister(client);
            var files = lister.ListObjectsInBucket(bucket, null);
            Assert.That(files, Is.EquivalentTo(Enumerable.Empty<string>()));
        }
    }
}
