

using MassTransit;
using Microservices.Saga.Orchestration.Shared.Events.Order;
using Microservices.Saga.Orchestration.Shared.Exceptions;
using Microservices.Saga.Orchestration.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Orchestration.Core.Entities;
using Orchestration.Core.Models.Order;
using Orchestration.Infrastructure.Data;
namespace Orchestration.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        readonly OrchestrationDbContext _dbContext;
        readonly IPublishEndpoint _publishEndpoint;

        public OrderService(OrchestrationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<OrderState> SubmitOrder(List<OrderItem> orderItems)
        {
            var orderState = new OrderState(NewId.NextGuid(), DateTime.UtcNow, orderItems.Sum(q => q.Count * q.Price));

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