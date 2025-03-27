using Frontend.Server.Services;
using Frontend.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Server.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
	private readonly INotificationService _notificationService = notificationService;

	[HttpPost("friendrequest")]
	public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto friendRequestDto)
	{
		if (friendRequestDto == null)
		{
			return BadRequest("Invalid request data.");
		}

		await _notificationService.CreateFriendRequestAsync(friendRequestDto);

		return Ok("Friend request sent.");
	}

	[HttpPost("friendrequest/confirm")]
	public async Task<IActionResult> ConfirmFriendRequest([FromBody] FriendRequestDto friendRequestDto)
	{
		if (friendRequestDto == null)
		{
			return BadRequest("Invalid request data.");
		}

		await _notificationService.ConfirmFriendRequestAsync(friendRequestDto);

		return Ok("Friend request confirmed.");
	}

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetUserNotifications(string userId)
	{
		var notifications = await _notificationService.GetUserNotificationsAsync(userId);

		return Ok(notifications);
	}

	[HttpPost("read")]
	public async Task<IActionResult> MarkNotificationAsRead(string recieverId, string notificationId)
	{
		await _notificationService.MarkAsReadAsync(recieverId, notificationId);

		return Ok();
	}
}