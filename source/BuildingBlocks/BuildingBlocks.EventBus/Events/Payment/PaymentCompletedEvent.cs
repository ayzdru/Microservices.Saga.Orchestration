using BuildingBlocks.EventBus.Interfaces.Payment;

namespace BuildingBlocks.EventBus.Events.Payment;

public class PaymentCompletedEvent : IPaymentCompletedEvent
{
    public Guid CorrelationId { get; set; }
}