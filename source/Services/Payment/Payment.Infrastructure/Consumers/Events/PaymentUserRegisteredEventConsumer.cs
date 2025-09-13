using BuildingBlocks.Core.Entities;
using BuildingBlocks.EventBus.Events.User;
using BuildingBlocks.MassTransit.Consumers.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Consumers.Events
{
    public class PaymentUserRegisteredEventConsumer : UserRegisteredEventConsumer
    {
        public PaymentUserRegisteredEventConsumer(
            ILogger<UserRegisteredEventConsumer> logger,
            UserManager<User> userManager)
            : base(logger, userManager)
        {

        }
        public override Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            return base.Consume(context);
        }
    }
}
