using Bogus;
using FluentAssertions;
using NSubstitute;
using SalesApi.Domain.Entities;

namespace SalesApi.Tests.Domain;

public class SaleTests
{
    private readonly Faker<SaleItem> _saleItemFaker;
    private readonly Faker<Sale> _saleFaker;
    private readonly Faker<Product> _productFaker;

    public SaleTests()
    {
        _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Finance.Amount())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1).FirstOrDefault())
            .RuleFor(p => p.Image, f => Guid.NewGuid().ToString());
        
        _saleItemFaker = new Faker<SaleItem>()
            .RuleFor(si => si.Id, f => f.Random.Guid())
            .RuleFor(si => si.SaleId, f => f.Random.Guid())
            .RuleFor(si => si.ProductId, f => f.Random.Guid())
            .RuleFor(si => si.Product, f => _productFaker.Generate())
            .RuleFor(si => si.Quantity, f => f.Random.Decimal(1, 15))
            .RuleFor(si => si.UnitPrice, f => f.Random.Decimal(5, 100))
            .RuleFor(si => si.ValueMonetaryTaxApplied, f => f.Random.Decimal(0, 20));

        _saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.Date, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => f.Random.Guid())
            .RuleFor(s => s.BranchId, f => f.Random.Guid())
            .RuleFor(s => s.Canceled, f => f.Random.Bool())
            .RuleFor(s => s.Items, f => _saleItemFaker.Generate(f.Random.Number(1, 5)));
    }

    [Fact]
    public void TotalAmount_ShouldReturnSumOfItemTotals()
    {
        // Arrange
        var sale = _saleFaker.Generate();
        var expectedTotal = sale.Items.Sum(item => item.Total);

        // Act
        var totalAmount = sale.TotalAmount;

        // Assert
        totalAmount.Should().Be(expectedTotal);
    }

    [Fact]
    public void CalculateTaxes_ShouldCallCalculateTaxOnAllItems()
    {
        // Arrange
        var item1 = Substitute.For<SaleItem>();
        var item2 = Substitute.For<SaleItem>();
        var sale = new Sale
        {
            Items = [item1, item2]
        };

        // Act
        sale.CalculateTaxes();

        // Assert
        item1.Received(1).CalculateTax();
        item2.Received(1).CalculateTax();
    }
}