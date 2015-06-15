using System;

namespace Ressa.Common.Logger
{
    public interface ILog
    {
        void Write(string value);
        void Write(string format, params object[] args);
        void Write(Exception ex, string value);
        void Write(Exception ex, string format, params object[] args);
    }
}
