namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using AutoMapper;
using MediatR;
using MorningPatch.Application.Abstractions.Application.SteamGameNews;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
using MorningPatch.Domain.Entities;

/**
 * <summary>
 * The query handler for the <see cref="ListPreviousDaysNewsQuery"/>.
 * </summary>
 */
public sealed class ListPreviousDaysNewsQueryHandler : IRequestHandler<ListPreviousDaysNewsQuery, ListPreviousDaysNewsQueryResponse>

{
	private readonly IMapper mapper;
	private readonly IReadOnlyRepository<SteamGame> steamGamesRepository;
	private readonly ISteamGameNewsService steamGameNewsService;

	/**
	 * <summary>
	 * Instantiates a new <see cref="ListPreviousDaysNewsQueryHandler"/> instance.
	 * </summary>
	 * <param name="mapper">The mapper to use.</param>
	 * <param name="steamGamesRepository">A read-only repository of <see cref="SteamGame"/> entities.</param>
	 * <param name="steamGameNewsService">The service to retrieve the news for each game with.</param>
	 */
	public ListPreviousDaysNewsQueryHandler(IMapper mapper,
											IReadOnlyRepository<SteamGame> steamGamesRepository,
											ISteamGameNewsService steamGameNewsService)
	{
		this.mapper = mapper;
		this.steamGamesRepository = steamGamesRepository;
		this.steamGameNewsService = steamGameNewsService;
	}

	public async Task<ListPreviousDaysNewsQueryResponse> Handle(ListPreviousDaysNewsQuery request,
																CancellationToken cancellationToken)
	{
		var games = await steamGamesRepository.ListAsync(cancellationToken);
		var tasks = games.Select(async game => await steamGameNewsService.GetNewsForGameAsync(game, request.DayOffset));
		var news = await Task.WhenAll(tasks);

		return mapper.Map<ListPreviousDaysNewsQueryResponse>(news.SelectMany(news => news).ToList());
	}
}