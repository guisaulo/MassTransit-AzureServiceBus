using MassTransit.AzureServiceBus.Contracts.Comandos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class LoteFaultConsumer : IConsumer<Fault<CriarLoteSchemaCommand>>
    {
        public Task Consume(ConsumeContext<Fault<CriarLoteSchemaCommand>> context)
        {
            Console.Out.WriteLineAsync($"Nova mensagem de comando com falha foi recebida: {context.Message.Message.LoteId}");
            return Task.CompletedTask;
        }
    }
}