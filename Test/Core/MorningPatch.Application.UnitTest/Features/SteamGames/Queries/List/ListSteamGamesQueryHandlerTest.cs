namespace MorningPatch.Application.UnitTest.Features.SteamGames.Queries.List;
using Moq;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGames.Queries.List;
using MorningPatch.Application.UnitTest.Utilities;
using MorningPatch.Domain.Entities;

[TestFixture]
public sealed class ListSteamGamesQueryHandlerTest
{
	private Mock<IReadOnlyRepository<SteamGame>> mockSteamGamesRepository;
	private ListSteamGamesQueryHandler queryHandler;

	[SetUp]
	public void SetUp()
	{
		mockSteamGamesRepository = new Mock<IReadOnlyRepository<SteamGame>>();
		var mapper = MapperFactory.Create<ListSteamGamesQueryProfile>();
		queryHandler = new ListSteamGamesQueryHandler(mapper, mockSteamGamesRepository.Object);
	}

	[Test]
	public async Task WhenHandleAndDatabaseIsEmpty_ThenReturnsNoSteamGames()
	{
		var steamGames = new List<SteamGame>();

		mockSteamGamesRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
								.ReturnsAsync(steamGames);

		var request = new ListSteamGamesQuery();
		var response = await queryHandler.Handle(request, CancellationToken.None);

		Assert.That(response.SteamGames, Is.Empty);

		mockSteamGamesRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Test]
	public async Task WhenHandleAndDatabaseIsNotEmpty_ThenReturnsSteamGames()
	{
		var steamGames = new List<SteamGame>
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

		mockSteamGamesRepository.Setup(mock => mock.ListAsync(It.IsAny<CancellationToken>()))
								.ReturnsAsync(steamGames);

		var request = new ListSteamGamesQuery();
		var response = await queryHandler.Handle(request, CancellationToken.None);

		Assert.That(response.SteamGames.Count, Is.EqualTo(steamGames.Count));

		mockSteamGamesRepository.Verify(mock => mock.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
	}
}