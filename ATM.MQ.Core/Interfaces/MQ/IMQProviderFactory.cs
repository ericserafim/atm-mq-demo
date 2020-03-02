using System;

namespace ATM.MQ.Core.Interfaces.MQ
{
    public interface IMQProviderFactory
    {
        IMQProvider Create();
    }
}
