namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Models.SteamGameNews"/> data provided in the
 * response for the <see cref="ListPreviousDaysNewsQuery"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsQueryNewsResponse
{
	public required ListPreviousDaysNewsQuerySteamGameResponse SteamGame { get; init; }
	public required string Title { get; init; }
	public required string Author { get; init; }
	public required long UnixTimestamp { get; init; }
	public required Uri Uri { get; init; }
}