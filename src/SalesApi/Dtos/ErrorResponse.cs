namespace SalesApi.Dtos;

public record ErrorResponse(string Type, string Error, string Detail);