using System;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class DateTimeFormatter : IDateTimeFormatter
    {
        public string GetDateAsString(DateTimeOffset dateTime)
        {
            var totalSeconds = (dateTime - new DateTimeOffset(1970, 1, 1, 0, 0, 0, default(TimeSpan))).TotalSeconds;
            return Math.Floor(totalSeconds).ToString();
        }
    }
}
