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
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    cfg.Message<UnidadeTipoLoteEvent>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("masstransitunidade.032.tipolote.1.id.4396038d-7cbb-4a29-8225-7afd383f8f15");
                    });

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
