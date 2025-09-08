using MediatR;
using Product.Application.Commands;
using Product.Application.Interfaces;
using Product.Application.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Commands
{
    public record CreateProductCommand : IRequest<Guid?>
    {
        public required string Name { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }

    }
    public class CreateTodoListCommandHandler : IRequestHandler<CreateProductCommand, Guid?>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public CreateTodoListCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Guid?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            var product = new Core.Entities.Product(request.Name, request.Price, request.Stock);
            _applicationDbContext.Products.Add(product);
            product.AddNotification(new ProductDeletedNotification (product));
            var affected = await _applicationDbContext.SaveChangesAsync(cancellationToken);
            if (affected != 0)
            {
                await transaction.CommitAsync();
                return product.Id;
            }
            return null;
        }
    }
}
