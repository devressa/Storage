using System;
using System.IO;
using NUnit.Framework;

namespace Ressa.Storage.Tests.Integration.Sftp
{
    [TestFixture]
    public class When_copy_file_to_sftp : ClientFixtureBase
    {
        private const string SrcFile = @"TestData\HelloWorld.txt";
        private const string DstFile = "HelloWorld.txt";

        [Test]
        public void Should_store_the_file_on_sftp_folder()
        {
            SftpClient.CopyFile(SrcFile, DstFile);
            Assert.IsTrue(File.Exists(Path.Combine(SftpFolder, DstFile)), "The {0} does not exist in SFTP folder {1}", DstFile, SftpFolder);
        }

        [Test]
        public void Should_store_the_file_on_sftp_folder_with_proper_content()
        {
            var expectedContent = File.ReadAllText(SrcFile);

            SftpClient.CopyFile(SrcFile, DstFile);
            var actualContent = File.ReadAllText(Path.Combine(SftpFolder, DstFile));

            Assert.AreEqual(expectedContent, actualContent);
        }
    }
}
