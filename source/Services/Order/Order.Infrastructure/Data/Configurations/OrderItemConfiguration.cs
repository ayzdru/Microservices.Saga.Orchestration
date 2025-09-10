using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BuildingBlocks.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;
namespace Order.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : BaseConfiguration<Order.Core.Entities.OrderItem>
    {
        public override void Configure(EntityTypeBuilder<Order.Core.Entities.OrderItem> builder)
        {
            base.Configure(builder);
           
        }        
    }
}
