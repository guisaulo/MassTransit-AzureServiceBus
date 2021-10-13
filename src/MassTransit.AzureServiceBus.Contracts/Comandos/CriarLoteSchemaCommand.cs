using System;

namespace MassTransit.AzureServiceBus.Contracts.Comandos
{
    public interface CriarLoteSchemaCommand
    {
        Guid LoteId { get; }
        int Numero { get; }
        DateTime CreateDate { get; }
    }
}
