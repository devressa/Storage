using System;
using System.Linq;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Tests.Azure;
using Ressa.Storage.Azure.Interfaces;
using Rhino.Mocks;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    public class When_get_list_of_files
    {
        private AzureFacade facade;
        private ILister lister;

        [SetUp]
        public void SetUp()
        {
            lister = MockRepository.GenerateMock<ILister>();
            IUploader uploader = null;
            IUrlGenerator urlGenerator = null;
            IExistanceChecker existanceChecker = null;

            var builder = new AzureBuilder();
            var client = new Client(builder);

            facade = new AzureFacade(uploader, urlGenerator, existanceChecker, lister, client);
        }

        [Test]
        public void Should_get_list_from_lister()
        {
            lister.Expect(l => l.ListObjectsInBucket(null, null));
            facade.GetListOfFiles(null);
            lister.VerifyAllExpectations();
        }

        [Test]
        public void Should_pass_search_to_client_as_prefix()
        {
            const string prefix = "myFolder";
            lister.Expect(l => l.ListObjectsInBucket(null, prefix));
            facade.GetListOfFiles(null, prefix);
            lister.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_files_starting_with_keyword()
        {
            const string prefix = "my";
            var files = Enumerable.Range(1, 10).Select(i => string.Format("myfile{0}.pdf", i));
            lister.Expect(l => l.ListObjectsInBucket(null, prefix)).Return(files);
            facade.GetListOfFiles(null, prefix);
            lister.VerifyAllExpectations();
        }

        [Test]
        public void Should_be_able_to_get_list_of_files()
        {
            var bucket = AzureBucketBuilder.NewTestBucket().WithName(AzureBucketBuilder.BucketNameWithOneItem).Build();
            var files = AzureFacadeFactory.Create().GetListOfFiles(bucket);
            Assert.AreEqual(1, files.Count());
        }
    }
}
