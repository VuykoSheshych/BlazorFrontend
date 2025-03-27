using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Frontend.Server.Models;

namespace Frontend.Server.Data;

public class UserDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) :
	ApiAuthorizationDbContext<User>(options, operationalStoreOptions)
{
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<UserNotification> UserNotifications { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<UserNotification>()
			.HasKey(un => new { un.ReceiverId, un.NotificationId });

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Receiver)
			.WithMany(u => u.UserNotifications)
			.HasForeignKey(un => un.ReceiverId);

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Notification)
			.WithMany(n => n.UserNotifications)
			.HasForeignKey(un => un.NotificationId);
	}
}