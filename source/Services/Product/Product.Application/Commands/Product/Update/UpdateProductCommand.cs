using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Application.Notifications;
using Product.Core.Exceptions;
using Product.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Commands
{    
    public record UpdateProductCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public UpdateProductCommand(Guid ıd, string name, decimal price, int stock)
        {
            Id = ıd;
            Name = name;
            Price = price;
            Stock = stock;
        }       
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateProductCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product.Core.Entities.Product), request.Id);
            }
            product.AddNotification(new ProductUpdatedNotification(product));
            product.Update(request.Name, request.Price, request.Stock);
            _applicationDbContext.Products.Update(product);
            var affected = await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return affected >= 1;
        }
    }
}
