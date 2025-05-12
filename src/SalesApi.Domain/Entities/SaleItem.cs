namespace SalesApi.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ValueMonetaryTaxApplied { get; set; }
    public decimal Total =>  (UnitPrice * Quantity) + ValueMonetaryTaxApplied;

    private const decimal IvaTax = 0.1m;
    private const decimal SpecialIvaTax = 0.2m;
    
    public void CalculateTax()
    {
        ValueMonetaryTaxApplied = Quantity switch
        {
            > 20 => throw new ArgumentOutOfRangeException(nameof(Quantity)),
            >= 10 => UnitPrice * Quantity * SpecialIvaTax,
            < 4 => 0,
            _ => UnitPrice * Quantity * IvaTax
        };
    }
}