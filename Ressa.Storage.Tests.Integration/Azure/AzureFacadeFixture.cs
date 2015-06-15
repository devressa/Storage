using System;
using NUnit.Framework;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Interfaces;
using Ressa.Storage.Tests.Azure;

namespace Ressa.Storage.Tests.Integration.Azure
{
    [TestFixture]
    public abstract class AzureFacadeFixture
    {
        protected IAzureFacade Facade;
        protected AzureBucket Bucket;
        protected string Prefix;
        protected string Key;
        protected string FileName;

        [SetUp]
        public virtual void SetUp()
        {
            Facade = AzureFacadeFactory.Create();
            Prefix = string.Format("{0}", Guid.NewGuid());
            Key = Guid.NewGuid().ToString();
            FileName = string.Format("{0}-{1}", Prefix, Key);
            Bucket = AzureBucketBuilder.NewTestBucket().Build();
        }
    }
}
