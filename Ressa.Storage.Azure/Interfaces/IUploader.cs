using System;
using System.IO;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IUploader
    {
        void Upload(AzureBucket bucket, string fileName, byte[] content);
        bool Upload(AzureBucket bucket, string fileName, Stream stream);
        bool Delete(AzureBucket bucket, string filename);
        void Copy(AzureBucket sourceBucket, string sourceFileName, AzureBucket targetBucket, string targetFileName);
    }
}
