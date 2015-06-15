using System;
using System.IO;
using NUnit.Framework;

namespace Ressa.Storage.Tests.Integration.Sftp
{
    [TestFixture]
    public class When_working_with_directories : ClientFixtureBase
    {
        [Test]
        public void Should_return_false_when_directory_not_exists()
        {
            var exists = SftpClient.Exists(Guid.NewGuid().ToString());
            Assert.IsFalse(exists);
        }

        [Test]
        public void Should_return_true_when_directory_exists()
        {
            var expectedFolder = Guid.NewGuid().ToString();
            var folder = Path.Combine(SftpFolder, expectedFolder);
            Directory.CreateDirectory(folder);

            var exists = SftpClient.Exists(expectedFolder);

            Assert.IsTrue(exists);
        }

        [Test]
        public void Should_create_the_folder()
        {
            var expectedFolder = Guid.NewGuid().ToString();
            SftpClient.CreateDirectory(expectedFolder);
            Assert.IsTrue(Directory.Exists(Path.Combine(SftpFolder, expectedFolder)));
        }
    }
}
