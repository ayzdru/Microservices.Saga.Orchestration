using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BuildingBlocks.Core.Entities;
using BuildingBlocks.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Infrastructure.Data.Configurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);           
        }        
    }
}
