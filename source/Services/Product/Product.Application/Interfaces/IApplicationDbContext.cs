using Product.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product.Core.Entities.Product> Products { get; set; }
        DbSet<User> Users { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}