using System;

namespace MassTransit.AzureServiceBus.Contracts
{
    public interface LoteCalculadoEvent
    {
        Guid LoteId { get; }
        bool Success { get; }
        string Origem { get; }
    }
}
