namespace MorningPatch.Application.UnitTest.Features.SteamGames.Commands.Update;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGames.Commands.Update;
using MorningPatch.Application.UnitTest.Utilities;
using MorningPatch.Domain.Entities;
using System.Net;
using JsonSerializer=System.Text.Json.JsonSerializer;

[TestFixture]
public sealed class UpdateSteamGamesCommandHandlerTest
{
	private const string ApiKey = "API_KEY";
	private const string SteamId = "STEAM_ID";

	private Mock<IRepository<SteamGame>> mockSteamGameRepository;
	private Mock<IHttpClientFactory> mockHttpClientFactory;
	private Mock<IConfiguration> mockConfiguration;
	private UpdateSteamGamesCommandHandler commandHandler;

	[SetUp]
	public void SetUp()
	{
		Environment.SetEnvironmentVariable("STEAM_WEB_API_KEY", UpdateSteamGamesCommandHandlerTest.ApiKey);
		Environment.SetEnvironmentVariable("STEAM_ID", UpdateSteamGamesCommandHandlerTest.SteamId);

		var mapper = MapperFactory.Create<UpdateSteamGamesCommandProfile>();
		mockSteamGameRepository = new Mock<IRepository<SteamGame>>();
		mockHttpClientFactory = new Mock<IHttpClientFactory>();
		mockConfiguration = new Mock<IConfiguration>();
		commandHandler = new UpdateSteamGamesCommandHandler(mapper,
															mockSteamGameRepository.Object,
															mockHttpClientFactory.Object,
															mockConfiguration.Object);

		mockConfiguration.Setup(mock => mock["STEAM_WEB_API_KEY"])
						 .Returns(UpdateSteamGamesCommandHandlerTest.ApiKey);

		mockConfiguration.Setup(mock => mock["STEAM_ID"])
						 .Returns(UpdateSteamGamesCommandHandlerTest.SteamId);

		var currentSteamGames = new List<SteamGame>
		{
			new SteamGame
			{
				AppId = 10,
				Name = "Counter-Strike",
				ImageIconHash = "6b0312cda02f5f777efa2f3318c307ff9acafbb5"
			},
			new SteamGame
			{
				AppId = 4000,
				Name = "Garry's Mod",
				ImageIconHash = "4a6f25cfa2426445d0d9d6e233408de4d371ce8b"
			},
			new SteamGame
			{
				AppId = 500,
				Name = "Left 4 Dead",
				ImageIconHash = "428df26bc35b09319e31b1ffb712487b20b3245c"
			}
		};

		mockSteamGameRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
							   .ReturnsAsync(currentSteamGames);

	}

	[TearDown]
	public void TearDown()
	{
		Environment.SetEnvironmentVariable("STEAM_WEB_API_KEY", null);
		Environment.SetEnvironmentVariable("STEAM_ID", null);
	}

	[Test]
	public async Task WhenHandle_ThenNewOwnedSteamGamesAreAddedToDatabase()
	{

		var steamGamesJsonResponse = JsonSerializer.Serialize(new
		{
			response = new
			{
				games = new[]
				{
					new
					{
						appid = 10,
						name = "Counter-Strike",
						img_icon_url = "6b0312cda02f5f777efa2f3318c307ff9acafbb5"
					},
					new
					{
						appid = 4000,
						name = "Garry's Mod",
						img_icon_url = "4a6f25cfa2426445d0d9d6e233408de4d371ce8b"
					},
					new
					{
						appid = 500,
						name = "Left 4 Dead",
						img_icon_url = "428df26bc35b09319e31b1ffb712487b20b3245c"
					},
					new
					{
						appid = 107410,
						name = "Arma 3",
						img_icon_url = "3212af52faf994c558bd622cb0f360c1ef295a6b"
					},
					new
					{
						appid = 427520,
						name = "Factorio",
						img_icon_url = "267f5a89f36ab287e600a4e7d4e73d3d11f0fd7d"
					}
				}
			}
		});

		var httpClient = UpdateSteamGamesCommandHandlerTest.CreateHttpClient(steamGamesJsonResponse, HttpStatusCode.OK);
		mockHttpClientFactory.Setup(mock => mock.CreateClient(It.IsAny<string>()))
							 .Returns(httpClient);

		var request = new UpdateSteamGamesCommand();
		var response = await commandHandler.Handle(request, CancellationToken.None);

		mockConfiguration.Verify(mock => mock["STEAM_WEB_API_KEY"], Times.Once);
		mockConfiguration.Verify(mock => mock["STEAM_ID"]);
		mockSteamGameRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameRepository.Verify(mock => mock.AddRangeAsync(It.IsAny<IEnumerable<SteamGame>>(), It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameRepository.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockHttpClientFactory.Verify(mock => mock.CreateClient(It.IsAny<string>()), Times.Once);

		Assert.That(response.Count, Is.EqualTo(2));
	}

	[Test]
	public async Task WhenHandleAndThereAreNoNewSteamGames_ThenTheDatabaseIsNotModified()
	{
		var steamGamesJsonResponse = JsonSerializer.Serialize(new
		{
			response = new
			{
				games = new[]
				{
					new
					{
						appid = 10,
						name = "Counter-Strike",
						img_icon_url = "6b0312cda02f5f777efa2f3318c307ff9acafbb5"
					},
					new
					{
						appid = 4000,
						name = "Garry's Mod",
						img_icon_url = "4a6f25cfa2426445d0d9d6e233408de4d371ce8b"
					},
					new
					{
						appid = 500,
						name = "Left 4 Dead",
						img_icon_url = "428df26bc35b09319e31b1ffb712487b20b3245c"
					}
				}
			}
		});

		var httpClient = UpdateSteamGamesCommandHandlerTest.CreateHttpClient(steamGamesJsonResponse, HttpStatusCode.OK);
		mockHttpClientFactory.Setup(mock => mock.CreateClient(It.IsAny<string>()))
							 .Returns(httpClient);

		var request = new UpdateSteamGamesCommand();
		var response = await commandHandler.Handle(request, CancellationToken.None);

		mockConfiguration.Verify(mock => mock["STEAM_WEB_API_KEY"], Times.Once);
		mockConfiguration.Verify(mock => mock["STEAM_ID"]);
		mockSteamGameRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameRepository.Verify(mock => mock.AddRangeAsync(It.IsAny<IEnumerable<SteamGame>>(), It.IsAny<CancellationToken>()), Times.Never);
		mockSteamGameRepository.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
		mockHttpClientFactory.Verify(mock => mock.CreateClient(It.IsAny<string>()), Times.Once);

		Assert.That(response.Count, Is.Zero);
	}

	[Test]
	public async Task WhenHandleAndSteamWebApiCallFails_ThenThrowsHttpRequestException()
	{
		var request = new UpdateSteamGamesCommand();
		var httpClient = UpdateSteamGamesCommandHandlerTest.CreateHttpClient("", HttpStatusCode.InternalServerError);

		mockHttpClientFactory.Setup(mock => mock.CreateClient(It.IsAny<string>()))
							 .Returns(httpClient);

		Assert.ThrowsAsync<HttpRequestException>(() => commandHandler.Handle(request, CancellationToken.None));

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