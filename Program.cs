using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorFrontend;
using BlazorFrontend.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorFrontend.Features.Auth.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("UsersAndAuth", client =>
{
	client.BaseAddress = new Uri("https://localhost:7187/api/");
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

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped<UserSharedService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<ThemeService>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("UsersAndAuth"));

builder.Services.AddMudServices();

await builder.Build().RunAsync();