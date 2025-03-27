using Frontend.Shared.Models.Dtos;

namespace Frontend.Server.Services;

public interface INotificationService
{
	Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
	Task CreateFriendRequestAsync(FriendRequestDto friendRequestDto);
	Task ConfirmFriendRequestAsync(FriendRequestDto friendRequestDto);
	Task MarkAsReadAsync(string recieverId, string notificationId);
}
