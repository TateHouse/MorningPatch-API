namespace MorningPatch.API.IntegrationTest.Endpoints.SteamGames.Commands.Update;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.API.Endpoints.SteamGames.Commands.Update.DTOs;
using MorningPatch.Persistence;
using System.Net;
using System.Net.Http.Json;

[TestFixture]
public sealed class UpdateSteamGamesEndpointTest : EndpointTest
{
	[Test]
	public async Task WhenUpdateSteamGames_ThenReturnsResponseWithOkStatus()
	{
		var response = await PostAsync(SteamGamesEndpointsMapper.Prefix);

		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task WhenUpdateSteamGames_ThenReturnsResponseWithNumberOfGamesAddedToDatabase()
	{
		var response = await PostAsync(SteamGamesEndpointsMapper.Prefix);
		var content = await response.Content.ReadFromJsonAsync<UpdateSteamGamesEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.Count, Is.GreaterThan(0));
	}

	[Test]
	public async Task WhenUpdateSteamGamesAndNoGamesAreInDatabase_ThenNewGamesAreAddedToDatabase()
	{
		await PostAsync(SteamGamesEndpointsMapper.Prefix);

		var scope = WebApplicationFactory.Services.CreateAsyncScope();
		var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
		var steamGamesCount = await databaseContext.SteamGames.CountAsync();

		Assert.That(steamGamesCount, Is.GreaterThan(0));
	}

	[Test]
	public async Task WhenUpdateSteamGamesAndAtLeastOneGameIsNotInDatabase_ThenNewGamesAreAddedToDatabase()
	{
		await PostAsync(SteamGamesEndpointsMapper.Prefix);

		var scope = WebApplicationFactory.Services.CreateAsyncScope();
		var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
		var steamGame = await databaseContext.SteamGames.FirstOrDefaultAsync();

		if (steamGame == null)
		{
			Assert.Fail();
		}

		databaseContext.SteamGames.Remove(steamGame!);
		await databaseContext.SaveChangesAsync();

		var response = await PostAsync(SteamGamesEndpointsMapper.Prefix);
		var content = await response.Content.ReadFromJsonAsync<UpdateSteamGamesEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.Count, Is.EqualTo(1));
	}

	[Test]
	public async Task WhenUpdateSteamGamesAndAllGamesAreInDatabase_ThenNoGamesAreAddedToDatabase()
	{
		await PostAsync(SteamGamesEndpointsMapper.Prefix);
		var response = await PostAsync(SteamGamesEndpointsMapper.Prefix);
		var content = await response.Content.ReadFromJsonAsync<UpdateSteamGamesEndpointResponse>();

		Assert.That(content, Is.Not.Null);
		Assert.That(content.Count, Is.Zero);
	}
}