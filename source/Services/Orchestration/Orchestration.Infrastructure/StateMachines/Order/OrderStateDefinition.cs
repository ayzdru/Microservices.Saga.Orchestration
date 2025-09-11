namespace Orchestration.Infrastructure.StateMachines.Order;

using MassTransit;
using Orchestration.Infrastructure.Data;

public class OrderStateDefinition :
    SagaDefinition<OrderStateInstance>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<OrderStateInstance> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        //endpointConfigurator.UseEntityFrameworkOutbox<OrchestrationSagaDbContext>(context);
    }
}