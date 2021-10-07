using MassTransit.AzureServiceBus.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MassTransit.AzureServiceBus.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "MassTransit AzureServiceBus API",
                    Description = "POC com o MassTransit e AzureServiceBus",
                });
            });

            services.AddMassTransit(x =>
            {
                //Configura o tipo de formatação dos endpoints
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((_, cfg) =>
                {
                    cfg.Host(Configuration.GetConnectionString("AzureServiceBus"));

                    //Configura Topologia das mensagens para uma fila especifica
                    cfg.Message<CriarLoteSchemaCommand>(cfgTopology =>
                    {
                        cfgTopology.SetEntityName("mes-lotes-subscriber");
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MassTransit.AzureServiceBus.Api");
                    c.OAuthClientId("Swagger");
                    c.OAuthClientSecret("swagger");
                    c.OAuthAppName("MassTransit.AzureServiceBus.Api v1");
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
