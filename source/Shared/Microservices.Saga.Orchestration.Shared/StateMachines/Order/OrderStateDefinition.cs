using MassTransit;
using Microservices.Saga.Orchestration.Shared.Infrastructure.Data;

namespace Microservices.Saga.Orchestration.Shared.StateMachines.Order
{
    public class OrderStateDefinition : SagaDefinition<OrderState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
            ISagaConfigurator<OrderState> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

            endpointConfigurator.UseEntityFrameworkOutbox<OrderStateDbContext>(context);
        }
    }
}