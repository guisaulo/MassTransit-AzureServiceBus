using MassTransit.AzureServiceBus.Contracts.Comandos;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class LoteConsumer : IConsumer<CriarLoteSchemaCommand>
    {
        public Task Consume(ConsumeContext<CriarLoteSchemaCommand> context)
        {
            try
            {
                Console.Out.WriteLineAsync($"Nova mensagem de comando recebida: "
                    + $"LoteId: {context.Message.LoteId} "
                    + $"Numero: {context.Message.Numero} "
                    + $"CreatedDate: {context.Message.CreateDate}");

                return Task.CompletedTask;

                //throw new Exception($"Houve um erro ao consumir a mensagem de comando");
                //GerarRecursao();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GerarRecursao()
        {
            GerarRecursao();
        }
    }
}
