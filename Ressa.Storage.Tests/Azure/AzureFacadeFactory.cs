using System;
using System.ComponentModel;
using Ressa.Common.DI;
using Ressa.Storage.Azure;
using Ressa.Storage.Azure.Interfaces;
using Ressa.Storage.Interfaces;

namespace Ressa.Storage.Tests.Azure
{
    public class AzureFacadeFactory
    {
        private static bool isConfigured;
        private static IServiceContainer container;

        public static IAzureFacade Create()
        {
            if (!isConfigured)
            {
                container = new Container();

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
                    Component.For<IConfigurationManagerWrapper>().ImplementedBy<ConfigurationManagerWrapperFake>().LifeStyle.Transient,
                    Component.For<IAzureClientFactory>().ImplementedBy<AzureClientFactory>().LifeStyle.Transient,
                    Component.For<IFileWrapper>().ImplementedBy<FileWrapper>().LifeStyle.Transient,
                    Component.For<IAzureBuilder>().ImplementedBy<AzureBuilder>().LifeStyle.Transient
                    );

                isConfigured = true;
            }

            return container.Resolve<IAzureFacade>();
        }
    }
}
