using System;

namespace Ressa.Storage.AmazonS3.Dtos
{
    public class AmazonS3FileDetails
    {
        public bool IsFound { get; set; }
        public string Filename { get; set; }
        public long SizeInBytes { get; set; }
        public string BucketName { get; set; }
        public string ETag { get; set; }
        public DateTime LastModified { get; set; }
    }
}
