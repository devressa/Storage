using System;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Interfaces;
using Ressa.Storage.Tests.Azure;
using Rhino.Mocks;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    class When_copy_file
    {
        private const string SourceFile = "source.src";
        private const string TargetFile = "target.trgt";
        private AzureBucket sourceBucket;
        private AzureBucket targetBucket;

        [SetUp]
        public void TestFixtureSetup()
        {
            sourceBucket = AzureBucketBuilder.NewTestBucket().WithName("ressa.storage.tests.source").Build();
            targetBucket = AzureBucketBuilder.NewTestBucket().WithName("ressa.storage.tests.target").Build();

        }

        [Test]
        public void Should_provide_all_arguments_to_azure_client_copy_method()
        { 
            var configuartionManager = MockRepository.GenerateStub<IConfigurationManagerWrapper>();
            var amazonClientWrapper = MockRepository.GenerateMock<IAzureClientWrapper>();
            var amazonFactory = MockRepository.GenerateStub<IAzureClientFactory>();
            amazonFactory.Stub(x => x.CreateInstance(Arg<AzureBucket>.Is.Equal(targetBucket))).Return(amazonClientWrapper);
            var uploader = new Uploader(null, configuartionManager, amazonFactory, null);

            uploader.Copy(sourceBucket, SourceFile, targetBucket, TargetFile);

            amazonClientWrapper.AssertWasCalled(x => x.Copy(
                    Arg<string>.Is.Equal(sourceBucket.Name), Arg<string>.Is.Equal(SourceFile),
                    Arg<string>.Is.Equal(targetBucket.Name), Arg<string>.Is.Equal(TargetFile)
                    ));
        }

        [Test]
        public void Should_provide_all_arguments_to_uploader_copy_method()
        {
            var uploader = MockRepository.GenerateStub<IUploader>();
            var facade = new AzureFacade(uploader, null, null, null, null);

            facade.Copy(sourceBucket, SourceFile, targetBucket, TargetFile);

            uploader.AssertWasCalled(x => x.Copy(
                    Arg<AzureBucket>.Is.Equal(sourceBucket), Arg<string>.Is.Equal(SourceFile),
                    Arg<AzureBucket>.Is.Equal(targetBucket), Arg<string>.Is.Equal(TargetFile)
                    ));
        }

    }
}
