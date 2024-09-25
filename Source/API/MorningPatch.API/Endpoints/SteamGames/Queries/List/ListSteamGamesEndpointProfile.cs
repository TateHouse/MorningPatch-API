namespace MorningPatch.API.Endpoints.SteamGames.Queries.List;
using AutoMapper;
using MorningPatch.API.Endpoints.SteamGames.Queries.List.DTO;
using MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;

/**
 * <summary>
 * An AutoMapper <see cref="Profile"/> for the <see cref="ListSteamGamesEndpoint"/>.
 * </summary>
 */
public sealed class ListSteamGamesEndpointProfile : Profile
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="ListSteamGamesEndpointProfile"/> instance.
	 * </summary>
	 */
	public ListSteamGamesEndpointProfile()
	{
		CreateMap<ListSteamGamesQuerySteamGameResponse, ListSteamGamesEndpointSteamGameResponse>();
		CreateMap<ListSteamGamesQueryResponse, ListSteamGamesEndpointResponse>();
	}
}