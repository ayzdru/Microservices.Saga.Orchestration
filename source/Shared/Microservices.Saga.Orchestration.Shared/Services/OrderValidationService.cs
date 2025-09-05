using MassTransit;
using Microservices.Saga.Orchestration.Shared.Interfaces;
using Sample.Components.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Services
{
    internal class OrderValidationService: IOrderValidationService
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
