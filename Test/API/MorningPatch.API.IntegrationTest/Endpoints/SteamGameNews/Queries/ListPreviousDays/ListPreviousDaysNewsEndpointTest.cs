namespace MorningPatch.API.IntegrationTest.Endpoints.SteamGameNews.Queries.ListPreviousDays;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MorningPatch.API.Endpoints.SteamGameNews;
using MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
using MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.Persistence;
using System.Net;
using System.Net.Http.Json;

[TestFixture]
public sealed class ListPreviousDaysNewsEndpointTest : EndpointTest
{
	[Test]
	public async Task GivenEmptyDatabase_WhenListPreviousDaysNews_ThenReturnsResponseWithOkStatus()
	{

		var response = await GetAsync(ListPreviousDaysNewsEndpointTest.BuildUrl(1));

		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task GivenEmptyDatabase_WhenListPreviousDaysNews_ThenReturnsNoSteamGameNews()
	{
		var response = await GetAsync(ListPreviousDaysNewsEndpointTest.BuildUrl(1));
		var content = await response.Content.ReadFromJsonAsync<ListPreviousDaysNewsEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.News, Is.Empty);
	}

	[Test]
	[TestCase(1)]
	[TestCase(7)]
	public async Task GivenNonEmptyDatabase_WhenListPreviousDaysNews_ThenReturnsSteamGameNews(int dayOffset)
	{
		await PostAsync(SteamGamesEndpointsMapper.Prefix);

		var scope = WebApplicationFactory.Services.CreateAsyncScope();
		var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
		var steamGameCount = await databaseContext.SteamGames.CountAsync();

		if (steamGameCount == 0)
		{
			Assert.Fail("No Steam games are stored in the database");
		}

		var startUnixTimestamp = DateTimeOffset.UtcNow.AddDays(-dayOffset).ToUnixTimeSeconds();
		var response = await GetAsync(ListPreviousDaysNewsEndpointTest.BuildUrl(dayOffset));
		var content = await response.Content.ReadFromJsonAsync<ListPreviousDaysNewsEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.News, Is.Not.Empty);
		Assert.Multiple(() =>
		{
			foreach (var gameNews in content.News)
			{
				var steamGame = gameNews.SteamGame;
				Assert.That(steamGame, Is.Not.Null);
				Assert.That(steamGame.Name, Is.Not.Empty);
				Assert.That(steamGame.ImageIconHash, Is.Not.Empty);

				Assert.That(gameNews.Title, Is.Not.Empty);
				Assert.That(gameNews.Uri.ToString(), Is.Not.Empty);
				Assert.That(gameNews.UnixTimestamp, Is.GreaterThanOrEqualTo(startUnixTimestamp));
			}
		});
	}

	private static string BuildUrl(int dayOffset)
	{
		return $"{SteamGameNewsEndpointsMapper.Prefix}?dayOffset={dayOffset}";
	}
}