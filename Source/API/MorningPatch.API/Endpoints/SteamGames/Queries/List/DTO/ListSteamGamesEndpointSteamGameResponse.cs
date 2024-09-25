namespace MorningPatch.API.Endpoints.SteamGames.Queries.List.DTO;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Entities.SteamGame"/> data provided in the
 * response for the <see cref="ListSteamGamesEndpoint"/>.
 * </summary>
 */
public sealed record ListSteamGamesEndpointSteamGameResponse
{
	public required int AppId { get; init; }
	public required string Name { get; init; }
	public required string ImageIconHash { get; init; }
}