namespace BuildingBlocks.EventBus.Interfaces.Product;

public interface IStockReservationFailedEvent
{
    Guid CorrelationId { get; set; }
    string ErrorMessage { get; set; }
}