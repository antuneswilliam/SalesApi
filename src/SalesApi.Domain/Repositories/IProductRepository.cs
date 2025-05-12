using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Product> Create(Product product);
    Task<Product?> Get(Guid id);
}