using Frontend.Server.Data;
using Frontend.Server.Models;
using Frontend.Shared.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Server.Services;

public class NotificationService(UserDbContext context, UserService userService)
{
	private readonly UserDbContext _context = context;
	private readonly UserService _userService = userService;

	public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
	{
		var userNotifications = await _context.UserNotifications.Where(un => un.Receiver.Id == userId).ToListAsync();

		List<NotificationDto> notificationDtos = [];
		if (userNotifications.Count != 0)
		{
			foreach (var un in userNotifications)
			{
				notificationDtos.Add(new(
					un.NotificationId.ToString(),
					un.Notification.Sender,
					un.Notification.Message,
					un.IsRead,
					un.Notification.CreatedAt
				));
			}
		}
		return notificationDtos;
	}

	public async Task CreateFriendRequestAsync(FriendRequestDto friendRequestDto)
	{
		var sender = await _userService.GetUserByIdAsync(friendRequestDto.SenderId);
		var message = $"New friend request from {sender?.UserName}!";

		var newNotification = new Notification() { Sender = friendRequestDto.SenderId, Message = message };
		var newUserNotification = new UserNotification() { ReceiverId = friendRequestDto.ReceiverId, Notification = newNotification };

		await _context.UserNotifications.AddAsync(newUserNotification);
		await _context.SaveChangesAsync();
	}

	public async Task ConfirmFriendRequestAsync(FriendRequestDto friendRequestDto)
	{
		var sender = await _userService.GetUserByIdAsync(friendRequestDto.SenderId);
		var receiver = await _userService.GetUserByIdAsync(friendRequestDto.ReceiverId);

		if (sender is not null && receiver is not null)
		{
			sender.Friends.Add(receiver);
			receiver.Friends.Add(sender);
			await _context.SaveChangesAsync();
		}
	}

	public async Task MarkAsReadAsync(string recieverId, string notificationId)
	{
		var userNotification = await _context.UserNotifications.FindAsync(recieverId, notificationId);
		if (userNotification != null)
		{
			userNotification.IsRead = true;
			await _context.SaveChangesAsync();
		}
	}
}
