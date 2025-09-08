using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Extensions;
using Product.Application.Interfaces;
using Product.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Queries
{   
    public record GetProductByIdQueryResponse(Guid Id, string Name, decimal Price, int Stock);
    public record GetProductByIdQuery :  IRequest<GetProductByIdQueryResponse>
    {
        public Guid Id { get; init; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetProductByIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products.GetById(request.Id).Select(q => new GetProductByIdQueryResponse(q.Id, q.Name, q.Price, q.Stock)).SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new NotFoundException(nameof(product), request.Id);
            }
            return product;
        }
    }
}
