namespace MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Entities.SteamGame"/> data provided in the
 * response for the <see cref="ListSteamGamesQuery"/>.
 * </summary>
 */
public sealed record ListSteamGamesQuerySteamGameResponse
{
	public required int AppId { get; init; }
	public required string Name { get; init; }
	public required string ImageIconHash { get; init; }
}