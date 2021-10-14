using MassTransit.AzureServiceBus.Contracts.Eventos;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker.Consumers
{
    public class EventCodeConsumer : IConsumer<IEventMessage>
    {
        public Task Consume(ConsumeContext<IEventMessage> context)
        {
            return Task.CompletedTask;
        }
    }
}
