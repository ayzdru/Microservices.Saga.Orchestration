using MassTransit;
using Microservices.Saga.Orchestration.Shared.Interfaces;
using Orchestration.Core.Interfaces;
using Orchestration.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.Services
{
    public class OrderValidationService: IOrderValidationService
    {
        readonly IPublishEndpoint _publishEndpoint;

        public OrderValidationService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task ValidateOrder(Guid orderId)
        {
            await _publishEndpoint.Publish(new OrderValidated { OrderId = orderId });
        }
    }
}
