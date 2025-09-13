using BuildingBlocks.Core.Entities;
using BuildingBlocks.EventBus.Interfaces.User;
using BuildingBlocks.MassTransit.Consumers.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.Consumers.Events
{
    public class OrchestrationUserRegisteredEventConsumer : UserRegisteredEventConsumer
    {
        public OrchestrationUserRegisteredEventConsumer(
            ILogger<UserRegisteredEventConsumer> logger,
            UserManager<User> userManager)
            : base(logger, userManager)
        {

        }
        public override Task Consume(ConsumeContext<IUserRegisteredEvent> context)
        {
            return base.Consume(context);
        }
    }
}
