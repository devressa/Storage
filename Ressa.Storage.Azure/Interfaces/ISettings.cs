using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface ISettings
    {
        string BucketName { get; set; }
        string PublicKey { get; }
        string SecretKey { get; }
    }
}
