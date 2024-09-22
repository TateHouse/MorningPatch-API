namespace MorningPatch.API.IntegrationTest.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

/**
 * <summary>
 * A custom <see cref="WebApplicationFactory{TEntryPoint}"/> for the API integration tests.
 * </summary>
 */
public sealed class WebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureAppConfiguration((_, configurationBuiler) =>
		{
			configurationBuiler.AddJsonFile("AppSettings.Test.json", false, true);
			configurationBuiler.AddUserSecrets<Program>();
		});
	}
}