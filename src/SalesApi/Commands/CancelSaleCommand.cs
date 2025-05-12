using MediatR;
using SalesApi.Domain.Repositories;
using SalesApi.Dtos;
using SalesApi.Queries;

namespace SalesApi.Commands;

public record CancelSaleCommand(Guid Id) : IRequest<SalesResponse>;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, SalesResponse>
{
    private readonly ISaleRepository _saleRepository;

    public CancelSaleCommandHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }
    
    public async Task<SalesResponse> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sale = await _saleRepository.FindAsync(request.Id);

            if (sale == null)
                return new SalesResponse([], "Not Found", "error");
            
            sale.Canceled = true;

            await _saleRepository.UpdateAsync(sale);

            return new SalesResponse([], "Sell canceled");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new SalesResponse([], "Error deleting sale", "error");
        }
    }
}