using MediatR;
using SalesApi.Domain.Repositories;
using SalesApi.Dtos;

namespace SalesApi.Queries;

public record GetSalesQuery : IRequest<SalesResponse>;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, SalesResponse>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
    
    public async Task<SalesResponse> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetAll();
        return new SalesResponse(sales);
    }
}