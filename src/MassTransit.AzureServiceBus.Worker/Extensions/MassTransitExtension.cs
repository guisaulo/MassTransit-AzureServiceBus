using GreenPipes;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using MassTransit.AzureServiceBus.Worker.Consumers;
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
                x.AddConsumer<NotificacaoLoteSchemaConsumer>();
                x.AddConsumer<LoteRecalculadoConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    //Configura Endpoint da fila principal
                    cfg.ReceiveEndpoint("masstransit-mes-lotes-subscriber", e =>
                    {
                        //Tenta entregar a mensagem ao consumidor 1 vez antes de lançar a exceção no pipeline - fila de erros
                        e.UseMessageRetry(r => r.Immediate(1));
                        //Descartar criação de filas automaticas de erros e faltas
                        e.DiscardFaultedMessages();
                        e.ConfigureConsumer<LoteConsumer>(context);
                    });

                    //Configura Endpoint da fila de erros e falhas
                    cfg.ReceiveEndpoint("masstransit-mes-lotes-dead-letter", e =>
                    {
                        e.ConfigureConsumer<LoteFaultConsumer>(context);
                    });

                    //Configura a mensagem que sera enviada para o tópico
                    cfg.Message<LoteSchemaCriadoEvent>(configTopology =>
                    {
                        configTopology.SetEntityName("masstransit-mes-notificacao-lote-schema-publisher");
                    });

                    //Configura a assinatura e o consumidor
                    cfg.SubscriptionEndpoint<LoteSchemaCriadoEvent>("masstransit-notificacao-lote-schema-subscriber", endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumer<NotificacaoLoteSchemaConsumer>(context);
                    });

                    cfg.Message<LoteRecalculadoEvent>(configTopology =>
                    {
                        configTopology.SetEntityName("mes-lotes-publisher");
                    });

                    cfg.SubscriptionEndpoint<LoteRecalculadoEvent>("masstransit-notificacao-recalculo-lotes-subscriber", endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumer<LoteRecalculadoConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
