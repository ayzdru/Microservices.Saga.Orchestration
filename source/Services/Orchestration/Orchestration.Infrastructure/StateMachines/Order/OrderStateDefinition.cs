using MassTransit;
using Orchestration.Infrastructure.Data;

namespace Orchestration.Infrastructure.StateMachines.Order
{
    public class OrderStateDefinition : SagaDefinition<OrderStateMachineInstance>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
            ISagaConfigurator<OrderStateMachineInstance> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

            endpointConfigurator.UseEntityFrameworkOutbox<OrchestrationDbContext>(context);
        }
    }
}