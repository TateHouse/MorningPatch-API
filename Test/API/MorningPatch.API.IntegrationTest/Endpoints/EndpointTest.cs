namespace MorningPatch.API.IntegrationTest.Endpoints;
using MorningPatch.API.IntegrationTest.Utilities;

/**
 * <summary>
 * An abstract class that all endpoint tests derive from.
 * </summary>
 */
public abstract class EndpointTest
{
	/**
	 * The custom <see cref="WebApplicationFactory"/>.
	 */
	protected WebApplicationFactory WebApplicationFactory { get; private set; }

	[SetUp]
	public virtual async Task SetUpAsync()
	{
		WebApplicationFactory = new WebApplicationFactory();
	}

	[TearDown]
	public virtual async Task TearDownAsync()
	{
		await WebApplicationFactory.DisposeAsync();
	}

	/**
	 * <summary>
	 * Asynchronously sends a POST request to the specified <paramref name="url"/>.
	 * </summary>
	 * <param name="url">The location of the endpoint.</param>
	 * <returns>A task that represents the asynchronous operation, and it contains the <see cref="HttpResponseMessage"/>
	 * returned by the endpoint.</returns>
	 */
	protected async Task<HttpResponseMessage> PostAsync(string url)
	{
		using var client = WebApplicationFactory.CreateClient();

		return await client.PostAsync(url, null);
	}
}