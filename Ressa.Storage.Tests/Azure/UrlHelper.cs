using System;
using System.Web;

namespace Ressa.Storage.Tests.Azure
{
    public class UrlHelper
    {
        public static string ExtractUrl(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl))
                return fullUrl;

            var fullUri = new Uri(fullUrl);
            return HttpUtility.ParseQueryString(fullUri.Query).Get("url");
        }
    }
}
