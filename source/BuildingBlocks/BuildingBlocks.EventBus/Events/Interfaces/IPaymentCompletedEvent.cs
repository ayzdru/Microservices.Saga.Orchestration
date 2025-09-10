using System;

namespace EventBus.Events.Interfaces;

public interface IPaymentCompletedEvent
{
    Guid CorrelationId { get; set; }
}