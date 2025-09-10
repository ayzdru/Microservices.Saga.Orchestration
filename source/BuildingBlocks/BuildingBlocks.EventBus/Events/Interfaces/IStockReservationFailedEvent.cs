
namespace EventBus.Events.Interfaces;

public interface IStockReservationFailedEvent
{
    Guid CorrelationId { get; set; }
    string ErrorMessage { get; set; }
}