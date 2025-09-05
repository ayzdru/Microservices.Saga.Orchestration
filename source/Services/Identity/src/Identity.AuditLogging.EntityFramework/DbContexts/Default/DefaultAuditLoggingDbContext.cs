using Microsoft.EntityFrameworkCore;
using Identity.AuditLogging.EntityFramework.Entities;

namespace Identity.AuditLogging.EntityFramework.DbContexts.Default
{
    public class DefaultAuditLoggingDbContext : AuditLoggingDbContext<AuditLog>
    {
        public DefaultAuditLoggingDbContext(DbContextOptions<DefaultAuditLoggingDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {

        }
    }
}