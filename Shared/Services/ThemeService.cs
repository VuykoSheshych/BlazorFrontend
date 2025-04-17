using MudBlazor;

namespace BlazorFrontend.Shared.Services;

public class ThemeService
{
	public event Action? OnThemeChanged;
	public bool IsDarkMode { get; private set; } = false;
	public readonly MudTheme CustomTheme = new()
	{
		PaletteLight = new PaletteLight()
		{
			Primary = Colors.Green.Darken2,
			Secondary = Colors.Green.Accent4,
			AppbarBackground = Colors.Green.Darken3,
			AppbarText = Colors.Shades.White,
			DrawerBackground = Colors.Green.Lighten5,
			DrawerText = Colors.Green.Darken4,
			Background = Colors.Shades.White,
			Surface = Colors.Green.Lighten5,
			TextPrimary = Colors.Green.Darken4,
			TextSecondary = Colors.Green.Darken2
		},

		PaletteDark = new PaletteDark()
		{
			Primary = Colors.DeepPurple.Accent2,
			Secondary = Colors.Purple.Lighten2,
			AppbarBackground = "#1E1E2F",
			AppbarText = Colors.Shades.White,
			DrawerBackground = "#1A1A2E",
			DrawerText = Colors.Shades.White,
			Background = "#121212",
			Surface = "#1E1E2F",
			TextPrimary = Colors.Shades.White,
			TextSecondary = Colors.Gray.Lighten1,
			Black = "#0D0D0D"
		}
	};

	public void ToggleTheme()
	{
		IsDarkMode = !IsDarkMode;
		OnThemeChanged?.Invoke();
	}
}
