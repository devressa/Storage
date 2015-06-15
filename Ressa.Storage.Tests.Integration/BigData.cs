using System;

namespace Ressa.Storage.Tests.Integration
{
    static class BigData
    {
        readonly static byte[] Mb1;
        public readonly static byte[] Mb100;
        public readonly static byte[] Mb40;
        public readonly static byte[] Mb10;

        static BigData()
        {
            var rand = new Random(Environment.TickCount);
            Mb1 = new byte[1024 * 1024];
            for (var i = 0; i < Mb1.Length; ++i)
            {
                Mb1[i] = (byte)rand.Next();
            }
            Mb100 = CreateArray(100);
            Mb40 = CreateArray(40);
            Mb10 = CreateArray(10);
        }

        private static byte[] CreateArray(int size)
        {
            var data = new byte[size * Mb1.Length];
            for (var i = 0; i < size; ++i)
            {
                Array.Copy(Mb1, 0, data, i * Mb1.Length, Mb1.Length);
            }
            return data;
        }
    }
}
