using MediatR;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Application.Extensions;
using Product.Application.Interfaces;
using BuildingBlocks.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Product.Application.Queries
{   
    public record Product(Guid Id,string Name, decimal Price, int Stock);
    public record GetProductsByIdsQuery :  IRequest<List<Product>>
    {
        public List<Guid> ProductIds { get; init; }

        public GetProductsByIdsQuery(List<Guid> productIds)
        {
            ProductIds = productIds;
        }
    }
    public class GetProductsByIdsQueryHandler : IRequestHandler<GetProductsByIdsQuery, List<Product>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetProductsByIdsQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<Product>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var products = await _applicationDbContext.Products.Where(q=> request.ProductIds.Contains(q.Id)).Select(q => new Product(q.Id, q.Name, q.Price, q.Stock)).ToListAsync(cancellationToken);

            if (products == null)
            {
                throw new NotFoundException(nameof(products), request.ProductIds);
            }
            return products;
        }
    }
}
