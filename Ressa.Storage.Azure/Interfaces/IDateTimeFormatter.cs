using System;

namespace Ressa.Storage.Azure.Interfaces
{
    public interface IDateTimeFormatter
    {
        string GetDateAsString(DateTimeOffset dateTime);
    }
}
