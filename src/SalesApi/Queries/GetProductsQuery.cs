using MediatR;
using SalesApi.Domain.Repositories;
using SalesApi.Dtos;

namespace SalesApi.Queries;

public record GetProductsQuery : IRequest<ProductResponse>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ProductResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAll();
        return new ProductResponse(products);
    }
}