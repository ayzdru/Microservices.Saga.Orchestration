using Payment.Application.Interfaces;
using Payment.Core;
using Payment.Core.Entities;
using Payment.Infrastructure.Extensions;
using Payment.Infrastructure.Interceptors;
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

namespace Payment.Infrastructure.Data
{
    public class OrchestrationDbContext : DbContext, IApplicationDbContext
    {
        public OrchestrationDbContext(DbContextOptions<OrchestrationDbContext> options)
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
