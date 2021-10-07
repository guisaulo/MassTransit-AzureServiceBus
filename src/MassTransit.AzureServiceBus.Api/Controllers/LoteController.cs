using MassTransit.AzureServiceBus.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MassTransit.AzureServiceBus.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoteController : Controller
    {
        private const string URI_QUEUE = "sb://athena-sbus-dev-brazilsouth-001.servicebus.windows.net/mes-lotes-subscriber";
        private const string URI_FAULT_QUEUE = "sb://athena-sbus-dev-brazilsouth-001.servicebus.windows.net/mes-lotes-dead-letter";

        private readonly ISendEndpointProvider _sendEndpointProvider;

        public LoteController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost, Route("Sender")]
        public async Task<IActionResult> SendCommand()
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
    }
}
