namespace SalesApi.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount => Items.Sum(item => item.Total);
    public Guid BranchId { get; set; }
    public bool Canceled { get; set; }
    public List<SaleItem> Items { get; set; }

    public void CalculateTaxes()
    {
        Items.ForEach(item => item.CalculateTax());
    }
}