using System;

namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface LoteSchemaCriadoEvent
    {
        Guid LoteId { get; }
        int Numero { get; }
        DateTime CreatedDate { get; }
    }
}
