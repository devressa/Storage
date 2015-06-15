using System;
using System.Linq;
using NUnit.Framework;
using Ressa.Storage.AmazonS3;
using Ressa.Storage.AmazonS3.Interfaces;
using Rhino.Mocks;

namespace Ressa.Storage.Tests.AmazonS3
{
    [TestFixture]
    public class When_get_list_of_files
    {
        private AmazonS3Facade s3Facade;
        private ILister lister;

        [SetUp]
        public void SetUp()
        {
            lister = MockRepository.GenerateMock<ILister>();
            IUploader uploader = null;
            IUrlGenerator urlGenerator = null;
            IExistanceChecker existanceChecker = null;

            var signer = new Signer();
            var client = new Client(signer);

            s3Facade = new AmazonS3Facade(uploader, urlGenerator, existanceChecker, lister, client);
        }

        [Test]
        public void Should_get_list_from_lister()
        {
            lister.Expect(l => l.ListObjectsInBucket(null, null));

            s3Facade.GetListOfFiles(null);

            lister.VerifyAllExpectations();
        }

        [Test]
        public void Should_pass_search_to_client_as_prefix()
        {
            const string prefix = "myFolder";
            lister.Expect(l => l.ListObjectsInBucket(null, prefix));

            s3Facade.GetListOfFiles(null, prefix);

            lister.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_files_starting_with_keyword()
        {
            const string prefix = "my";
            var s3Files = Enumerable.Range(1, 10).Select(i => string.Format("myfile{0}.pdf", i));
            lister.Expect(l => l.ListObjectsInBucket(null, prefix)).Return(s3Files);

            s3Facade.GetListOfFiles(null, prefix);

            lister.VerifyAllExpectations();
        }
    }
}
