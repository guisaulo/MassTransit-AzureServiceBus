using GreenPipes;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using MassTransit.AzureServiceBus.Worker.Consumers;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.AzureServiceBus.Worker.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<LoteConsumer>();
                x.AddConsumer<LoteFaultConsumer>();
                x.AddConsumer<LoteRecalculadoConsumer>();
                x.AddConsumer<LoteCalculadoConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    cfg.ReceiveEndpoint("masstransit-mes-lotes-queue", e =>
                    {
                        e.UseMessageRetry(r => r.Immediate(5));
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<LoteConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("masstransit-mes-lotes-dead-letter-queue", e =>
                    {
                        e.ConfigureConsumer<LoteFaultConsumer>(context);
                    });

                    cfg.Message<LoteRecalculadoEvent>(configTopology =>
                    {
                        configTopology.SetEntityName("masstransit-mes-lotes-publisher");
                    });

                    cfg.SubscriptionEndpoint<LoteRecalculadoEvent>("masstransit-notificacao-recalculo-lotes-subscriber", endpointConfig =>
                    {
                        var filter = new CorrelationFilter();
                        filter.Properties["notificacaoId"] = "30879c69-702b-4532-bb27-c6c479660e75";
                        filter.Properties["recalculoLoteId"] = "4396038d-7cbb-4a29-8225-7afd383f8f15";

                        endpointConfig.Rule = new RuleDescription()
                        {
                            Name = "masstransit-filter",
                            Filter = filter
                        };
                        endpointConfig.ConfigureConsumer<LoteRecalculadoConsumer>(context);
                    });

                    cfg.Message<LoteCalculadoEvent>(configTopology =>
                    {
                        configTopology.SetEntityName("masstransit-mes-lotes-publisher");
                    });

                    cfg.SubscriptionEndpoint<LoteCalculadoEvent>("masstransit-lote-calculado-subscriber", endpointConfig =>
                    {
                        endpointConfig.Rule = new RuleDescription()
                        {
                            Name = "masstransit-filter",
                            Filter = new SqlFilter("NotificacaoId IS NOT NULL AND RecalculoLoteId IS NOT NULL")
                        };
                        endpointConfig.ConfigureConsumer<LoteCalculadoConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
