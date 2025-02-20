using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Frontend.Server.Models;

namespace Frontend.Server.Data;

public class IdentityDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) :
	ApiAuthorizationDbContext<User>(options, operationalStoreOptions)
{

}
