using System;
using NUnit.Framework;
using Ressa.Storage.AmazonS3;
using Ressa.Testing.Extensions;

namespace Ressa.Storage.Tests.AmazonS3
{
    [TestFixture]
    public class S3DateTimeFormatterFixture
    {
        private DateTimeFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            formatter = new DateTimeFormatter();
        }

        [Test]
        public void Should_return_a_date_formatted_as_total_seconds()
        {
            var dateTime = new DateTimeOffset(1970, 1, 1, 0, 25, 0, default(TimeSpan));

            var result = formatter.GetDateAsString(dateTime);

            result.ShouldEqual("1500");
        }

        [Test]
        public void Should_ignore_parts_of_the_date_smaller_than_seconds()
        {
            var dateTime = new DateTimeOffset(1970, 1, 1, 0, 25, 0, 14, default(TimeSpan));

            var result = formatter.GetDateAsString(dateTime);

            result.ShouldEqual("1500");
        }

        [Test]
        public void Should_ignore_ticks()
        {
            DateTimeOffset dateTime = new DateTimeOffset(1970, 1, 1, 0, 25, 0, 14, default(TimeSpan));
            dateTime = dateTime.AddTicks(17);

            string result = formatter.GetDateAsString(dateTime);

            result.ShouldEqual("1500");
        }
    }
}
