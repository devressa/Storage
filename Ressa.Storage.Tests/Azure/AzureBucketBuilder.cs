using System;
using Ressa.Storage.Azure;

namespace Ressa.Storage.Tests.Azure
{
    public class AzureBucketBuilder
    {
        private const string accountName = "ressatesting";
        private string name;
        private string publicKey;
        private string secretKey;

        public static AzureBucketBuilder New()
        {
            return new AzureBucketBuilder();
        }

        public static AzureBucketBuilder NewTestBucket()
        {
            var bucket = new AzureBucketBuilder();

            bucket.WithName("testbucket")
                .WithPublicKey("")
                .WithSecretKey("");

            return bucket;
        }

        public const string BucketNameWithOneItem = "bucketwithoneitem";
        public const string BucketNameToCopyTest = "testbucket2";
        public const string BucketNameNotExist = "not exist bucket";
        public const int MaximumTimeDifference = 5;

        public AzureBucketBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public AzureBucketBuilder WithPublicKey(string publicKey)
        {
            this.publicKey = publicKey;
            return this;
        }

        public AzureBucketBuilder WithSecretKey(string secretKey)
        {
            this.secretKey = secretKey;
            return this;
        }

        public AzureBucket Build()
        {
            return new AzureBucket(accountName, name, publicKey, secretKey);
        }

        public static AzureBucket NewNotExitBucket()
        {
            return NewTestBucket().WithName(BucketNameNotExist).Build();
        }

        public static AzureBucket NewTestCopyBucket()
        {
            return NewTestBucket().WithName(BucketNameToCopyTest).Build();
        }
    }
}
