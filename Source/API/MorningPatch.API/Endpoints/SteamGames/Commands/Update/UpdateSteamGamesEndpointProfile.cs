namespace MorningPatch.API.Endpoints.SteamGames.Commands.Update;
using AutoMapper;
using MorningPatch.API.Endpoints.SteamGames.Commands.Update.DTOs;
using MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;

/**
 * <summary>
 * The AutoMapper <see cref="Profile"/> for hte <see cref="UpdateSteamGamesEndpoint"/>.
 * </summary>
 */
public sealed class UpdateSteamGamesEndpointProfile : Profile
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="UpdateSteamGamesEndpointProfile"/> instance.
	 * </summary>
	 */
	public UpdateSteamGamesEndpointProfile()
	{
		CreateMap<UpdateSteamGamesCommandResponse, UpdateSteamGamesEndpointResponse>();
	}
}