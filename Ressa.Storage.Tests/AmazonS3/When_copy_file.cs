using System;
using NUnit.Framework;
using Ressa.Storage.AmazonS3;
using Ressa.Storage.AmazonS3.Interfaces;
using Rhino.Mocks;

namespace Ressa.Storage.Tests.AmazonS3
{
    [TestFixture]
    class When_copy_file
    {
        private const string SourceFile = "source.src";
        private const string TargetFile = "target.trgt";
        private AmazonS3Bucket sourceBucket;
        private AmazonS3Bucket targetBucket;

        [SetUp]
        public void TestFixtureSetup()
        {
            sourceBucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.source").Build();
            targetBucket = AmazonS3BucketBuilder.NewTestBucket().WithName("ressa.storage.tests.target").Build();

        }

        [Test]
        public void Should_provide_all_arguments_to_amazon_s3_client_copy_method()
        {
            var configuartionManager = MockRepository.GenerateStub<IConfigurationManagerWrapper>();
            var amazonS3ClientWrapper = MockRepository.GenerateMock<IAmazonS3ClientWrapper>();
            var amazonS3Factory = MockRepository.GenerateStub<IAmazonS3ClientFactory>();
            amazonS3Factory.Stub(x => x.CreateInstance(Arg<AmazonS3Bucket>.Is.Equal(targetBucket))).Return(amazonS3ClientWrapper);
            var uploader = new Uploader(null, configuartionManager, amazonS3Factory, null);

            uploader.Copy(sourceBucket, SourceFile, targetBucket, TargetFile);

            amazonS3ClientWrapper.AssertWasCalled(x => x.Copy(
                    Arg<string>.Is.Equal(sourceBucket.Name), Arg<string>.Is.Equal(SourceFile),
                    Arg<string>.Is.Equal(targetBucket.Name), Arg<string>.Is.Equal(TargetFile)
                    ));
        }

        [Test]
        public void Should_provide_all_arguments_to_uploader_copy_method()
        {
            var uploader = MockRepository.GenerateStub<IUploader>();
            var facade = new AmazonS3Facade(uploader, null, null, null, null);

            facade.Copy(sourceBucket, SourceFile, targetBucket, TargetFile);

            uploader.AssertWasCalled(x => x.Copy(
                    Arg<AmazonS3Bucket>.Is.Equal(sourceBucket), Arg<string>.Is.Equal(SourceFile),
                    Arg<AmazonS3Bucket>.Is.Equal(targetBucket), Arg<string>.Is.Equal(TargetFile)
                    ));
        }
    }
}
