using BuildingBlocks.EventBus.Models.Order;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.EventBus.Interfaces.Order;

public interface IOrderCreatedEvent
{
    Guid CorrelationId { get; set; }
    List<OrderItem> OrderItems { get; set; }
}