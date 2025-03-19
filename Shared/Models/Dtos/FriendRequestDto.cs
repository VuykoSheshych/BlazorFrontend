namespace Frontend.Shared.Models.Dtos;

public record FriendRequestDto(string ReceiverId, string SenderId, DateTime CreatedAt) { }