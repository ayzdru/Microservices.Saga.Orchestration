using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestration.Application.Interfaces;
using Orchestration.Core;
using Orchestration.Core.Entities;
using Orchestration.Infrastructure.Extensions;
using Orchestration.Infrastructure.Interceptors;
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
        public DbSet<OrderState> OrderStates { get; set; }
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
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
        }
    }
}
