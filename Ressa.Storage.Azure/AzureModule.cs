using System;
using System.ComponentModel;
using Ressa.Common;
using Ressa.Common.DI;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class AzureModule : IModule
    {
        public void Initialize(IServiceContainer container)
        {
            container.Register(
                Component.For<IAzureFacade>().ImplementedBy<AzureFacade>().LifeStyle.Transient,
                Component.For<IUrlGenerator>().ImplementedBy<UrlGenerator>().LifeStyle.Transient,
                Component.For<IDateTimeFormatter>().ImplementedBy<DateTimeFormatter>().LifeStyle.Transient,
                Component.For<IUrlSigner>().ImplementedBy<UrlSigner>().LifeStyle.Transient,
                Component.For<ISigner>().ImplementedBy<Signer>().LifeStyle.Transient,
                Component.For<IUploader>().ImplementedBy<Uploader>().LifeStyle.Transient,
                Component.For<IExistanceChecker>().ImplementedBy<ExistanceChecker>().LifeStyle.Transient,
                Component.For<IClient>().ImplementedBy<Client>().LifeStyle.Transient,
                Component.For<ILister>().ImplementedBy<Lister>().LifeStyle.Transient,
                Component.For<IConfigurationManagerWrapper>().ImplementedBy<ConfigurationManagerWrapper>().LifeStyle.Transient,
                Component.For<IAzureClientFactory>().ImplementedBy<AzureClientFactory>().LifeStyle.Transient,
                Component.For<IFileWrapper>().ImplementedBy<FileWrapper>().LifeStyle.Transient,
                Component.For<IAzureBuilder>().ImplementedBy<AzureBuilder>().LifeStyle.Transient,
                Component.For<IAzureBucketManipulator>().ImplementedBy<AzureBucketManipulator>().LifeStyle.Transient,
                Component.For<IAzureBucketManipulatorFactory>().ImplementedBy<AzureBucketManipulatorFactory>().LifeStyle.Transient
            );
        }
    }
}
