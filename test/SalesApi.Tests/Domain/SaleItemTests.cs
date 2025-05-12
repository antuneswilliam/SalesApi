using Bogus;
using FluentAssertions;
using SalesApi.Domain.Entities;

namespace SalesApi.Tests.Domain;

public class SaleItemTests
{
    private readonly Faker<SaleItem> _saleItemFaker;
    private readonly Faker<Product> _productFaker;

    public SaleItemTests()
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
            .RuleFor(si => si.UnitPrice, f => f.Random.Decimal(5, 100))
            .RuleFor(si => si.ValueMonetaryTaxApplied, f => f.Random.Decimal(0, 20));
    }

    [Fact]
    public void Total_ShouldReturnSumOfUnitPriceTimesQuantityAndTax()
    {
        // Arrange
        var quantity = new Faker().Random.Decimal(1, 15);
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = quantity;
        var expectedTotal = (saleItem.UnitPrice * saleItem.Quantity) + saleItem.ValueMonetaryTaxApplied;

        // Act
        var total = saleItem.Total;

        // Assert
        total.Should().Be(expectedTotal);
    }

    [Fact]
    public void CalculateTax_WithQuantityLessThan4_ShouldSetTaxToZero()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = new Faker().Random.Decimal(1, 3);

        // Act
        saleItem.CalculateTax();

        // Assert
        saleItem.ValueMonetaryTaxApplied.Should().Be(0);
    }

    [Fact]
    public void CalculateTax_WithQuantityBetween4And9_ShouldApplyIvaTax()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = new Faker().Random.Decimal(4, 9);
        var expectedTax = saleItem.Quantity * saleItem.UnitPrice * 0.1m;

        // Act
        saleItem.CalculateTax();

        // Assert
        saleItem.ValueMonetaryTaxApplied.Should().Be(expectedTax);
    }

    [Fact]
    public void CalculateTax_WithQuantityBetween10And20_ShouldApplySpecialIvaTax()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = new Faker().Random.Decimal(10, 20);
        var expectedTax = saleItem.Quantity * saleItem.UnitPrice * 0.2m;

        // Act
        saleItem.CalculateTax();

        // Assert
        saleItem.ValueMonetaryTaxApplied.Should().Be(expectedTax);
    }

    [Fact]
    public void CalculateTax_WithQuantityGreaterThan20_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = new Faker().Random.Decimal(21, 30);

        // Act
        Action act = () => saleItem.CalculateTax();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("Quantity");
    }

    [Fact]
    public void CalculateTax_ShouldNotThrowException_WhenQuantityIsExactly20()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = 20;
        var expectedTax = 20 * saleItem.UnitPrice * 0.2m;

        // Act
        saleItem.CalculateTax();

        // Assert
        saleItem.ValueMonetaryTaxApplied.Should().Be(expectedTax);
    }

    [Fact]
    public void CalculateTax_ShouldNotThrowException_WhenQuantityIsExactly4()
    {
        // Arrange
        var saleItem = _saleItemFaker.Generate();
        saleItem.Quantity = 4;
        var expectedTax = 4 * saleItem.UnitPrice * 0.1m;

        // Act
        saleItem.CalculateTax();

        // Assert
        saleItem.ValueMonetaryTaxApplied.Should().Be(expectedTax);
    }
}