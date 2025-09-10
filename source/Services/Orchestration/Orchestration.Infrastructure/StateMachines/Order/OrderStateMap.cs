using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orchestration.Infrastructure.StateMachines.Order;

public class OrderStateMap: SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);

        entity.Property(x => x.UserId)
               .IsRequired();

        entity.Property(x => x.OrderId)
            .IsRequired();

        entity.Property(x => x.TotalPrice)
            .HasDefaultValue(0);
    }
}