using System;

namespace MassTransit.AzureServiceBus.Contracts
{
    public interface RecalculoLoteEvent
    {
        Guid LoteId { get; }
        bool Success { get; }
        string Origem { get; }
    }
}
