using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BuildingBlocks.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;
namespace Order.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : BaseConfiguration<Order.Core.Entities.Order>
    {
        public override void Configure(EntityTypeBuilder<Order.Core.Entities.Order> builder)
        {
            base.Configure(builder);
            builder.Property(o => o.UserId).IsRequired();
            builder.Property(b => b.ErrorMessage).HasMaxLength(Constants.Order.ErrorMessageMaximumLength).IsRequired(Constants.Order.ErrorMessageRequired);
            builder.Navigation(n => n.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
        }        
    }
}
