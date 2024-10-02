namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Models.SteamGameNews"/> data provided in the
 * response for the <see cref="ListPreviousDaysNewsQuery"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsQuerySteamGameResponse
{
	public required int AppId { get; init; }
	public required string Name { get; init; }
	public required string ImageIconHash { get; init; }
}