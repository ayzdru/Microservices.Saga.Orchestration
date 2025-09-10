using Product.Core;
using BuildingBlocks.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using BuildingBlocks.Infrastructure.Common;

namespace BuildingBlocks.Infrastructure.Common
{
    public abstract class BaseConfiguration<T> : BaseAuditableConfiguration<T>, IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
        }
    }
}
