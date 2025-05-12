using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Repositories;

public interface ISaleRepository
{
    Task<List<Sale>> GetAll();
    Task<Sale> Create(Sale sale);
    Task UpdateAsync(Sale sale);
    Task<Sale?> FindAsync(Guid id);
}