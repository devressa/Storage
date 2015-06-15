using System;

namespace Ressa.Storage.AmazonS3.Interfaces
{
    public interface IDateTimeFormatter
    {
        string GetDateAsString(DateTimeOffset dateTime);
    }
}
