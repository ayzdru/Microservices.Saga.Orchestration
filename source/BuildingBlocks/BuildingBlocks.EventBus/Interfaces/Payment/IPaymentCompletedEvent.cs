using System;

namespace BuildingBlocks.EventBus.Interfaces.Payment;

public interface IPaymentCompletedEvent
{
    Guid CorrelationId { get; set; }
}