using MassTransit.AzureServiceBus.Contracts.Eventos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class LoteRecalculadoConsumer : IConsumer<LoteRecalculadoEvent>
    {
        public Task Consume(ConsumeContext<LoteRecalculadoEvent> context)
        {
            Console.Out.WriteLineAsync($"Nova mensagem de evento recebida: "
                + $"LoteId: {context.Message.LoteId}");

            return Task.CompletedTask;
        }
    }
}
