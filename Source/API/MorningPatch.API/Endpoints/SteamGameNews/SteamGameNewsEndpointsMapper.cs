namespace MorningPatch.API.Endpoints.SteamGameNews;
using MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays;

/**
 * <summary>
 * Maps the <see cref="MorningPatch.Domain.Models.SteamGameNews"/> related endpoints.
 * </summary>
 */
public static class SteamGameNewsEndpointsMapper
{
	/**
	 * <summary>
	 * The shared prefix for all <see cref="MorningPatch.Domain.Models.SteamGameNews"/> related endpoints.
	 * </summary>
	 */
	public const string Prefix = "/api/v1/steam-game-news/";
	private readonly static string[] Tags = ["Steam Game News"];

	/**
	 * <summary>
	 * An extension method for <see cref="WebApplication"/> to map the
	 * <see cref="MorningPatch.Domain.Models.SteamGameNews"/> related endpoints.
	 * </summary>
	 */
	public static void MapSteamGameNewsEndpoints(this WebApplication application)
	{
		var groupBuilder = application.MapGroup(SteamGameNewsEndpointsMapper.Prefix);

		ListPreviousDaysNewsEndpoint.MapEndpoint(groupBuilder, SteamGameNewsEndpointsMapper.Tags);
	}
}