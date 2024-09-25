namespace MorningPatch.Application.Features.SteamGames.Queries.List;
using AutoMapper;
using MediatR;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;
using MorningPatch.Domain.Entities;

/**
 * <summary>
 * The query handler for the <see cref="ListSteamGamesQuery"/>.
 * </summary>
 */
public sealed class ListSteamGamesQueryHandler : IRequestHandler<ListSteamGamesQuery, ListSteamGamesQueryResponse>
{
	private readonly IMapper mapper;
	private readonly IReadOnlyRepository<SteamGame> steamGamesRepository;

	/**
	 * <summary>
	 * Instantiates a new <see cref="ListSteamGamesQueryHandler"/> instance.
	 * </summary>
	 * <param name="mapper">The mapper to use.</param>
	 * <param name="steamGamesRepository">A read-only repository of <see cref="SteamGame"/> entities.</param>
	 */
	public ListSteamGamesQueryHandler(IMapper mapper, IReadOnlyRepository<SteamGame> steamGamesRepository)
	{
		this.mapper = mapper;
		this.steamGamesRepository = steamGamesRepository;
	}

	public async Task<ListSteamGamesQueryResponse> Handle(ListSteamGamesQuery request, CancellationToken cancellationToken)
	{
		var steamGames = await steamGamesRepository.ListAsync(cancellationToken);

		return mapper.Map<ListSteamGamesQueryResponse>(steamGames);
	}
}