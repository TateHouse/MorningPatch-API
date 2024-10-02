namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the data returned by the <see cref="ListPreviousDaysNewsQuery"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsQueryResponse
{
	public required IReadOnlyList<ListPreviousDaysNewsQueryNewsResponse> News { get; init; }
}