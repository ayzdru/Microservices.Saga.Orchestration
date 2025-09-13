using System.Collections.Generic;
using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Messages.Product;

public class StockRollbackMessage
{
    public List<OrderItem> OrderItems { get; set; }
}