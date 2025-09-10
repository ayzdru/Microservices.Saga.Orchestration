using System;
using System.Collections.Generic;

namespace EventBus.Events.Interfaces;

public interface IStockReservedEvent
{
    Guid CorrelationId { get; set; }
    List<OrderItem> OrderItemList { get; set; }
}