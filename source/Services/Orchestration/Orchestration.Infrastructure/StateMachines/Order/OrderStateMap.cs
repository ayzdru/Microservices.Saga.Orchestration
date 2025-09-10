using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orchestration.Infrastructure.StateMachines.Order;

public class OrderStateMap: SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        
    }
}