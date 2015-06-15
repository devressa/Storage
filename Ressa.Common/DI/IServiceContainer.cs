using System;

namespace Ressa.Common.DI
{
    public interface IServiceContainer : IDisposable
    {
        T GetInstance<T>();
    }
}
