using System;
using NUnit.Framework;
using Ressa.Storage.AmazonS3;
using Ressa.Storage.AmazonS3.Interfaces;
using Ressa.Storage.Tests.AmazonS3;

namespace Ressa.Storage.Tests.Integration.AmazonS3
{
    [TestFixture]
    abstract class AmazonS3FacadeFixture
    {
        protected IAmazonS3Facade Facade;
        protected AmazonS3Bucket Bucket;
        protected string Prefix;
        protected string Key;
        protected string FileName;

        [SetUp]
        public virtual void SetUp()
        {
            Facade = AmazonS3FacadeFactory.Create();
            Prefix = string.Format("{0}", Guid.NewGuid());
            Key = Guid.NewGuid().ToString();
            FileName = string.Format("{0}-{1}", Prefix, Key);
            Bucket = AmazonS3BucketBuilder.NewTestBucket().Build();
        }
    }
}
