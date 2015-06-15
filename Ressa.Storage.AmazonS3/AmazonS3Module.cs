using System;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.AmazonS3
{
    public class AmazonS3Module : IModule
    {
        public void Initialize(IWindsorContainer container)
        {
            container.Register(
                Component.For<IS3Facade>().ImplementedBy<S3Facade>().LifeStyle.Transient,
                Component.For<IUrlGenerator>().ImplementedBy<UrlGenerator>().LifeStyle.Transient,
                Component.For<IDateTimeFormatter>().ImplementedBy<DateTimeFormatter>().LifeStyle.Transient,
                Component.For<IUrlSigner>().ImplementedBy<UrlSigner>().LifeStyle.Transient,
                Component.For<ISigner>().ImplementedBy<Signer>().LifeStyle.Transient,
                Component.For<IUploader>().ImplementedBy<Uploader>().LifeStyle.Transient,
                Component.For<IExistanceChecker>().ImplementedBy<ExistanceChecker>().LifeStyle.Transient,
                Component.For<IClient>().ImplementedBy<Client>().LifeStyle.Transient,
                Component.For<ILister>().ImplementedBy<Lister>().LifeStyle.Transient,
                Component.For<IConfigurationManagerWrapper>().ImplementedBy<ConfigurationManagerWrapper>().LifeStyle.Transient,
                Component.For<IAmazonS3ClientFactory>().ImplementedBy<AmazonS3ClientFactory>().LifeStyle.Transient,
                Component.For<IFileWrapper>().ImplementedBy<FileWrapper>().LifeStyle.Transient
            );
        }
    }
}
