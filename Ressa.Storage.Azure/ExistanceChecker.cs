using System;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class ExistanceChecker : IExistanceChecker
    {
        private readonly IClient client;

        public ExistanceChecker(IClient client)
        {
            this.client = client;
        }

        public bool Exists(AzureBucket bucket, string fileName)
        {
            return client.DoesFileExist(bucket, fileName);
        }
    }
}
