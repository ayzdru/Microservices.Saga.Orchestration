using BuildingBlocks.EventBus.Interfaces.Product;
using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Events.Product;

public class StockReservedEvent : IStockReservedEvent
{
    public Guid CorrelationId { get; set; }
    public List<OrderItem> OrderItems { get; set; }

}