using BuildingBlocks.Core.Entities;
using BuildingBlocks.Infrastructure.Data.Configurations;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestration.Application.Interfaces;
using Orchestration.Infrastructure.StateMachines.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orchestration.Infrastructure.Data
{
    public class OrchestrationDbContext : SagaDbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public OrchestrationDbContext(DbContextOptions<OrchestrationDbContext> options)
           : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
        }
    }
}
