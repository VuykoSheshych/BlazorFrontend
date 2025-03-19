namespace Frontend.Shared.Models.Dtos;

public record PlayerDto(string Name, string ConnectionId, TimeSpan? TimeReserve = null) { }