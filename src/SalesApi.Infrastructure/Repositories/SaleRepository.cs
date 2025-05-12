using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Repositories;

namespace SalesApi.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetAll()
    {
        return await _context.Sales
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<Sale> Create(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<Sale?> FindAsync(Guid id)
    {
        return await _context.Sales.FindAsync(id);
    }
}