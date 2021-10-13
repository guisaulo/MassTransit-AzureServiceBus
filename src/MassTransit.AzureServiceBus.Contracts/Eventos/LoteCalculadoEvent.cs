using System;

namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface LoteCalculadoEvent
    {
        Guid LoteId { get; }
    }
}
