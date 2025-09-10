using BuildingBlocks.EventBus.Interfaces.Product;

namespace BuildingBlocks.EventBus.Events.Product;

public class StockReservationFailedEvent : IStockReservationFailedEvent
{
    public Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
}