namespace BlazorFrontend.Shared.Services;

public static class AvatarService
{
	public static string GetAvatarUrl(byte[]? avatar)
	{
		if (avatar != null)
		{
			return $"data:image/png;base64,{Convert.ToBase64String(avatar)}";
		}
		else
		{
			return "default-avatar.png";
		}
	}
	public static byte[]? ConvertToByteArray(string? base64String)
	{
		if (base64String != null)
		{
			return Convert.FromBase64String(base64String);
		}
		else
		{
			return null;
		}
	}
}