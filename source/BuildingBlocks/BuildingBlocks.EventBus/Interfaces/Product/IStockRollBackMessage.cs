using BuildingBlocks.EventBus.Models.Order;
using System.Collections.Generic;

namespace BuildingBlocks.EventBus.Interfaces.Product;

public interface IStockRollBackMessage
{
    public List<OrderItem> OrderItems { get; set; }
}