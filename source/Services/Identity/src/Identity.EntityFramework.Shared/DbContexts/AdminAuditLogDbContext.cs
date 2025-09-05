using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Identity.AuditLogging.EntityFramework.DbContexts;
using Identity.AuditLogging.EntityFramework.Entities;

namespace Identity.EntityFramework.Shared.DbContexts;

public class AdminAuditLogDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
{
    public AdminAuditLogDbContext(DbContextOptions<AdminAuditLogDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

    public DbSet<AuditLog> AuditLog { get; set; }
}