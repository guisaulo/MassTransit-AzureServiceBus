using MassTransit.AzureServiceBus.Contracts.Comandos;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class LoteConsumer : IConsumer<CriarLoteSchemaCommand>
    {
        public readonly IPublishEndpoint _publishEndpoint;
        public LoteConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Consume(ConsumeContext<CriarLoteSchemaCommand> context)
        {
            Console.Out.WriteLineAsync($"Nova mensagem de comando recebida: "
                + $"CorrelationId: {context.CorrelationId} "
                + $"LoteId: {context.Message.LoteId} "
                + $"Numero: {context.Message.Numero}");

            return Task.CompletedTask;

            //Descomentar para simular um erro
            //throw new Exception($"Houve um erro ao consumir a mensagem: "
            //    + $"CorrelationId: {context.CorrelationId}"
            //    + $"LoteId: {context.Message.LoteId}");
            //    OlaMundo();
        }

        public void OlaMundo()
        {
            OlaMundo();
        }
    }
}
