using System;
using System.Globalization;
using System.Linq;

namespace Ressa.Common.Logger
{
    public abstract class BaseLog : ILog
    {
        public abstract void Write(string value);

        public virtual void Write(Exception ex, string value)
        {
            var aggregateException = ex as AggregateException;
            if (aggregateException != null && aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Any())
                ex = aggregateException.InnerExceptions.First();
            
            var invariantCulture = CultureInfo.InvariantCulture;
            object[] objArray = { value, Environment.NewLine, ex };
            Write(string.Format(invariantCulture, "{0}{1}{2}", objArray));
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }
        
        public void Write(Exception ex, string format, params object[] args)
        {
            Write(ex, string.Format(format, args));
        }
    }
}
