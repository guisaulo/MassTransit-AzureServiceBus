namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface IEventMessage
    {
        int EventCode { get; }
    }
}
