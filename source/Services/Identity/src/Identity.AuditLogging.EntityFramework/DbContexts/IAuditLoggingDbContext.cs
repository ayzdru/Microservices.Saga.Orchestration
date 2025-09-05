using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Identity.AuditLogging.EntityFramework.Entities;

namespace Identity.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext<TAuditLog> where TAuditLog : AuditLog
    {
        DbSet<TAuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}