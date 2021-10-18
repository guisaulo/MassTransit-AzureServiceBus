using MassTransit.AzureServiceBus.Contracts.Eventos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class UnidadeTipoLoteConsumer : IConsumer<UnidadeTipoLoteEvent>
    {
        public Task Consume(ConsumeContext<UnidadeTipoLoteEvent> context)
        {
            Console.Out.WriteLineAsync($"Nova mensagem de evento recebida, Id: {context.Message.Id}");

            return Task.CompletedTask;
        }
    }
}
