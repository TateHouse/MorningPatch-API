namespace MorningPatch.Application.Features.SteamGames.Commands.Update;
using AutoMapper;
using MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;

/**
 * <summary>
 * The AutoMapper <see cref="Profile"/> for hte <see cref="UpdateSteamGamesCommand"/>.
 * </summary>
 */
public sealed class UpdateSteamGamesCommandProfile : Profile
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="UpdateSteamGamesCommandProfile"/> instance.
	 * </summary>
	 */
	public UpdateSteamGamesCommandProfile()
	{
		CreateMap<int, UpdateSteamGamesCommandResponse>()
			.ForMember(destinationMember => destinationMember.Count,
					   memberOptions => memberOptions.MapFrom(sourceMember => sourceMember));
	}
}