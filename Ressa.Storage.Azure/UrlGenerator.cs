using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class UrlGenerator : IUrlGenerator
    {
        private readonly IAzureBuilder azureComponentsBuilder;
        private readonly ConfigurationHelper configurationHelper;

        public UrlGenerator(IAzureComponentsBuilder azureComponentsBuilder, IConfigurationManagerWrapper configurationManager)
        {
            this.azureComponentsBuilder = azureComponentsBuilder;
            configurationHelper = new ConfigurationHelper(configurationManager);
        }

        public string GetPreviewUrl(AzureBucket azureBucket, string fileName)
        {
            var blob = azureComponentsBuilder.BuildContainer(azureBucket).GetBlockBlobReference(fileName);
            var signature = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = SharedAccessBlobPermissions.Read
            });
            return blob.Uri.AbsoluteUri + signature;
        }

        public string GetUrl(AzureBucket azureBucket, string fileName, DateTimeOffset expiryTime, string contentDispositionFileName = null)
        {
            var blob = azureComponentsBuilder.BuildContainer(azureBucket).GetBlockBlobReference(fileName);
            var builder = new UriBuilder(blob.Uri);
            var encodedFileName = EncodeContentDisposition(CleanFileName(string.IsNullOrEmpty(contentDispositionFileName)
                ? fileName
                : contentDispositionFileName));
            var encodedContentDisposition = string.Format("attachment; filename=\"{0}\"; filename*=utf-8''{0}", encodedFileName);
            builder.Query = blob.GetSharedAccessSignature(
                new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime = expiryTime,
                    Permissions = SharedAccessBlobPermissions.Read
                },
                new SharedAccessBlobHeaders
                {
                    ContentDisposition = encodedContentDisposition

                }).TrimStart('?');
            return builder.Uri.AbsoluteUri;
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Concat(new[] { '&', '%', '+' }).Aggregate(fileName, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), " "));
        }

        private static string EncodeContentDisposition(string contentDisposition)
        {
            contentDisposition = ApplyEncodeRules(contentDisposition);
            return contentDisposition;
        }

        private static string ApplyEncodeRules(string strToEncode)
        {
            strToEncode = HttpUtility.UrlEncode(strToEncode).Replace("+", "%20");
            return strToEncode;
        }

        public string GetUploadUrl(AzureBucket azureBucket, string fileName, DateTimeOffset expiryTime)
        {
            var blob = azureComponentsBuilder
                .BuildContainer(azureBucket)
                .GetBlockBlobReference(fileName);
            var signature = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = expiryTime,
                Permissions = SharedAccessBlobPermissions.Write
            });
            return blob.Uri.AbsoluteUri + signature;
        }
    }
}
