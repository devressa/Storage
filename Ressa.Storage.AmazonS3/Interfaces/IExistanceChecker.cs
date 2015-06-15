using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IExistanceChecker
    {
        bool Exists(AmazonS3Bucket bucket, string fileName);
    }
}
