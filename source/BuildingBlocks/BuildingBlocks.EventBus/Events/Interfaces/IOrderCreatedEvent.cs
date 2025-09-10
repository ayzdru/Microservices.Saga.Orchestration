using System;
using System.Collections.Generic;

namespace EventBus.Events.Interfaces;

public interface IOrderCreatedEvent
{
    Guid CorrelationId { get; set; }
    List<OrderItem> OrderItemList { get; set; }
}