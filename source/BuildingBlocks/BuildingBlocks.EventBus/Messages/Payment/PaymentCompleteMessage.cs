using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Messages.Payment;

public class PaymentCompleteMessage
{
    public Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; }

}