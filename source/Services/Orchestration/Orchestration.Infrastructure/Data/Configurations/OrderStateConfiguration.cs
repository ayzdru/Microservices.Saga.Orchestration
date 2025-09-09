using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestration.Core.Entities;
using Orchestration.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Infrastructure.Data.Configurations
{
    public class OrderStateConfiguration : BaseConfiguration<OrderState>
    {
        public override void Configure(EntityTypeBuilder<OrderState> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => new
            {
                x.OrderId
            }).IsUnique();
        }        
    }
}
