using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend.Client;
using Frontend.Client.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string blazorServerUrl;
string gameplayServiceUrl;

if (builder.HostEnvironment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовуються підключення через localhost
	blazorServerUrl = "https://localhost:7187";
	gameplayServiceUrl = "https://localhost:7251";
	// When running the project in a development environment, connections via localhost are used
}
else
{
	// В інших випадках використовуються змінна середовища
	blazorServerUrl = Environment.GetEnvironmentVariable("BLAZOR_SERVER_URL") ?? throw new Exception("BLAZOR_SERVER_URL is not set");
	gameplayServiceUrl = Environment.GetEnvironmentVariable("GAMEPLAY_URL") ?? throw new Exception("GAMEPLAY_URL is not set");
	// In other cases, environment variables are used
}

Console.WriteLine($"BLAZOR_SERVER_URL = {blazorServerUrl}");
Console.WriteLine($"GAMEPLAY_URL = {gameplayServiceUrl}");

builder.Services.AddHttpClient("Frontend.ServerAPI", client => client.BaseAddress = new Uri(blazorServerUrl))
	.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddHttpClient("GamePlayServiceAPI", client => client.BaseAddress = new Uri(gameplayServiceUrl));

builder.Services.AddScoped<GameRecordService>();
builder.Services.AddScoped<UserServiceClient>();
builder.Services.AddScoped<GameHubClient>();

builder.Services.AddMudServices();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
