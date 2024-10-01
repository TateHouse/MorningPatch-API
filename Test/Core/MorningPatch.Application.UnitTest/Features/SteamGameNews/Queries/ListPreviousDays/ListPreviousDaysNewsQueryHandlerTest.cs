namespace MorningPatch.Application.UnitTest.Features.SteamGameNews.Queries.ListPreviousDays;
using Moq;
using MorningPatch.Application.Abstractions.Application.SteamGameNews;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using MorningPatch.Application.UnitTest.Utilities;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;

[TestFixture]
public sealed class ListPreviousDaysNewsQueryHandlerTest
{
	private Mock<IReadOnlyRepository<SteamGame>> mockSteamGamesRepository;
	private Mock<ISteamGameNewsService> mockSteamGameNewsService;
	private ListPreviousDaysNewsQueryHandler queryHandler;
	private List<SteamGame> steamGames;
	private List<SteamGameNews> steamGameNews;

	[SetUp]
	public void SetUp()
	{
		var mapper = MapperFactory.Create<ListPreviousDaysNewsQueryProfile>();
		mockSteamGamesRepository = new Mock<IReadOnlyRepository<SteamGame>>();
		mockSteamGameNewsService = new Mock<ISteamGameNewsService>();
		queryHandler = new ListPreviousDaysNewsQueryHandler(mapper,
															mockSteamGamesRepository.Object,
															mockSteamGameNewsService.Object);

		steamGames = new List<SteamGame>
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

		steamGameNews = new List<SteamGameNews>
		{
			new SteamGameNews
			{
				AppId = steamGames[0].AppId,
				Name = steamGames[0].Name,
				ImageIconHash = steamGames[0].ImageIconHash,
				Title = "Counter-Strike creator regrets not balancing the AWP",
				Author = "editor@pcgamesn.com",
				Uri = new Uri("https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909"),
				UnixTimestamp = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds()
			},
			new SteamGameNews
			{
				AppId = steamGames[0].AppId,
				Name = steamGames[0].Name,
				ImageIconHash = steamGames[0].ImageIconHash,
				Title = "Counter-Strike co-creator says he's happy they sold the game to Valve—'They have done a great job of maintaining the legacy'",
				Author = "Rich Stanton",
				Uri = new Uri("https://steamstore-a.akamaihd.net/news/externalpost/PC Gamer/5969041959965161740"),
				UnixTimestamp = DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeSeconds()
			},
			new SteamGameNews
			{
				AppId = steamGames[1].AppId,
				Name = steamGames[1].Name,
				ImageIconHash = steamGames[1].ImageIconHash,
				Title = "August 2024 Patch",
				Author = "Rubat",
				Uri = new Uri("https://steamstore-a.akamaihd.net/news/externalpost/steam_community_announcements/5963414363394124821"),
				UnixTimestamp = DateTimeOffset.UtcNow.AddDays(-2).ToUnixTimeSeconds()
			},
			new SteamGameNews
			{
				AppId = steamGames[2].AppId,
				Name = steamGames[2].Name,
				ImageIconHash = steamGames[2].ImageIconHash,
				Title = "Left 4 Dead - Update",
				Author = "Kerry",
				Uri = new Uri("https://steamstore-a.akamaihd.net/news/externalpost/steam_community_announcements/6890025005346883818"),
				UnixTimestamp = DateTimeOffset.UtcNow.AddDays(-3).ToUnixTimeSeconds()
			},
		};
	}

	[Test]
	public async Task WhenHandleAndDatabaseContainsNoSteamGames_ThenReturnsNoGameNews()
	{
		mockSteamGamesRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
								.ReturnsAsync(new List<SteamGame>());

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(It.IsAny<SteamGame>(), It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>());

		var request = new ListPreviousDaysNewsQuery
		{
			DayOffset = -1
		};

		var response = await queryHandler.Handle(request, CancellationToken.None);

		Assert.That(response, Is.Not.Null);
		Assert.That(response.News, Has.Count.EqualTo(0));

		mockSteamGamesRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(It.IsAny<SteamGame>(), It.IsAny<long>()), Times.Never);
	}

	[Test]
	public async Task GivenOneDayOffset_WhenHandleAndDatabaseContainsSteamGames_ThenReturnsGameNewsForGamesForPreviousDay()
	{
		mockSteamGamesRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
								.ReturnsAsync(steamGames);

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[0], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>
								{
									steamGameNews[0],
								});

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[1], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>());

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[2], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>());

		var request = new ListPreviousDaysNewsQuery
		{
			DayOffset = -1
		};

		var response = await queryHandler.Handle(request, CancellationToken.None);

		Assert.That(response, Is.Not.Null);
		Assert.That(response.News, Has.Count.EqualTo(1));

		mockSteamGamesRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[0], It.IsAny<long>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[1], It.IsAny<long>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[2], It.IsAny<long>()), Times.Once);
	}

	[Test]
	public async Task GivenOneWeekOffset_WhenHandleAndDatabaseContainsSteamGames_ThenReturnsGameNewsForPreviousWeek()
	{
		mockSteamGamesRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
								.ReturnsAsync(steamGames);

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[0], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>
								{
									steamGameNews[0],
									steamGameNews[1]
								});

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[1], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>
								{
									steamGameNews[2]
								});

		mockSteamGameNewsService.Setup(mock => mock.GetNewsForGameAsync(steamGames[2], It.IsAny<long>()))
								.ReturnsAsync(new List<SteamGameNews>
								{
									steamGameNews[3],
								});

		var request = new ListPreviousDaysNewsQuery
		{
			DayOffset = -7
		};

		var response = await queryHandler.Handle(request, CancellationToken.None);

		Assert.That(response, Is.Not.Null);
		Assert.That(response.News, Has.Count.EqualTo(4));

		mockSteamGamesRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[0], It.IsAny<long>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[1], It.IsAny<long>()), Times.Once);
		mockSteamGameNewsService.Verify(mock => mock.GetNewsForGameAsync(steamGames[2], It.IsAny<long>()), Times.Once);
	}
}