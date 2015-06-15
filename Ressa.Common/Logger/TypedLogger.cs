using System;

namespace Ressa.Common.Logger
{
    internal sealed class TypedLogger : BaseLog
    {
        private readonly Func<ILog> logger;
        private readonly string fullName;

        public TypedLogger(Func<ILog> logger, string fullName)
        {
            this.logger = logger;
            this.fullName = fullName;
        }
        public override void Write(string value)
        {
            logger().Write(string.Format("{0}: {1}", fullName, value));
        }

        public override void Write(Exception ex, string value)
        {
            logger().Write(ex, string.Format("{0}: {1}", fullName, value));
        }
    }
}
