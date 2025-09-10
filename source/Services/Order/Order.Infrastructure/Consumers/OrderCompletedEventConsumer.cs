using BuildingBlocks.EventBus.Interfaces.Order;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Core.Enums;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Consumers;

public class OrderCompletedEventConsumer : IConsumer<IOrderCompletedEvent>
{
    private readonly OrderDbContext _context;

    private readonly ILogger<OrderCompletedEventConsumer> _logger;

    public OrderCompletedEventConsumer(OrderDbContext context, ILogger<OrderCompletedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderCompletedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.ChangeStatus(OrderStatus.Complete);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order with Id: {MessageOrderId} completed successfully", context.Message.OrderId);
        }
        else
        {
            _logger.LogError("Order with Id: {MessageOrderId} not found", context.Message.OrderId);
        }
    }
}