using MassTransit.AzureServiceBus.Contracts.Comandos;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.AzureServiceBus.Api.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    cfg.Message<CriarLoteSchemaCommand>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("masstransit-mes-lotes-queue");
                    });

                    cfg.Message<LoteRecalculadoEvent>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("masstransit-mes-lotes-publisher");
                    });

                    cfg.Message<LoteCalculadoEvent>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("masstransit-mes-lotes-publisher");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
