using BuildingBlocks.EventBus.Events.Order;
using BuildingBlocks.EventBus.Events.Product;
using BuildingBlocks.EventBus.Interfaces.Order;
using BuildingBlocks.MassTransit.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Consumers.Events;

public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
{
    private readonly ProductDbContext _dbContext;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IMassTransitService _massTransitService;

    public OrderCreatedEventConsumer(ProductDbContext dbContext, ILogger<OrderCreatedEventConsumer> logger, IMassTransitService massTransitService)
    {
        _massTransitService = massTransitService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
    {       
        var isThereEnoughStock = true;
        foreach (var item in _dbContext.Products.Where(x => context.Message.OrderItems.Select(y => y.ProductId).Contains(x.Id)).AsEnumerable())
        {
            if (!context.Message.OrderItems.Select(y => y.ProductId).Contains(item.Id) || item.Stock <= context.Message.OrderItems.FirstOrDefault(y => y.ProductId == item.Id).Count)
            {
                isThereEnoughStock = false;
                break;
            }
        }

        if (!isThereEnoughStock)
        {
            await _massTransitService.Publish(new StockReservationFailedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                ErrorMessage = "Not enough stock"
            });

            _logger.LogInformation("Not enough stock for CorrelationId Id :{MessageCorrelationId}", context.Message.CorrelationId);
        }
        else
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

                if (stock == null)
                {
                    await _massTransitService.Publish(new StockReservationFailedEvent
                    {
                        CorrelationId = context.Message.CorrelationId,
                        ErrorMessage = $"Stock not found with product id {item.ProductId} and CorrelationId Id :{context.Message.CorrelationId}"
                    });

                    _logger.LogInformation("Stock not found with product Id: {ItemProductId} and CorrelationId Id :{MessageCorrelationId}", item.ProductId, context.Message.CorrelationId);
                    return;
                }

                stock.StockDecrease(item.Count);
                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Stock was reserved with CorrelationId Id: {MessageCorrelationId}", context.Message.CorrelationId);

            StockReservedEvent stockReservedEvent = new StockReservedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                OrderItems = context.Message.OrderItems
            };

            await _massTransitService.Publish(stockReservedEvent);
          
        }
    }
}