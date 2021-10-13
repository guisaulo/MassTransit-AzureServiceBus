using MassTransit.AzureServiceBus.Contracts;
using MassTransit.AzureServiceBus.Contracts.Comandos;
using MassTransit.AzureServiceBus.Contracts.Eventos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoteController : Controller
    {
        private const string URI_QUEUE = "sb://athena-sbus-dev-brazilsouth-001.servicebus.windows.net/masstransit-mes-lotes-queue";
        private const string URI_FAULT_QUEUE = "sb://athena-sbus-dev-brazilsouth-001.servicebus.windows.net/masstransit-mes-lotes-dead-letter-queue";

        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public LoteController(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost, Route("CriarLoteSchema")]
        public async Task<IActionResult> CriarLoteSchema()
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(URI_QUEUE));

            ////context contém os headers do envelope de mensagem
            await sendEndpoint.Send<CriarLoteSchemaCommand>(new
            {
                LoteId = NewId.NextGuid(),
                Numero = "123",
                CreatedDate = DateTime.UtcNow
            }, context =>
            {
                context.CorrelationId = NewId.NextGuid();
                context.TimeToLive = TimeSpan.Parse("5000");
                context.FaultAddress = new Uri(URI_FAULT_QUEUE);
            });

            return Ok();
        }

        [HttpPost, Route("PublicarLoteRecalculado")]
        public async Task<IActionResult> PublicarLoteRecalculado()
        {
            await _publishEndpoint.Publish<LoteRecalculadoEvent>(new
            {
                LoteId = NewId.NextGuid()
            }, context => {
                context.Headers.Set("NotificacaoId", "30879c69-702b-4532-bb27-c6c479660e75");
                context.Headers.Set("RecalculoLoteId", "4396038d-7cbb-4a29-8225-7afd383f8f15");
            });

            return Ok();
        }

        [HttpPost, Route("PublicarLoteCalculado")]
        public async Task<IActionResult> PublicarLoteCalculado()
        {
            await _publishEndpoint.Publish<LoteCalculadoEvent>(new
            {
                LoteId = NewId.NextGuid()
            }, context =>
            {
                context.Headers.Set("NotificacaoId", "30879c69-702b-4532-bb27-c6c479660e75");
                context.Headers.Set("RecalculoLoteId", "4396038d-7cbb-4a29-8225-7afd383f8f15");
            });

            //await _publishEndpoint.Publish<LoteCalculadoEvent>(new
            //{
            //    LoteId = NewId.NextGuid()
            //}, context => {
            //    context.Headers.Set("RecalculoLoteId", "4396038d-7cbb-4a29-8225-7afd383f8f15");
            //});

            return Ok();
        }
    }
}
