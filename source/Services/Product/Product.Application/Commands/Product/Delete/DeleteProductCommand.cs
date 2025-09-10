using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Application.Notifications;
using BuildingBlocks.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Commands
{    
    public record DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteProductCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(product), request.Id);
            }

            _applicationDbContext.Products.Remove(product);
            product.AddNotification(new ProductDeletedNotification(product));
            var affected = await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return (affected >= 1);
        }
    }
}
