using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IExistanceChecker
    {
        bool Exists(AzureBucket bucket, string fileName);
    }
}
