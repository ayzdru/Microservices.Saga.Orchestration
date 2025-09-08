

using MassTransit;
using Microservices.Saga.Orchestration.Shared.Entities;
using Microservices.Saga.Orchestration.Shared.Events.Order;
using Microservices.Saga.Orchestration.Shared.Infrastructure.Data;
using Microservices.Saga.Orchestration.Shared.Interfaces;
using Microservices.Saga.Orchestration.Shared.Models.Order;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace Microservices.Saga.Orchestration.Shared
{
    public class OrderService :
    IOrderService
    {
        readonly OrderStateDbContext _dbContext;
        readonly IPublishEndpoint _publishEndpoint;

        public OrderService(OrderStateDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<OrderState> SubmitOrder(List<OrderItem> orderItems)
        {
            var orderState = new OrderState
            {
                OrderId = NewId.NextGuid(),
                OrderDate = DateTime.UtcNow,
                TotalPrice = orderItems.Sum(q => q.Count * q.Price)
            };

            await _dbContext.Set<OrderState>().AddAsync(orderState);

            await _publishEndpoint.Publish(new OrderStartedEvent
            {
                OrderId = orderState.OrderId,
                OrderItems = orderItems,
                TotalPrice = orderState.TotalPrice
            });

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new DuplicateOrderException("Duplicate order", exception);
            }

            return orderState;
        }
    }
}