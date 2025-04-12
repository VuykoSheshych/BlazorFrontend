using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorFrontend;
using BlazorFrontend.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorFrontend.Features.Auth.Services;
using MudBlazor.Services;
using BlazorFrontend.Features.Game.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string? usersAndAuthApiUrl;
string? gamePlayApiUrl;

if (builder.HostEnvironment.Environment == "Development")
{
	usersAndAuthApiUrl = "https://localhost:7187";
	gamePlayApiUrl = "https://localhost:7251";
}
else
{
	usersAndAuthApiUrl = builder.HostEnvironment.BaseAddress;
	gamePlayApiUrl = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddHttpClient("UsersAndAuthAPI", client =>
{
	client.BaseAddress = new Uri(usersAndAuthApiUrl);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
	// Cookies are not supported in WASM, so this is omitted.
	// Add any other necessary configuration here if needed.
})
.ConfigureAdditionalHttpMessageHandlers((handlers, serviceProvider) =>
{
	handlers.Add(new IncludeCredentialsHandler());
});

builder.Services.AddHttpClient("GamePlayAPI", client =>
{
	client.BaseAddress = new Uri(gamePlayApiUrl);
});

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped<UserSharedService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GameHubClient>();
builder.Services.AddScoped<GameRecordService>();
builder.Services.AddSingleton<ThemeService>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("UsersAndAuthAPI"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GamePlayAPI"));

builder.Services.AddMudServices();

await builder.Build().RunAsync();