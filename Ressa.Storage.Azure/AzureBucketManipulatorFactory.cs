using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    class AzureBucketManipulatorFactory : IAzureBucketManipulatorFactory
    {
        public IAzureBucketManipulator Create(string accountName, string primaryAccessKey)
        {
            return new AzureBucketManipulator(accountName, primaryAccessKey);
        }
    }
}
