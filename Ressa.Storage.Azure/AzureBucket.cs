using System;

namespace Ressa.Storage.Azure
{
    public class AzureBucket
    {
        public string AccountName { get; private set; }
        public string Name { get; private set; }
        public string PrimaryAccessKey { get; private set; }
        public string SecondaryAccessKey { get; private set; }

        public AzureBucket(string accountName, string containerName, string primaryAccessKey, string secondaryAccessKey)
        {
            AccountName = accountName;
            Name = containerName;
            PrimaryAccessKey = primaryAccessKey;
            SecondaryAccessKey = secondaryAccessKey;
        }
    }
}
