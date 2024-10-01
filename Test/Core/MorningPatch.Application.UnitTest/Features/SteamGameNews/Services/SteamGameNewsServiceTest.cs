namespace MorningPatch.Application.UnitTest.Features.SteamGameNews.Services;
using Moq;
using Moq.Protected;
using MorningPatch.Application.Features.SteamGameNews.Services;
using MorningPatch.Domain.Entities;
using System.Net;
using System.Text.Json;

[TestFixture]
public sealed class SteamGameNewsServiceTest
{
	private Mock<IHttpClientFactory> mockHttpClientFactory;
	private SteamGameNewsService steamGameNewsService;
	private SteamGame steamGame;
	private string steamGameNewsJsonResponse;

	[SetUp]
	public void SetUp()
	{
		mockHttpClientFactory = new Mock<IHttpClientFactory>();
		steamGameNewsService = new SteamGameNewsService(mockHttpClientFactory.Object);
		steamGame = new SteamGame
		{
			AppId = 10,
			Name = "Counter-Strike",
			ImageIconHash = "6b0312cda02f5f777efa2f3318c307ff9acafbb5",
		};

		steamGameNewsJsonResponse = JsonSerializer.Serialize(new
		{
			appnews = new
			{
				appid = 10,
				newsitems = new[]
				{
					new
					{
						gid = "5963413728335624909",
						title = "Counter-Strike creator regrets not balancing the AWP",
						url = "https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909",
						is_external_url = true,
						author = "editor@pcgamesn.com",
						contents = "",
						feedlabel = "PCGamesN",
						date = DateTimeOffset.UtcNow.AddDays(0).ToUnixTimeSeconds(),
						feedname = "PCGamesN",
						feed_type = 0,
						appid = 10
					},
					new
					{
						gid = "5969041959965161740",
						title = "Counter-Strike co-creator says he's happy they sold the game to Valve—'They have done a great job of maintaining the legacy'",
						url = "https://steamstore-a.akamaihd.net/news/externalpost/PC Gamer/5969041959965161740",
						is_external_url = true,
						author = "Rich Stanton",
						contents = "",
						feedlabel = "PC Gamer",
						date = DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeSeconds(),
						feedname = "PC Gamer",
						feed_type = 0,
						appid = 10
					},
					new
					{
						gid = "5395938616813185893",
						title = "December 12, 2023 Update",
						url = "https://steamstore-a.akamaihd.net/news/externalpost/steam_community_announcements/5395938616813185893",
						is_external_url = true,
						author = "benb",
						contents = "",
						feedlabel = "Community Announcement",
						date = DateTimeOffset.UtcNow.AddDays(-14).ToUnixTimeSeconds(),
						feedname = "Community Announcement",
						feed_type = 1,
						appid = 10
					},
				}
			}
		});
	}

	[Test]
	[TestCase(-1, 1)]
	[TestCase(-7, 2)]
	[TestCase(-14, 3)]
	public async Task GivenDayOffset_WhenGetNewsForGameAsync_ThenReturnsGameNewsWithinDayOffest(int dayOffset, int expectedGameNewsCount)
	{
		var httpClient = SteamGameNewsServiceTest.CreateHttpClient(steamGameNewsJsonResponse, HttpStatusCode.OK);
		mockHttpClientFactory.Setup(mock => mock.CreateClient(It.IsAny<string>()))
							 .Returns(httpClient);

		var startUnixTimestamp = DateTimeOffset.UtcNow.AddDays(dayOffset).ToUnixTimeSeconds();
		var response = (await steamGameNewsService.GetNewsForGameAsync(steamGame, startUnixTimestamp)).ToList();

		Assert.That(response, Has.Count.EqualTo(expectedGameNewsCount));

		mockHttpClientFactory.Verify(mock => mock.CreateClient(It.IsAny<string>()), Times.Once);
	}

	[Test]
	public async Task WhenGetNewsForGameAsyncAndHttpRequestFails_ThenThrowsHttpRequestException()
	{
		var httpClient = SteamGameNewsServiceTest.CreateHttpClient("", HttpStatusCode.InternalServerError);
		mockHttpClientFactory.Setup(mock => mock.CreateClient(It.IsAny<string>()))
							 .Returns(httpClient);

		var startUnixTimestamp = DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeSeconds();

		Assert.ThrowsAsync<HttpRequestException>(() => steamGameNewsService.GetNewsForGameAsync(steamGame, startUnixTimestamp));

		mockHttpClientFactory.Verify(mock => mock.CreateClient(It.IsAny<string>()), Times.Once);
	}

	private static HttpClient CreateHttpClient(string jsonResponseContent, HttpStatusCode statusCode)
	{
		var httpResponseMessage = new HttpResponseMessage
		{
			StatusCode = statusCode,
			Content = new StringContent(jsonResponseContent)
		};

		var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
		mockHttpMessageHandler.Protected()
							  .Setup<Task<HttpResponseMessage>>("SendAsync",
																ItExpr.IsAny<HttpRequestMessage>(),
																ItExpr.IsAny<CancellationToken>())
							  .ReturnsAsync(httpResponseMessage);

		return new HttpClient(mockHttpMessageHandler.Object);
	}
}