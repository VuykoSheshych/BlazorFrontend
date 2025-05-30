using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BlazorFrontend.Features.Auth.Services;

public class IncludeCredentialsHandler : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
		return await base.SendAsync(request, cancellationToken);
	}
}
