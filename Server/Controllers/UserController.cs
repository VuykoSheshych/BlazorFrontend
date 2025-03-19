using Frontend.Server.Services;
using Frontend.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Server.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService, NotificationService notificationService) : ControllerBase
{
	private readonly UserService _userService = userService;
	private readonly NotificationService _notificationService = notificationService;

	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await _userService.GetUsersAsync();

		List<UserDto> userDtos = [];
		foreach (var user in users)
		{
			userDtos.Add(UserService.CreateUserDto(user));
		}

		return Ok(userDtos);
	}

	[HttpGet("id-{id}")]
	public async Task<IActionResult> GetUserById(string id)
	{
		var userDto = await _userService.GetUserByIdAsync(id);

		if (userDto is null) return BadRequest("User not found");

		return Ok(UserService.CreateUserDto(userDto));
	}

	[HttpGet("username-{userName}")]
	public async Task<IActionResult> GetUserByUsername(string userName)
	{
		var userDto = await _userService.GetUserByUserNameAsync(userName);

		if (userDto is null) return BadRequest("User not found");

		return Ok(UserService.CreateUserDto(userDto));
	}

	[HttpPost("friendrequests")]
	public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto friendRequestDto)
	{
		Console.WriteLine($"friendRequestDto.ReceiverId = {friendRequestDto.ReceiverId}");
		Console.WriteLine($"friendRequestDto.SenderId = {friendRequestDto.SenderId}");

		if (friendRequestDto == null)
		{
			return BadRequest("Invalid request data.");
		}

		await _notificationService.CreateFriendRequestAsync(friendRequestDto);

		return Ok("Friend request sent.");
	}

	[HttpPost("friendrequests/confirm")]
	public async Task<IActionResult> ConfirmFriendRequest([FromBody] FriendRequestDto friendRequestDto)
	{
		if (friendRequestDto == null)
		{
			return BadRequest("Invalid request data.");
		}

		await _notificationService.ConfirmFriendRequestAsync(friendRequestDto);

		return Ok("Friend request confirmed.");
	}

	[HttpGet("notifications/{userId}")]
	public async Task<IActionResult> GetUserNotifications(string userId)
	{
		var notifications = await _notificationService.GetUserNotificationsAsync(userId);

		return Ok(notifications);
	}

	[HttpPost("notifications/{recieverId}")]
	public async Task<IActionResult> MarkNotificationAsRead(string recieverId, string notificationId)
	{
		await _notificationService.MarkAsReadAsync(recieverId, notificationId);

		return Ok();
	}
}