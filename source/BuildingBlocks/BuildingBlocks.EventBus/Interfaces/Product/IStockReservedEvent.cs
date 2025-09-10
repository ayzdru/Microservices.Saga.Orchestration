using BuildingBlocks.EventBus.Models.Order;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.EventBus.Interfaces.Product;

public interface IStockReservedEvent
{
    Guid CorrelationId { get; set; }
    List<OrderItem> OrderItems { get; set; }
}