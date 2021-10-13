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
                //Configura o tipo de formatação dos endpoints
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    //Configura Topologia das mensagens para uma fila especifica
                    cfg.Message<CriarLoteSchemaCommand>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("masstransit-mes-lotes-queue");
                    });

                    //Configura Topologia das mensagens para uma fila especifica
                    cfg.Message<LoteRecalculadoEvent>(cfgTopology =>
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
