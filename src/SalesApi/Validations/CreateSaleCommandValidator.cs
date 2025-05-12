using FluentValidation;
using SalesApi.Commands;

namespace SalesApi.Validations;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.SaleDate).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleForEach(x => x.Items).SetValidator(new CreateSaleCommandItemValidator());
    }
}

public class CreateSaleCommandItemValidator : AbstractValidator<CreateSaleCommandItem>
{
    public CreateSaleCommandItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}