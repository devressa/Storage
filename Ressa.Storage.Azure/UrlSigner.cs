using System;
using System.Linq;
using Ressa.Storage.Azure.Interfaces;

namespace Ressa.Storage.Azure
{
    public class UrlSigner : IUrlSigner
    {
        private readonly ISigner signer;

        public UrlSigner(ISigner signer)
        {
            this.signer = signer;
        }

        public string Sign(AzureBucket bucket, Uri toSign)
        {
            var queryStringSegments = toSign.Query.TrimStart('?').Split('&');
            var expires = queryStringSegments.Single(x => x.StartsWith("Expires=")).Substring("Expires=".Length);
            return signer.Sign(bucket, string.Format("GET\n\n\n{0}\n{1}", expires, toSign.AbsolutePath));
        }
    }
}
