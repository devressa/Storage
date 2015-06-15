using System;

namespace Ressa.Storage.Azure.Exceptions
{
    public class AzureException : Exception
    {
        public AzureException(string message, Exception innerexception) : base(message, innerexception)
        {
        }
    }
}
