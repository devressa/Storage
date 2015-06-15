using System;

namespace Ressa.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullEmptyOrWhiteSpace(this string source)
        {
            return string.IsNullOrEmpty(source) || source.Trim().Length == 0;
        }
    }
}
