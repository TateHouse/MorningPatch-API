namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the data returned by the <see cref="ListPreviousDaysNewsEndpoint"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsEndpointResponse
{
	public required IReadOnlyList<ListPreviousDaysNewsEndpointNewsResponse> News { get; init; }
}