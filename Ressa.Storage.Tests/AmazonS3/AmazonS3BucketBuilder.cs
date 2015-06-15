using System;
using Ressa.Storage.AmazonS3;

namespace Ressa.Storage.Tests.AmazonS3
{
    public class AmazonS3BucketBuilder
    {
        public const string BucketNameNotExist = "not exist bucket";
        public const int MaximumTimeDifference = 5;

        private string name;
        private string publicKey;
        private string secretKey;

        public static AmazonS3BucketBuilder New()
        {
            return new AmazonS3BucketBuilder();
        }

        public static AmazonS3BucketBuilder NewTestBucket()
        {
            var bucket = new AmazonS3BucketBuilder();

            bucket.WithName("ressa.storage.tests.testbucket")
                  .WithPublicKey("")
                  .WithSecretKey("");

            return bucket;
        }

        public AmazonS3BucketBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public AmazonS3BucketBuilder WithPublicKey(string publicKey)
        {
            this.publicKey = publicKey;
            return this;
        }

        public AmazonS3BucketBuilder WithSecretKey(string secretKey)
        {
            this.secretKey = secretKey;
            return this;
        }

        public AmazonS3Bucket Build()
        {
            return new AmazonS3Bucket(name, publicKey, secretKey);
        }

        public static AmazonS3Bucket NewNotExitBucket()
        {
            return NewTestBucket().WithName(BucketNameNotExist).Build();
        }
    }
}
