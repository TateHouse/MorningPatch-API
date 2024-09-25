namespace MorningPatch.Application.Features.SteamGames.Queries.List;
using AutoMapper;
using MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;
using MorningPatch.Domain.Entities;

/**
 * <summary>
 * The AutoMapper <see cref="Profile"/> for the <see cref="ListSteamGamesQuery"/>.
 * </summary>
 */
public sealed class ListSteamGamesQueryProfile : Profile
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="ListSteamGamesQueryProfile"/> instance.
	 * </summary>
	 */
	public ListSteamGamesQueryProfile()
	{
		CreateMap<SteamGame, ListSteamGamesQuerySteamGameResponse>();
		CreateMap<IEnumerable<SteamGame>, ListSteamGamesQueryResponse>()
			.ForMember(destinationMember => destinationMember.SteamGames,
					   memberOptions => memberOptions.MapFrom(sourceMember => sourceMember));
	}
}