using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;
namespace Product.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : BaseConfiguration<Product.Core.Entities.Product>
    {
        public override void Configure(EntityTypeBuilder<Product.Core.Entities.Product> builder)
        {
            base.Configure(builder);
            builder.Property(b => b.Name).HasMaxLength(Constants.Product.NameMaximumLength).IsRequired(Constants.Product.NameRequired);
            
        }        
    }
}
