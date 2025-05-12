using FluentValidation;
using SalesApi.Commands;

namespace SalesApi.Validations;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        
        RuleFor(x => x.Description).NotEmpty();
        
        RuleFor(x => x.Price).GreaterThan(0);
        
        RuleFor(x => x.Category).NotEmpty();
        
        RuleFor(x => x.Image).NotEmpty();
    }
}