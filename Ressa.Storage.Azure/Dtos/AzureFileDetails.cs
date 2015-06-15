using System;

namespace Ressa.Storage.Azure.Dtos
{
    public class AzureFileDetails
    {
        public bool IsFound { get; set; }
        public string Filename { get; set; }
        public long SizeInBytes { get; set; }
        public string BucketName { get; set; }
        public string ETag { get; set; }
        public DateTime LastModified { get; set; }
    }
}
