using System;
using NUnit.Framework;
using Ressa.Storage.Sftp;

namespace Ressa.Storage.Tests.Sftp
{
    [TestFixture]
    public class When_parsing_port
    {
        [Test]
        [TestCase("localhost", 22)]
        [TestCase("localhost:230", 230)]
        [TestCase("localhost:230/folder/uri?q=123&a=3", 230)]
        public void Should_return_expected_port(string url, int expectedPort)
        {
            var parser = new SftpUrlParser();
            var port = parser.GetPort(url);
            Assert.AreEqual(expectedPort, port);
        }
    }
}
