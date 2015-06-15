using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface ISigner
    {
        string Sign(AzureBucket bucket, string toSign);
    }
}
