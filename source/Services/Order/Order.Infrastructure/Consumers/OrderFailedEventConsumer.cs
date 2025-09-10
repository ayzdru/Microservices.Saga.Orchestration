using BuildingBlocks.EventBus.Interfaces.Order;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Core.Enums;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Consumers;

public class OrderFailedEventConsumer : IConsumer<IOrderFailedEvent>
{
    private readonly OrderDbContext _context;

    private readonly ILogger<OrderFailedEventConsumer> _logger;

    public OrderFailedEventConsumer(OrderDbContext context, ILogger<OrderFailedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderFailedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.ChangeStatus(OrderStatus.Fail, context.Message.ErrorMessage);     
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order with Id: {MessageOrderId} failed, status updated to {Status}", context.Message.OrderId, OrderStatus.Fail);
        }
        else
        {
            _logger.LogError("Order with Id: {MessageOrderId} not found", context.Message.OrderId);
        }
    }
}