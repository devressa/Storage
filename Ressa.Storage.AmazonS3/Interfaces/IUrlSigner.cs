using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IUrlSigner
    {
        string Sign(AmazonS3Bucket bucket, Uri toSign);
    }
}
