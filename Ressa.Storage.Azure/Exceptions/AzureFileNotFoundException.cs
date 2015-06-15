using System;

namespace Ressa.Storage.Azure.Exceptions
{
    public class AzureFileNotFoundException : Exception
    {
        public AzureFileNotFoundException(string message) : base(message)
        {
        }
    }
}
