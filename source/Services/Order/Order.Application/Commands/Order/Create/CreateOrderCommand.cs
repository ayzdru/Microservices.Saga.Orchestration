using BuildingBlocks.Core.Models;
using BuildingBlocks.EventBus.Interfaces.Order;
using BuildingBlocks.EventBus.Messages.Order;
using BuildingBlocks.MassTransit.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces;
using Order.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Application.Commands.Order.Create
{
    public record OrderItem
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
    public record CreateOrderCommand : IRequest<ApiResult<string>>
    {
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<string>>
    {
        private readonly IApplicationDbContext _applicationDbContext; 
        private readonly IMassTransitService _massTransitService;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        public CreateOrderCommandHandler(IApplicationDbContext applicationDbContext, IMassTransitService massTransitService, ILogger<CreateOrderCommandHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _massTransitService = massTransitService;
            _logger = logger;
        }
        public async Task<ApiResult<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new Core.Entities.Order(request.UserId, OrderStatus.Pending, request.OrderItems.Select(item => new Core.Entities.OrderItem(item.ProductId, item.Price, item.Count)).ToList());
           
            await _applicationDbContext.Orders.AddAsync(newOrder, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var createOrderMessage = new CreateOrderMessage()
            {
                OrderId = newOrder.Id,
                UserId = request.UserId,                
                TotalPrice = newOrder.OrderItems.Sum(x => x.Price * x.Count),
                OrderItems = newOrder.OrderItems.Select(item => new OrderItem
                {

                    Price = item.Price,
                    Count = item.Count,
                    ProductId = item.ProductId
                }).ToList()
            };

            await _massTransitService.Send<ICreateOrderMessage>(createOrderMessage, EventBusConstants.Queues.CreateOrderMessageQueueName);

            _logger.LogInformation("Order with Id: {NewOrderId} created successfully", newOrder.Id);

            return new ApiResult<string>(true, "Order created successfully");
        }
    }
}
