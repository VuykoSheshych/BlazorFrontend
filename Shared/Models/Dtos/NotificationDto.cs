namespace Frontend.Shared.Models.Dtos;

public record NotificationDto(string Id, string Sender, string Message, bool IsRead, DateTime CreatedAt) { }