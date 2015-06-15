using System;

namespace Ressa.Storage.Azure.Exceptions
{
    public class AzureUploadExeption : Exception
    {
        public AzureUploadExeption(string message) : base(message) { }
    }
}
