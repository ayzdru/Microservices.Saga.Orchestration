using Order.Application.Interfaces;
using Order.Core;
using Order.Core.Entities;
using Order.Infrastructure.Extensions;
using Order.Infrastructure.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Order.Core.Entities.Order> Orders { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
             : base(options)
        {

        }      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }     

    }
}
