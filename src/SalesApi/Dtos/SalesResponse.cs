using SalesApi.Domain.Entities;

namespace SalesApi.Dtos;

public record SalesResponse(List<Sale> Data, string Message = "", string Status = "success");