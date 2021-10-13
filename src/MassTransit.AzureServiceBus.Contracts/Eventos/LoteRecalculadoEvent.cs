using System;

namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface LoteRecalculadoEvent
    {
        Guid NotificacaoId { get; }
        Guid RecalculoLoteId { get; }
    }
}