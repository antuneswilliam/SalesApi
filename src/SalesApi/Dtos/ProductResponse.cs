using SalesApi.Domain.Entities;

namespace SalesApi.Dtos;

public record ProductResponse(List<Product> Data, string Message = "", string Status = "success");