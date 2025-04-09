using MudBlazor;

namespace BlazorFrontend.Shared.Services;

public class ThemeService
{
	public event Action? OnThemeChanged;
	public bool IsDarkMode { get; private set; } = false;
	public readonly MudTheme LightTheme = new()
	{
		PaletteLight = new PaletteLight()
		{
			Primary = Colors.Purple.Default,
			Secondary = Colors.Green.Accent4
		}
	};
	public readonly MudTheme DarkTheme = new()
	{
		PaletteDark = new PaletteDark()
		{
			Black = "#27272f",
			Background = Colors.Gray.Darken4,
			Surface = "#373740",
			Primary = Colors.Indigo.Lighten1,
			Secondary = Colors.Pink.Accent2,
			AppbarBackground = "#27272f",
			DrawerBackground = "#27272f"
		}
	};

	public void ToggleTheme()
	{
		IsDarkMode = !IsDarkMode;
		OnThemeChanged?.Invoke();
	}
}
