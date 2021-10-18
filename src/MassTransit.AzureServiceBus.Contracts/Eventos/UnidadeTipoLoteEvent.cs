using System;

namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface UnidadeTipoLoteEvent
    {
        Guid Id { get; }
        int Unidade { get; }
        int TipoLote { get; }
    }
}
