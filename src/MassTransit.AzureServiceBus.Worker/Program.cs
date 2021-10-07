using GreenPipes;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using MassTransit.AzureServiceBus.Worker.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Path.Combine(AppContext.BaseDirectory));
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        x.AddConsumer<LoteConsumer>();
                        x.AddConsumer<LoteFaultConsumer>();
                        x.AddConsumer<NotificacaoLoteConsumer>();

                        x.UsingAzureServiceBus((context, cfg) =>
                        {
                            cfg.Host(hostContext.Configuration.GetConnectionString("AzureServiceBus"));

                            //Configura Endpoint da fila principal
                            cfg.ReceiveEndpoint("mt-mes-lotes-subscriber", e =>
                            {
                                //Tenta entregar a mensagem ao consumidor 1 vez antes de lançar a exceção no pipeline - fila de erros
                                e.UseMessageRetry(r => r.Immediate(1));
                                //Descartar criação de filas automaticas de erros e faltas
                                e.DiscardFaultedMessages();
                                e.ConfigureConsumer<LoteConsumer>(context);
                            });

                            //Configura Endpoint da fila de erros e falhas
                            cfg.ReceiveEndpoint("mt-mes-lotes-dead-letter", e =>
                            {
                                e.ConfigureConsumer<LoteFaultConsumer>(context);
                            });

                            //Configura a mensagem que sera enviada para o tópico
                            cfg.Message<LoteSchemaCriadoEvent>(configTopology =>
                            {
                                configTopology.SetEntityName("mt-mes-lotes-publisher");
                            });

                            //Configura a assinatura e o consumidor
                            cfg.SubscriptionEndpoint<LoteSchemaCriadoEvent>("mt-notificacao-lotes-subscriber", endpointConfig =>
                            {
                                endpointConfig.ConfigureConsumer<NotificacaoLoteConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
        }
    }
}
