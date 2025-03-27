using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Frontend.Server.Data;
using Frontend.Server.Models;
using Frontend.Server.Services;

var builder = WebApplication.CreateBuilder(args);

string dbConnection;

if (builder.Environment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовується підключення через localhost
	dbConnection = builder.Configuration.GetConnectionString("PostgresLocalhostConnection")!;
	// When running the project in a development environment, connection via localhost are used
}
else
{
	// В інших випадках використовується змінна середовища
	dbConnection = Environment.GetEnvironmentVariable("BLAZORSERVER_DB_CONNECTION")!;
	// In other cases, environment variable are used
}

// В проєкті використовується PostgreSQL та "ліниве завантаження"
builder.Services.AddDbContext<UserDbContext>(options =>
	options.UseLazyLoadingProxies().UseNpgsql(dbConnection));
// The project uses PostgreSQL and lazy loading

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSwaggerGen();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<UserDbContext>();

builder.Services.AddIdentityServer()
	.AddApiAuthorization<User, UserDbContext>();

builder.Services.AddAuthentication()
	.AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
	await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
	app.UseWebAssemblyDebugging();
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
