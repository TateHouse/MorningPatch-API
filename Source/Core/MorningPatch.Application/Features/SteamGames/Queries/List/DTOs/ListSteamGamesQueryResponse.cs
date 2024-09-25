namespace MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;
/**
 * <summary>
 * A data transfer object containing the data returned by the <see cref="ListSteamGamesQuery"/>.
 * </summary>
 */
public sealed record ListSteamGamesQueryResponse
{
	public required IReadOnlyList<ListSteamGamesQuerySteamGameResponse> SteamGames { get; init; }
}