
namespace BuildingBlocks.EventBus.Events.Product;

public class StockReservationFailedEvent
{
    public Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
}