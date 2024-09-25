namespace MorningPatch.API.Endpoints.SteamGames.Queries.List.DTO;
/**
 * <summary>
 * A data transfer object containing the data returned by the <see cref="ListSteamGamesEndpoint"/>.
 * </summary>
 */
public sealed record ListSteamGamesEndpointResponse
{
	public required IReadOnlyList<ListSteamGamesEndpointSteamGameResponse> SteamGames { get; init; }
}