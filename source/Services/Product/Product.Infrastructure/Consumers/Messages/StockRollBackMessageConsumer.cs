using BuildingBlocks.EventBus.Interfaces.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Infrastructure.Data;

namespace Subscription.Infrastructure.Consumers.Messages;

public class StockRollBackMessageConsumer : IConsumer<IStockRollBackMessage>
{
    private readonly ProductDbContext _context;
    private readonly ILogger<StockRollBackMessageConsumer> _logger;

    public StockRollBackMessageConsumer(ProductDbContext context, ILogger<StockRollBackMessageConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IStockRollBackMessage> context)
    {
        foreach (var item in context.Message.OrderItems)
        {
            var stock = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

            if (stock != null)
            {
                stock.StockIncrease(item.Count);
                await _context.SaveChangesAsync();
            }
        }

        _logger.LogInformation($"Stock was released");
    }
}