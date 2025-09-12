using BuildingBlocks.Core.Models;
using BuildingBlocks.EventBus.Interfaces.Order;
using BuildingBlocks.EventBus.Messages.Order;
using BuildingBlocks.MassTransit.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces;
using Order.Application.Services;
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
    public record CreateOrderCommand : IRequest<CreateOrderCommandResponse>
    {
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
    public record CreateOrderCommandResponse(ApiResult<string> ApiResult, CreateOrderMessage CreateOrderMessage);
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext; 
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly ApiGatewayService _apiGatewayService;
        public CreateOrderCommandHandler(IApplicationDbContext applicationDbContext, ILogger<CreateOrderCommandHandler> logger, ApiGatewayService apiGatewayService)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _apiGatewayService = apiGatewayService;
        }
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.OrderItems.Select(x => x.ProductId).ToList();
            var products = await _apiGatewayService.GetProductsByIdsAsync(productIds, cancellationToken);
            if(products==null || products.Count != request.OrderItems.Count)
            {
                return new CreateOrderCommandResponse(new ApiResult<string>(false, "Order creation failed due to missing product details."), null);
            }
            var newOrder = new Core.Entities.Order(request.UserId, OrderStatus.Pending, request.OrderItems.Select(item => new Core.Entities.OrderItem(item.ProductId, products.SingleOrDefault(p => p.Id == item.ProductId).Price, item.Count)).ToList());
           
            await _applicationDbContext.Orders.AddAsync(newOrder, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var createOrderMessage = new CreateOrderMessage()
            {
                OrderId = newOrder.Id,
                UserId = request.UserId,                
                TotalPrice = newOrder.OrderItems.Sum(x => x.Price * x.Count),
                OrderItems = newOrder.OrderItems.Select(item => new BuildingBlocks.EventBus.Models.Order.OrderItem
                {
                    Price = item.Price,
                    Count = item.Count,
                    ProductId = item.ProductId
                }).ToList()
            };


            _logger.LogInformation("Order with Id: {NewOrderId} created successfully", newOrder.Id);

            return new CreateOrderCommandResponse(new ApiResult<string>(true, "Order created successfully"), createOrderMessage);
        }
    }
}
