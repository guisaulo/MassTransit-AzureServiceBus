using MassTransit.AzureServiceBus.Contracts.Eventos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class NotificacaoLoteSchemaConsumer : IConsumer<LoteSchemaCriadoEvent>
    {
        public Task Consume(ConsumeContext<LoteSchemaCriadoEvent> context)
        {
            Console.Out.WriteLineAsync($"Novo mensagem de evento recebida: {context.Message.LoteId}");
            return Task.CompletedTask;
        }
    }
}