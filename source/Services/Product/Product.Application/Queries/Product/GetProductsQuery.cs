using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Queries
{   
    public record GetProductsQueryResponse(Guid Id, string Name, decimal Price, int Stock);
    public record GetProductsQuery :  IRequest<List<GetProductsQueryResponse>>
    {
        
    }
    public class GetTodoListsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsQueryResponse>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetTodoListsQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Products.Select(q => new GetProductsQueryResponse(q.Id, q.Name, q.Price, q.Stock)).ToListAsync(cancellationToken);
        }
    }
}
