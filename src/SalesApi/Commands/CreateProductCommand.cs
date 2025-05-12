using AutoMapper;
using FluentValidation;
using MediatR;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Repositories;
using SalesApi.Dtos;
using SalesApi.Queries;

namespace SalesApi.Commands;

public record CreateProductCommand(
    string Title,
    string Description,
    decimal Price,
    string Category,
    string Image
) : IRequest<ProductResponse>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductCommandHandler(IProductRepository productRepository,
        IMapper mapper,
        IValidator<CreateProductCommand> validator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
            return new ProductResponse([], string.Join(';', errors), "error");
        }
        
        var product = _mapper.Map<Product>(request);
        await _productRepository.Create(product);
        return new ProductResponse([product]);
    }
}