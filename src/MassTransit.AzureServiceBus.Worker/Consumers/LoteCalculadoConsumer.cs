using MassTransit.AzureServiceBus.Contracts.Eventos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class LoteCalculadoConsumer : IConsumer<LoteCalculadoEvent>
    {
        public Task Consume(ConsumeContext<LoteCalculadoEvent> context)
        {
            Console.Out.WriteLineAsync($"Nova mensagem de evento recebida, LoteId: {context.Message.LoteId}");

            return Task.CompletedTask;
        }
    }
}
