using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace Ressa.Testing.Utilities
{
    public static class PortAllocator
    {
        private static readonly object PortLock = new object();
        private static int currentPort = 4238;

        public static int GetPort()
        {
            currentPort = GetPort(currentPort);
            return currentPort++;
        }

        public static int GetPort(int port)
        {
            var lockTaken = false;
            object obj = null;
            try
            {
                Monitor.Enter(obj = PortLock, ref lockTaken);
                var initialPort = port;
                List<int> list = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Where((t => t.Port >= initialPort)).OrderBy(t => t.Port).Select(t => t.Port).ToList();
                while (list.Contains(port))
                    ++port;
                return port;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(obj);
            }
        }
    }
}
