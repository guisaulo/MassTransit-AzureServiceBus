﻿using System;

namespace MassTransit.AzureServiceBus.Contracts.Eventos
{
    public interface LoteRecalculadoEvent
    {
        Guid LoteId { get; }
    }
}