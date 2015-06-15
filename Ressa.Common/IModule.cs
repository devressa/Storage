using System;
using Ressa.Common.DI;

namespace Ressa.Common
{
    public interface IModule
    {
        void Initialize(IServiceContainer container);
    }
}
