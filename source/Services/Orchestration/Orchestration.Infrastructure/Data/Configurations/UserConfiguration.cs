using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestration.Core.Entities;
using Orchestration.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Infrastructure.Data.Configurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);           
        }        
    }
}
