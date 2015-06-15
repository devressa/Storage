using System;

namespace Ressa.Common.Logger
{
    public static class LogManager
    {
        private static ILog logger;

        public static void Init(ILog log)
        {
            logger = log ?? new NullLog();
        }

        public static ILog GetLogger<T>()
        {
            return GetLogger(typeof (T));
        }

        public static ILog GetLogger(Type type)
        {
            return new TypedLogger(() => logger, type.FullName);
        }
    }
}
