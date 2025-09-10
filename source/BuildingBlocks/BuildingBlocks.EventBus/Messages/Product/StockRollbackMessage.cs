using System.Collections.Generic;
using BuildingBlocks.EventBus.Interfaces.Product;
using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Messages.Product;

public class StockRollbackMessage : IStockRollBackMessage
{
    public List<OrderItem> OrderItems { get; set; }
}