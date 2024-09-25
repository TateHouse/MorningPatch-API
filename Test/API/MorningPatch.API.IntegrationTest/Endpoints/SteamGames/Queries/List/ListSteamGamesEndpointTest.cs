namespace MorningPatch.API.IntegrationTest.Endpoints.SteamGames.Queries.List;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.API.Endpoints.SteamGames.Queries.List.DTO;
using MorningPatch.Persistence;
using System.Net;
using System.Net.Http.Json;

[TestFixture]
public sealed class ListSteamGamesEndpointTest : EndpointTest
{
	[Test]
	public async Task WhenListSteamGames_ThenReturnsResponseWithOkStatus()
	{
		var response = await GetAsync(SteamGamesEndpointsMapper.Prefix);

		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task WhenListSteamGamesAndDatabaseIsEmpty_ThenReturnsNoSteamGames()
	{
		var response = await GetAsync(SteamGamesEndpointsMapper.Prefix);
		var content = await response.Content.ReadFromJsonAsync<ListSteamGamesEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.SteamGames, Is.Empty);
	}

	[Test]
	public async Task WhenListSteamGamesAndDatabaseIsNotEmpty_ThenReturnsSteamGames()
	{
		await PostAsync(SteamGamesEndpointsMapper.Prefix);
		var response = await GetAsync(SteamGamesEndpointsMapper.Prefix);
		var content = await response.Content.ReadFromJsonAsync<ListSteamGamesEndpointResponse>();
		var scope = WebApplicationFactory.Services.CreateAsyncScope();
		var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
		var steamGameCount = await databaseContext.SteamGames.CountAsync();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.SteamGames, Has.Count.EqualTo(steamGameCount));
	}
}