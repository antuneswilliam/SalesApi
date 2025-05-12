using SalesApi.Commands;
using SalesApi.Domain.Entities;

namespace SalesApi.Profiles;

public class Profile : AutoMapper.Profile
{
    public Profile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<CreateSaleCommand, Sale>();
        CreateMap<CreateSaleCommandItem, SaleItem>();
    }
}