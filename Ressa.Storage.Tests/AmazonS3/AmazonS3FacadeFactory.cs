using System;
using Ressa.Common.DI;
using Ressa.Storage.AmazonS3;
using Ressa.Storage.AmazonS3.Interfaces;

namespace Ressa.Storage.Tests.AmazonS3
{
    public class AmazonS3FacadeFactory
    {
        private static bool isConfigured;
        private static IServiceContainer container;

        public static IAmazonS3Facade Create()
        {
            if (!isConfigured)
            {
                container = new Container1();

                var module = new AmazonS3Module();

                module.Initialize(container);

                isConfigured = true;
            }

            return container.Resolve<IAmazonS3Facade>();
        }
    }
}
