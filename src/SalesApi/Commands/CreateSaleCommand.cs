using AutoMapper;
using FluentValidation;
using MediatR;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Repositories;
using SalesApi.Dtos;
using SalesApi.Queries;

namespace SalesApi.Commands;

public record CreateSaleCommand(
    int SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    Guid BranchId,
    List<CreateSaleCommandItem> Items
) : IRequest<SalesResponse>;

public class CreateSaleCommandItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SalesResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateSaleCommand> _validator;
    private readonly IProductRepository _productRepository;

    public CreateSaleCommandHandler(ISaleRepository saleRepository,
        IMapper mapper,
        IValidator<CreateSaleCommand> validator,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _validator = validator;
        _productRepository = productRepository;
    }

    public async Task<SalesResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        
        var errors = await ValidateProducts(request);
        
        if (!result.IsValid)
        {
            errors.AddRange(
                result.Errors.Select(x => x.ErrorMessage).ToList());
        }
        
        if (errors.Count != 0)
            return new SalesResponse([], string.Join("; ", errors), "error");

        var sale = _mapper.Map<Sale>(request);

        try
        {
            sale.CalculateTaxes();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return new SalesResponse([],
                "You can buy only 20 pices of a item / You cannot buy more than 20 pices of same item", "error");
        }

        var created = await _saleRepository.Create(sale);
        return new SalesResponse([created]);
    }

    private async Task<List<string>> ValidateProducts(CreateSaleCommand request)
    {
        var nonExistingProducts = new List<string>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.Get(item.ProductId);
            if (product == null)
                nonExistingProducts.Add($"ProductId {item.ProductId} not found");
        }

        return nonExistingProducts;
    }
}