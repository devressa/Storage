using System;
using NUnit.Framework;
using Ressa.Storage.Sftp;

namespace Ressa.Storage.Tests.Sftp
{
    [TestFixture]
    public class When_parsing_host
    {
        [Test]
        [TestCase("localhost", "localhost")]
        [TestCase("localhost:230", "localhost")]
        [TestCase("localhost:230/folder/uri?q=123&a=3", "localhost")]
        public void Should_return_expected_host(string url, string expectedHost)
        {
            var parser = new SftpUrlParser();
            var host = parser.GetHost(url);
            Assert.AreEqual(expectedHost, host);
        }
    }
}
