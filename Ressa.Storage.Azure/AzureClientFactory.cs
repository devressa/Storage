using System;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class AzureClientFactory : IAzureClientFactory
    {
        private readonly IAzureBuilder azureComponentsBuilder;
        public AzureClientFactory(IAzureBuilder azureComponentsBuilder)
        {
            this.azureComponentsBuilder = azureComponentsBuilder;
        }

        public IAzureClientWrapper CreateInstance(AzureBucket bucket)
        {
            return new AzureClientWrapper(bucket, azureComponentsBuilder);
        }
    }
}
