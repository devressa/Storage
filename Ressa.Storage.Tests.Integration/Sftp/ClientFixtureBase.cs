using System;
using System.IO;
using System.Net;
using Nuane.Net;
using NUnit.Framework;
using Ressa.Storage.Sftp;
using Rhino.Mocks;
using Ressa.Storage.Sftp.Interfaces;
using Ressa.Testing.Utilities;

namespace Ressa.Storage.Tests.Integration.Sftp
{
    [TestFixture]
    public abstract class ClientFixtureBase
    {
        protected SftpServer Server;
        protected string SftpHost = "127.0.0.1";
        protected const string SftpUser = "TestUser";
        protected const string SftpPassword = "TestUserPassword";
        protected int Port;

        protected IClient SftpClient;
        protected ISftpUrlParser Parser;
        protected string SftpFolder;

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            Port = PortAllocator.GetPort();

            SftpFolder = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, string.Format("SFTP-{0}", Guid.NewGuid()));
            Directory.CreateDirectory(SftpFolder);

            Server = new SftpServer { Log = Console.Out };

            Server.Keys.Add(SshKey.Generate(SshKeyAlgorithm.RSA, 1024));
            Server.Bindings.Add(IPAddress.Loopback, Port);
            Server.Users.Add(new SshUser(SftpUser, SftpPassword, SftpFolder));

            Server.Start();
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            Server.Stop();
            try
            {
                new DirectoryInfo(SftpFolder).Delete(true);
            }
            catch
            {
                // do nothing
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            Parser = MockRepository.GenerateStub<ISftpUrlParser>();
            Parser.Stub(x => x.GetHost(null)).IgnoreArguments().Return(SftpHost);
            Parser.Stub(x => x.GetPort(null)).IgnoreArguments().Return(Port);

            var sftpProvider = new SftpProvider { SftpUrl = SftpHost, SftpUserName = SftpUser, SftpPassPhrase = SftpPassword };
            SftpClient = new Client(sftpProvider, Parser, new ConfigurationManagerWrapper());
            SftpClient.Connect();
        }

        [TearDown]
        public virtual void TearDown()
        {
            SftpClient.Dispose();
            try
            {
                foreach (var file in new DirectoryInfo(SftpFolder).GetFiles()) file.Delete();
            }
            catch
            {
                // do nothing
            }
        }
    }
}
