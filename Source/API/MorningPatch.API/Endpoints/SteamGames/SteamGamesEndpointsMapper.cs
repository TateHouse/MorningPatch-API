namespace MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.API.Endpoints.SteamGames.Commands.Update;
using MorningPatch.API.Endpoints.SteamGames.Queries.List;

/**
 * <summary>
 * Maps the <see cref="MorningPatch.Domain.Entities.SteamGame"/> related endpoints.
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
	public static void MapSteamGamesEndpoints(this WebApplication application)
	{
		var groupBuilder = application.MapGroup(SteamGamesEndpointsMapper.Prefix);

		ListSteamGamesEndpoint.MapEndpoint(groupBuilder, SteamGamesEndpointsMapper.Tags);
		UpdateSteamGamesEndpoint.MapEndpoint(groupBuilder, SteamGamesEndpointsMapper.Tags);
	}
}