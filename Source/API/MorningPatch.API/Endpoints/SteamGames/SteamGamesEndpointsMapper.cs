namespace MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.API.Endpoints.SteamGames.Commands.Update;

/**
 * <summary>
 * Maps teh <see cref="MorningPatch.Domain.Entities.SteamGame"/> related endpoints.
 * </summary>
 */
public static class SteamGamesEndpointsMapper
{
	/**
	 * <summary>
	 * The shared prefix for all <see cref="MorningPatch.Domain.Entities.SteamGame"/> related endpoints.
	 * </summary>
	 */
	public const string Prefix = "/api/v1/steam-games/";
	private readonly static string[] Tags = ["Steam Games"];

	/**
	 * <summary>
	 * An extension method for <see cref="WebApplication"/> to map the
	 * <see cref="MorningPatch.Domain.Entities.SteamGame"/> related endpoints.
	 * </summary>
	 */
	public static void MapSteamGamesEndpointsMapper(this WebApplication application)
	{
		var groupBuilder = application.MapGroup(SteamGamesEndpointsMapper.Prefix);

		UpdateSteamGamesEndpoint.MapEndpoint(groupBuilder, SteamGamesEndpointsMapper.Tags);
	}
}