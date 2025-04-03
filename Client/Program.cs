using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend.Client;
using Frontend.Client.Services;
using MudBlazor.Services;
using System.Net.Http.Json;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Log.Logger = new LoggerConfiguration()
	.WriteTo.GrafanaLoki("http://loki-stack.monitoring.svc.cluster.local:3100")
	.Enrich.WithProperty("Service", "BlazorWASM")
	.CreateLogger();

Log.Information("Blazor WASM Client starting...");

string blazorServerUrl;
string gameplayServiceUrl;

using var httpClient = new HttpClient();
var config = await httpClient.GetFromJsonAsync<BlazorConfig>("blazor-config.json") ?? throw new InvalidOperationException("Failed to load configuration");

if (string.IsNullOrWhiteSpace(config.BLAZOR_SERVER_URL) || string.IsNullOrWhiteSpace(config.GAMEPLAY_URL))
{
	Log.Error("Invalid config: BLAZOR_SERVER_URL = '{BLAZOR_SERVER_URL}', GAMEPLAY_URL = '{GAMEPLAY_URL}'",
		config.BLAZOR_SERVER_URL, config.GAMEPLAY_URL);
	throw new Exception("Invalid config values!");
}

Log.Information("Loaded config: BLAZOR_SERVER_URL = {BLAZOR_SERVER_URL}, GAMEPLAY_URL = {GAMEPLAY_URL}",
	config.BLAZOR_SERVER_URL, config.GAMEPLAY_URL);

if (builder.HostEnvironment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовуються підключення через localhost
	blazorServerUrl = "https://localhost:7187";
	gameplayServiceUrl = "https://localhost:7251";
	Log.Information("Running in Development mode.");
	// When running the project in a development environment, connections via localhost are used
}
else
{
	// В інших випадках використовуються змінна середовища
	blazorServerUrl = config.BLAZOR_SERVER_URL ?? throw new Exception("BLAZOR_SERVER_URL is not set");
	gameplayServiceUrl = config.GAMEPLAY_URL ?? throw new Exception("GAMEPLAY_URL is not set");
	Log.Information("Running in Production mode.");
	// In other cases, environment variables are used
}

builder.Services.AddHttpClient("Frontend.ServerAPI", client => client.BaseAddress = new Uri(blazorServerUrl))
	.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddHttpClient("GamePlayServiceAPI", client => client.BaseAddress = new Uri(gameplayServiceUrl));

builder.Services.AddScoped<GameRecordService>();
builder.Services.AddScoped<UserServiceClient>();
builder.Services.AddScoped<GameHubClient>();

builder.Services.AddMudServices();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();

class BlazorConfig
{
	public string BLAZOR_SERVER_URL { get; set; } = "";
	public string GAMEPLAY_URL { get; set; } = "";
}