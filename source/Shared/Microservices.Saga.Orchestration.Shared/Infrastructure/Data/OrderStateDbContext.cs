using MassTransit;
using MassTransit.Configuration;
using MassTransit.EntityFrameworkCoreIntegration;
using Microservices.Saga.Orchestration.Shared.Entities;
using Microservices.Saga.Orchestration.Shared.StateMachines.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Infrastructure.Data
{
    public class OrderStateDbContext :
    SagaDbContext
    {
        public DbSet<Entities.OrderState> OrderStates { get; set; }
        public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options)
            : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            MapOrder(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }

        static void MapOrder(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Entities.OrderState> order = modelBuilder.Entity<Entities.OrderState>();

            order.Property(x => x.Id);
            order.HasKey(x => x.Id);

            order.Property(x => x.OrderId);
            order.Property(x => x.OrderDate);
            order.Property(x => x.TotalPrice);

            order.HasIndex(x => new
            {
                x.OrderId
            }).IsUnique();
        }
    }
}