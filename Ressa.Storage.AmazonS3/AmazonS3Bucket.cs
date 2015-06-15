using System;

namespace Ressa.Storage.AmazonS3
{
    public class AmazonS3Bucket
    {
        public string Name { get; private set; }
        public string PublicKey { get; private set; }
        public string SecretKey { get; private set; }

        public AmazonS3Bucket(string name, string publicKey, string secretKey)
        {
            Name = name;
            PublicKey = publicKey;
            SecretKey = secretKey;
        }


    }
}
