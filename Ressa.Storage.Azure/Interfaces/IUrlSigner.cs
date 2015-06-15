using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IUrlSigner
    {
        string Sign(AzureBucket bucket, Uri toSign);
    }
}
